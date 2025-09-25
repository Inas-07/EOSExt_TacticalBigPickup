using EOSExt.TacticalBigPickup.Functions.Generic.BigPickup.Definition;
using ExtraObjectiveSetup.Utils;
using GameData;
using GTFO.API.Extensions;
using Il2CppInterop.Runtime.Injection;
using LevelGeneration;
using Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EOSExt.TacticalBigPickup.Functions.Generic.BigPickup
{
    public class BigPickupCustomHelper: MonoBehaviour
    {
        public ItemInLevel Item { get; private set; }
        
        private Dictionary<ePickupItemStatus, List<WardenObjectiveEventData>> eventsOnState = new();

        private void Setup(ItemInLevel item)
        {
            this.Item = item;
            item.GetSyncComponent().add_OnSyncStateChange((Il2CppSystem.Action<ePickupItemStatus, pPickupPlacement, PlayerAgent, bool>)OnSyncStateChange);
        }

        public bool TryGetEvents(ePickupItemStatus state, out List<WardenObjectiveEventData> events) => eventsOnState.TryGetValue(state, out events);

        private void OnSyncStateChange(ePickupItemStatus state, pPickupPlacement placement, PlayerAgent player, bool isRecall)
        {
            if (isRecall) return;

            if(TryGetEvents(state, out var events))
            {
                EOSLogger.Log($"item {Item.PublicName} on state {state}, executing {events.Count} events");
                WardenObjectiveManager.CheckAndExecuteEventsOnTrigger(events.ToIl2Cpp(), eWardenObjectiveEventTrigger.None, true);
            }
        }

        private void OnDestroy()
        {
            eventsOnState.Clear();
            eventsOnState = null;
        }

        private BigPickupCustomHelper()
        {

        }

        public static void Setup(ItemInLevel item, BigPickupCustomization states)
        {
            var h = item.gameObject.GetComponent<BigPickupCustomHelper>();

            if (h == null)
            {
                h = item.gameObject.AddComponent<BigPickupCustomHelper>();
                h.Setup(item);
            }

            foreach (var s in states.OnState)
            {
                if (!h.eventsOnState.TryGetValue(s.State, out var events))
                {
                    events = new();
                    h.eventsOnState[s.State] = events;
                }

                events.AddRange(s.EventsOnState);
            }
        }

        static BigPickupCustomHelper()
        {
            ClassInjector.RegisterTypeInIl2Cpp<BigPickupCustomHelper>();
        }
    }
}
