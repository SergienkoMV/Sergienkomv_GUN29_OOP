﻿using GamePrototype.Utils;

namespace GamePrototype.Items.EquipItems
{
    public sealed class Armour : EquipItem
    {

        public Armour(uint defence, uint maxDurability/*add*/, uint durability, string name) : base(maxDurability /*add*/, durability, name) => Defence = defence;

        public uint Defence { get; }


        public override EquipSlot Slot => EquipSlot.Armour;
    }
}
