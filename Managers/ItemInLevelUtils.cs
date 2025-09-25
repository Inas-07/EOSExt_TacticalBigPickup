using GameData;
using LevelGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EOSExt.TacticalBigPickup.Managers
{
    public static class ItemInLevelUtils
    {
        public static (eDimensionIndex dim, LG_LayerType layer, eLocalZoneIndex localIndex) GetGlobalZoneIndex(this ItemInLevel item)
        {
            var sn = item.CourseNode;
            return (sn.m_dimension.DimensionIndex, sn.LayerType, sn.m_zone.LocalIndex);
        }

        public static LG_PickupItem GetLGPickupItem(this ItemInLevel item) => item.GetComponentInParent<LG_PickupItem>();
    }
}
