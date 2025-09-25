using ExtraObjectiveSetup.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EOSExt.TacticalBigPickup.Functions.Generic.BigPickup
{
    public class BigPickupCustom
    {
        public uint ItemId { get; set; } = 0;

        public List<BigPickupStates> BigPickups { get; set; } = new();
    }
}
