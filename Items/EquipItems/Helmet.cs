using GamePrototype.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamePrototype.Items.EquipItems
{
    internal class Helmet : EquipItem //add
    {
        public Helmet(uint defence, uint maxDurability, uint durability, string name) : base(maxDurability, durability, name) => Defence = defence;
        
        public uint Defence { get; }

        public override EquipSlot Slot => EquipSlot.Helmet;
    }
}
