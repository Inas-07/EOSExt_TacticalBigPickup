using BepInEx;
using BepInEx.Unity.IL2CPP;
using ExtraObjectiveSetup.Utils;
using ExtraObjectiveSetup.JSON;
using ExtraObjectiveSetup.JSON.MTFOPartialData;
using HarmonyLib;
using EOSExt.TacticalBigPickup.Functions.EnemyTagger;
using EOSExt.TacticalBigPickup.Functions.FogBeacon.BigPickup;
using EOSExt.TacticalBigPickup.Functions.FogBeacon.LevelSpawned;

namespace EOSExt.TacticalBigPickup
{
    [BepInDependency("dev.gtfomodding.gtfo-api", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("GTFO.FloLib", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("Inas.ExtraObjectiveSetup", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency(MTFOPartialDataUtil.PLUGIN_GUID, BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency(InjectLibUtil.PLUGIN_GUID, BepInDependency.DependencyFlags.SoftDependency)]
    [BepInPlugin(AUTHOR + "." + PLUGIN_NAME, PLUGIN_NAME, VERSION)]
    
    public class EntryPoint: BasePlugin
    {
        public const string AUTHOR = "Inas";
        public const string PLUGIN_NAME = "EOSExt.TacticalBigPickup";
        public const string VERSION = "1.0.0";

        private Harmony m_Harmony;

        public override void Load()
        {
            SetupManagers();

            m_Harmony = new Harmony("EOSExt.TacticalBigPickup");
            m_Harmony.PatchAll();

            EOSLogger.Log("ExtraObjectiveSetup.TacticalBigPickup loaded.");
        }

        /// <summary>
        /// Explicitly invoke Init() to all managers to eager-load, which in the meantime defines chained puzzle creation order if any
        /// </summary>
        private void SetupManagers()
        {
            EnemyTaggerSettingManager.Current.Init();
            LevelSpawnedFogBeaconManager.Current.Init();
            BigPickupFogBeaconSettingManager.Current.Init();
        }
    }
}

