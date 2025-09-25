using GameData;
using LevelGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EOSExt.TacticalBigPickup.Functions.Generic.BigPickup.Definition
{
    public class BigPickupStateEvent
    {
        public ePickupItemStatus State { get; set; }

        public List<WardenObjectiveEventData> EventsOnState { get; set; } = new();
    }
}
