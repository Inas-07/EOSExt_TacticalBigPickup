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
using System;

namespace EOSExt.TacticalBigPickup.Functions.FogBeacon.BigPickup
{
    internal class BigPickupFogBeaconSettingManager: GenericDefinitionManager<BigPickupFogBeaconSetting>
    {
        public static BigPickupFogBeaconSettingManager Current { get; private set; }
        
        protected override string DEFINITION_NAME => "BigPickupFogBeacon_EOS";

        public override void Init() { }

        private BigPickupFogBeaconSettingManager() { }

        static BigPickupFogBeaconSettingManager()
        {
            Current = new();
        }
    }
}
