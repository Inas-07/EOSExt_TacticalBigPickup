using EOSExt.TacticalBigPickup.Functions.FogBeacon.LevelSpawned;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EOSExt.TacticalBigPickup.Patches
{
    [HarmonyPatch]
    internal static class FixLevelSpawnedFogBeaconRange
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(HeavyFogRepellerGlobalState), nameof(HeavyFogRepellerGlobalState.AttemptInteract))]
        private static void Post_HeavyFogRepellerGlobalState_AttemptInteract(HeavyFogRepellerGlobalState __instance)
        {
            var LSFBDef = LevelSpawnedFogBeaconManager.Current.GetLSFBDef(__instance);
            if (LSFBDef == null) return;

            __instance.m_repellerSphere.Range = LSFBDef.Range;
        }
    }
}
