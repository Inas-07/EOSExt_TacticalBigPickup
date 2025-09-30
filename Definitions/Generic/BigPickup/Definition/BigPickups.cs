using ExtraObjectiveSetup.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EOSExt.TacticalBigPickup.Functions.Generic.BigPickup.Definition
{
    public class BigPickups
    {
        public uint ItemId { get; set; } = 0;

        public List<BigPickupCustomization> BigPickupItems { get; set; } = new() { new() };
    }
}
