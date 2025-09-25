using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EOSExt.TacticalBigPickup.Managers
{
    public class BigPickupItemManager: PickupItemManager<CarryItemPickup_Core>
    {
        public static BigPickupItemManager Current { get; } = new();

        private BigPickupItemManager(): base()
        {
            
        }

        static BigPickupItemManager()
        {

        }
    }
}
