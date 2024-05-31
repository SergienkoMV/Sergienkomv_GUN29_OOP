using GamePrototype.Items.EconomicItems;
using GamePrototype.Utils;

namespace GamePrototype.Items.EquipItems
{
    public abstract class EquipItem : Item
    {
        private uint _durability;
        private uint _maxDurability;
        public uint Durability { get => _durability; protected set => _durability = value; }
        public override bool Stackable => false;

        public abstract EquipSlot Slot { get; }

        protected EquipItem(uint maxDurability, uint durability/*add*/, string name) : base(name) // => _maxDurability = maxDurability;
        {
            _maxDurability = maxDurability;
            _durability = durability; //add
        }

        public void ReduceDurability(uint delta) //=> _durability -= delta;
        {
            _durability -= delta;
            Console.WriteLine($"Durability of {this.Name} reduce in {delta}. Current durability {_durability}"); //add
        }

        public void Repair(uint delta) //=> 
        {
            uint deltaForWrite = _maxDurability - _durability; //add
            _durability /*+*/= _durability + delta > _maxDurability //изменил += на =, так как иначе _durability прибавлялось 2 раза
            ? _maxDurability
            : _durability + delta;
            
            Console.WriteLine($"The grindstone was used. Item {this.Name} was repair by {deltaForWrite}. Current durability is {_durability}");
        }
    }
}
