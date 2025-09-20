using EOSExt.TacticalBigPickup.Functions.EnemyTagger;
using EOSExt.TacticalBigPickup.Functions.FogBeacon.BigPickup;
using GameData;
using Gear;
using HarmonyLib;
using LevelGeneration;
using Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EOSExt.TacticalBigPickup.Patches
{
    [HarmonyPatch]
    public static class SetupBigPickupItemWithItemId
    {
        public const string BIG_PICKUP_FOG_BEACON_NAME = "Carry_FogBeacon - ConstantFog";
        public const string BIG_PICKUP_OBSERVER_NAME = "Carry_Observer";

        [HarmonyPostfix]
        [HarmonyPatch(typeof(LG_PickupItem), nameof(LG_PickupItem.SetupBigPickupItemWithItemId))]
        private static void Post_SetupBigPickupItemWithItemId(LG_PickupItem __instance, uint itemId)
        {
            string itemName = GameDataBlockBase<ItemDataBlock>.GetBlock(itemId)?.name ?? null;

            switch (itemName)
            {
                case BIG_PICKUP_FOG_BEACON_NAME:
                    BigPickupFogBeaconManager.SetupAsFogBeacon(__instance); break;
                case BIG_PICKUP_OBSERVER_NAME:
                    EnemyTaggerSettingManager.Current.SetupAsObserver(__instance);
                    break;
            }
        }

    }
}
