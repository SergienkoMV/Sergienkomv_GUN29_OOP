using GamePrototype.Items.EconomicItems;
using GamePrototype.Items.EquipItems;
using GamePrototype.Utils;
using System.Text;

namespace GamePrototype.Units
{
    public sealed class Player : Unit
    {
        private readonly Dictionary<EquipSlot, EquipItem> _equipment = new();

        public Player(string name, uint health, uint maxHealth, uint baseDamage) : base(name, health, maxHealth, baseDamage)
        {            
        }

        public override uint GetUnitDamage()
        {
            if (_equipment.TryGetValue(EquipSlot.Weapon, out var item) && item is Weapon weapon) 
            {
                return BaseDamage + weapon.Damage;
            }
            return BaseDamage;
        }

        public override void HandleCombatComplete()
        {
            var items = Inventory.Items;
            for (int i = 0; i < items.Count; i++) 
            {
                if (items[i] is EconomicItem economicItem) 
                {
                    UseEconomicItem(economicItem);
                    if (Inventory.TryRemove(items[i]))  //change
                    {
                        i--; //add (если удается удалить предмет, то в итераторе становится на 1 меньше и тогда +1 предмет не попадает в перебор)
                    }
                }
            }
        }

        public override void AddItemToInventory(Item item)
        {
            //if (item is EquipItem equipItem && _equipment.TryAdd(equipItem.Slot, equipItem)) 
            //{
            //    // Item was equipped
            //    Console.WriteLine("Item was equipped");
            //    return;
            //}

            if (item is EquipItem equipItem)
            {
                //if (_equipment.TryGetValue(equipItem.Slot, out var value))
                //{
                //    Console.WriteLine($"value: {value}");
                //    _equipment.TryChange(equipItem, value);
                //}
                //else { Console.WriteLine($"slot is empty"); }

                if (_equipment.TryAdd(equipItem.Slot, equipItem))
                {
                    // Item was equipped
                    Console.WriteLine($"Item {equipItem.Name} was equipped"); //edit
                    return;
                }
                //Сделать возможность замены существующей экипировки
                else
                {                   
                    Console.WriteLine($"Slot for {equipItem.Slot} already filled. Do you want, change your equipment? 1 - Yes, 2 - now.");
                    while (true)
                    {
                        if (int.TryParse(Console.ReadLine(), out var choosen))
                        {
                            switch (choosen)
                            {
                                case 1:
                                    if (!Inventory.TryAdd(_equipment[equipItem.Slot])) //Добавляем одетый предмет в инвентярь (по хорошему, надо было его сначала снять)
                                    {
                                        Console.WriteLine($"Inventory of {Name} is full. Your {equipItem.Slot} was left");
                                    }
                                    else 
                                    {
                                        //Console.WriteLine($"Your {equipItem.Slot} moved to inventory");
                                    }
                                    LookInTheInventory();
                                    _equipment[equipItem.Slot] = equipItem;
                                    Console.WriteLine($"{_equipment[equipItem.Slot].Name} equipped");
                                    return;
                                case 2:

                                    if (!Inventory.TryAdd(equipItem))
                                    {
                                        Console.WriteLine($"Inventory of {Name} is full");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Item added to inventory");
                                        LookInTheInventory();
                                    }

                                    return;

                            }
                        }
                    }
                }
            }

            base.AddItemToInventory(item);
        }

        private void LookInTheInventory()  //add
        {
            Console.WriteLine("Inventoty:");
            for (int i = 0; i < Inventory.Items.Count; i++)
            {
                Console.WriteLine($"{i+1}. {Inventory.Items[i].Name}");
            }
        }

        private void UseEconomicItem(EconomicItem economicItem)
        {
            if (economicItem is HealthPotion healthPotion) 
            {
                Health += healthPotion.HealthRestore; //не учитывается максимальное здоровье и HP становится выше максимального
            } 
            else if (economicItem is Grindstone grindstone) //add
            {
                if (_equipment.TryGetValue(EquipSlot.Armour, out var armor)) //add
                {
                    armor.Repair(grindstone.DurabilityRestore); //add
                }
                if (_equipment.TryGetValue(EquipSlot.Helmet, out var helmet)) //add
                {
                    helmet.Repair(grindstone.DurabilityRestore); //add
                }
            }
        }

        protected override uint CalculateAppliedDamage(uint damage)
        {
            uint defence = 0;
            if (_equipment.TryGetValue(EquipSlot.Armour, out var item) && item is Armour armour) 
            {
                /*damage -= (uint)(damage * (armour.Defence / 100f));*/
                defence += armour.Defence;
                //Сделать так, чтобы броня после каждого получения урона теряла один пункт прочности.
                armour.ReduceDurability(1);                
            }
            if (_equipment.TryGetValue(EquipSlot.Helmet, out var item2) && item2 is Helmet helmet) //add
            {
                /*damage -= (uint)(damage * (helmet.Defence / 100f));*/
                defence += helmet.Defence;
                //Сделать так, чтобы броня после каждого получения урона теряла один пункт прочности.
                helmet.ReduceDurability(1);
            }
            damage -= (uint)(damage * (/*armour.Defence*/ defence / 100f));
            return damage;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendLine(Name);
            builder.AppendLine($"Health {Health}/{MaxHealth}");
            builder.AppendLine("Loot:");
            var items = Inventory.Items;
            for (int i = 0; i < items.Count; i++) 
            {
                builder.AppendLine($"[{items[i].Name}] : {items[i].Amount}");
            }
            return builder.ToString();
        }
    }
}
