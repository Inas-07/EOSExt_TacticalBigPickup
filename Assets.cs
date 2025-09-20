using GTFO.API;
using UnityEngine;

namespace EOSExt.TacticalBigPickup
{
    internal static class Assets
    {
        public static GameObject OBSVisual { get; private set; }
        
        public static void Init()
        {
            OBSVisual = AssetAPI.GetLoadedAsset<GameObject>("Assets/SecuritySensor/OBSVisual.prefab");
        }
    }
}
