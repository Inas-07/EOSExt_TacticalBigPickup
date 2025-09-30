using EOSExt.TacticalBigPickup.Functions.Generic.BigPickup.Definition;
using EOSExt.TacticalBigPickup.Impl;
using EOSExt.TacticalBigPickup.Managers;
using ExtraObjectiveSetup.BaseClasses;
using ExtraObjectiveSetup.Utils;
using GTFO.API;
using LevelGeneration;
using Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EOSExt.TacticalBigPickup.Functions.Generic.BigPickup
{
    public class BigPickupCustomizationManager : GenericExpeditionDefinitionManager<BigPickups>
    {
        public static BigPickupCustomizationManager Current { get; } = new();

        protected override string DEFINITION_NAME => "BigPickupCustomization";

        private void Build(BigPickups def)
        {
            var itemsByZone = BigPickupItemManager.Current.GetItemsOf(def.ItemId);
            foreach(var b in def.BigPickupItems)
            {
                if(!itemsByZone.TryGetValue(b.GlobalZoneIndexTuple(), out var items))
                {
                    EOSLogger.Error($"EventsOnBigPickup: zone not found {b.GlobalZoneIndexTuple()}");
                    continue;
                }

                if(b.Index < 0 || b.Index >= items.Count)
                {
                    EOSLogger.Error($"EventsOnBigPickup: itemID {def.ItemId}, index {b.Index} is invalid - there're {items.Count} items in {b.GlobalZoneIndexTuple()} - valid value falls in range [0, {items.Count - 1})");
                    continue;
                }

                var item = items[b.Index];
                BigPickupCustomHelper.Setup(item, b);

                CustomBigPickupFunctionImplementor.SetupCustomBigPickupFunctions(item.GetLGPickupItem(), b.Functions);
            }
        }

        private void Build()
        {
            if (!definitions.TryGetValue(RundownManager.ActiveExpedition.LevelLayoutData, out var defs)) return;
            defs.Definitions.ForEach(Build);
        }

        public BigPickupCustomizationManager() : base() 
        {
            LevelAPI.OnBuildDone += Build;
        }
    }
}
