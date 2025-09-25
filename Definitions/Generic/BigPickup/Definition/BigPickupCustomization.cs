using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EOSExt.TacticalBigPickup.Functions.Generic.BigPickup.Definition
{
    public class BigPickupCustomization: ExtraObjectiveSetup.BaseClasses.GlobalZoneIndex
    {
        public int Index { get; set; } = 0;

        public List<string> Functions { get; set; } = new();

        public List<BigPickupStateEvent> OnState { get; set; } = new();
    }
}
