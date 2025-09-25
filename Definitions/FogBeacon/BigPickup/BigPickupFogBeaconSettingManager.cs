using GTFO.API.Utilities;
using System.IO;
using System.Collections.Generic;
using GTFO.API;
using ExtraObjectiveSetup.BaseClasses;
using ExtraObjectiveSetup.Utils;
using System.Linq;
using UnityEngine.Playables;
using Gear;
using LevelGeneration;
using Player;
using UnityEngine;

namespace EOSExt.TacticalBigPickup.Functions.FogBeacon.BigPickup
{
    internal class BigPickupFogBeaconSettingManager: GenericExpeditionDefinitionManager<BigPickupFogBeaconSetting>
    {
        public static BigPickupFogBeaconSettingManager Current { get; private set; }
        
        protected override string DEFINITION_NAME => "BigPickupFogBeacon";

        public static readonly BigPickupFogBeaconSetting DEDFAULT_SETTING = new BigPickupFogBeaconSetting();

        public BigPickupFogBeaconSetting SettingForCurrentLevel { private set; get; } = DEDFAULT_SETTING;

        public override void Init()
        {
            LevelAPI.OnBuildStart += UpdateSetting;
        }

        protected override void FileChanged(LiveEditEventArgs e)
        {
            base.FileChanged(e);
            if(GameStateManager.IsInExpedition)
            {
                UpdateSetting();
            }
        }

        private void UpdateSetting()
        {
            uint mainLevelLayout = RundownManager.ActiveExpedition.LevelLayoutData;
            SettingForCurrentLevel = definitions.TryGetValue(mainLevelLayout, out var s) && s.Definitions.Count > 0 ? 
                s.Definitions[0] /*TODO: for now I just want to make it a global setting which means there's only 1 setting */
                : DEDFAULT_SETTING;            
            EOSLogger.Debug($"FogBeaconSettingManager: updated setting for level with main level layout id {mainLevelLayout}");
        }

        private BigPickupFogBeaconSettingManager() { }

        static BigPickupFogBeaconSettingManager()
        {
            Current = new();
        }
    }
}
