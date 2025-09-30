using ExtraObjectiveSetup.BaseClasses;
using GameData;
using GTFO.API;
using LevelGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EOSExt.TacticalBigPickup.Managers
{
    public abstract class PickupItemManager<T> where T : ItemInLevel
    {
        // 考虑：bigpickup为“2选一”选一个大区刷出的情况，
        // 把GlobalZoneIndex作为索引的一部分，似乎不合理；但可以用它作为索引的情况也非常多
        // itemId -> spawned items
        protected Dictionary<uint, List<T>> RegisteredItems { get; private set; } = new();

        public virtual void Register(LG_PickupItem item)
        {
            var component = item.m_root.GetComponentInChildren<T>();
            Register(component);
        }

        public virtual void Register(T item)
        {
            var data = item.ItemDataBlock;
            if(!RegisteredItems.TryGetValue(data.persistentID, out var spawnedItems))
            {
                spawnedItems = new List<T>();
                RegisteredItems[data.persistentID] = spawnedItems;
            }

            spawnedItems.Add(item);
        }

        public virtual Dictionary<(eDimensionIndex dim, LG_LayerType layer, eLocalZoneIndex localIndex), List<T>> GetItemsOf(uint itemId) => RegisteredItems.TryGetValue(itemId, out var items) ? 
            items.GroupBy(item => item.GetGlobalZoneIndex()).ToDictionary(g => g.Key, g => g.ToList()) 
            : null;

        protected virtual void OnBuildDone()
        {

        }

        protected virtual void Clear()
        {
            RegisteredItems.Clear();
        }

        protected PickupItemManager()
        {
            LevelAPI.OnBuildDone += OnBuildDone;

            LevelAPI.OnBuildStart += Clear;
            LevelAPI.OnLevelCleanup += Clear;
        }

        static PickupItemManager()
        {

        }
    }
}
