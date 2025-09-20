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
    internal class BigPickupFogBeaconManager: GenericExpeditionDefinitionManager<BigPickupFogBeaconSetting>
    {
        public static BigPickupFogBeaconManager Current { get; private set; }
        
        protected override string DEFINITION_NAME => "BigPickupFogBeacon";

        public static readonly BigPickupFogBeaconSetting DEDFAULT_SETTING = new BigPickupFogBeaconSetting();

        public BigPickupFogBeaconSetting SettingForCurrentLevel { private set; get; } = DEDFAULT_SETTING;

        public override void Init()
        {
            LevelAPI.OnBuildStart += UpdateSetting;
        }

        protected override void FileChanged(LiveEditEventArgs e)
        {
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

        private BigPickupFogBeaconManager() { }

        static BigPickupFogBeaconManager()
        {
            Current = new();
        }

        public static void SetupAsFogBeacon(LG_PickupItem __instance)
        {
            FogRepeller_Sphere fogRepFake = new GameObject("FogInstance_Beacon_Fake").AddComponent<FogRepeller_Sphere>();
            fogRepFake.InfiniteDuration = false;
            fogRepFake.LifeDuration = 99999f;
            fogRepFake.GrowDuration = 99999f;
            fogRepFake.ShrinkDuration = 99999f;
            fogRepFake.Range = 1f;

            var setting = BigPickupFogBeaconManager.Current.SettingForCurrentLevel;
            FogRepeller_Sphere fogRepHold = new GameObject("FogInstance_Beacon_SmallLayer").AddComponent<FogRepeller_Sphere>();
            fogRepHold.InfiniteDuration = setting.RSHold.InfiniteDuration;
            fogRepHold.GrowDuration = setting.RSHold.GrowDuration;
            fogRepHold.ShrinkDuration = setting.RSHold.ShrinkDuration;
            fogRepHold.Range = setting.RSHold.Range;
            fogRepHold.Offset = Vector3.zero;

            FogRepeller_Sphere fogRepPlaced = new GameObject("FogInstance_Beacon_BigLayer").AddComponent<FogRepeller_Sphere>();
            fogRepPlaced.InfiniteDuration = setting.RSPlaced.InfiniteDuration;
            fogRepPlaced.GrowDuration = setting.RSPlaced.GrowDuration;
            fogRepPlaced.ShrinkDuration = setting.RSPlaced.ShrinkDuration;
            fogRepPlaced.Range = setting.RSPlaced.Range;
            fogRepPlaced.Offset = Vector3.zero;

            CarryItemPickup_Core core = __instance.m_root.GetComponentInChildren<CarryItemPickup_Core>();

            HeavyFogRepellerPickup fogRepellerPickup = core.Cast<HeavyFogRepellerPickup>();
            iCarryItemWithGlobalState itemWithGlobalState;
            byte index2;
            if (CarryItemWithGlobalStateManager.TryCreateItemInstance(eCarryItemWithGlobalStateType.FogRepeller, __instance.m_root, out itemWithGlobalState, out index2))
            {
                pItemData_Custom customData = fogRepellerPickup.GetCustomData() with
                {
                    byteId = index2
                };
                fogRepellerPickup.SetCustomData(customData, true);
            }

            HeavyFogRepellerGlobalState repellerGlobalState = itemWithGlobalState.Cast<HeavyFogRepellerGlobalState>();
            fogRepHold.transform.SetParent(repellerGlobalState.transform, false);
            fogRepPlaced.transform.SetParent(repellerGlobalState.transform, false);
            repellerGlobalState.m_repellerSphere = fogRepFake;

            // eliminate null ref
            fogRepHold.m_sphereAllocator = new();
            fogRepPlaced.m_sphereAllocator = new();

            Interact_Pickup_PickupItem interact = core.m_interact.Cast<Interact_Pickup_PickupItem>();
            interact.InteractDuration = setting.TimeToPickup;

            repellerGlobalState.CallbackOnStateChange += new System.Action<pCarryItemWithGlobalState_State, pCarryItemWithGlobalState_State, bool>((oldState, newState, isRecall) =>
            {
                if (isRecall)
                {
                    fogRepHold?.KillRepellerInstantly();
                    fogRepPlaced?.KillRepellerInstantly();
                    return;
                }

                switch ((eHeavyFogRepellerStatus)newState.status)
                {
                    case eHeavyFogRepellerStatus.Activated:
                        fogRepHold?.StartRepelling();

                        // eliminate StopRepelling() exception
                        if ((eHeavyFogRepellerStatus)oldState.status != eHeavyFogRepellerStatus.NoStatus)
                            fogRepPlaced?.StopRepelling();
                        interact.InteractDuration = setting.TimeToPlace;
                        break;
                    case eHeavyFogRepellerStatus.Deactivated:
                        fogRepHold?.StopRepelling();
                        fogRepPlaced?.StartRepelling();
                        interact.InteractDuration = setting.TimeToPickup;
                        break;
                }
            });
        }

    }
}
