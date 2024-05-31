using GamePrototype.Items.EconomicItems;
using GamePrototype.Items.EquipItems;
using GamePrototype.Units;

namespace GamePrototype.Utils
{
    public class UnitFactoryDemo
    {
        public static Unit CreatePlayer(string name)
        {
            var player = new Player(name, 30, 30, 6);
            player.AddItemToInventory(new Weapon(10, 15, 15,/*add*/ "Sword"));
            player.AddItemToInventory(new Helmet(5, 10, 7,/*add*/ "Helmet"));
            player.AddItemToInventory(new Armour(10, 15, 15,/*add*/ "Armour"));
            player.AddItemToInventory(new HealthPotion("Potion"));
            return player;
        }

        public static Unit CreateGoblinEnemy() => new Goblin(GameConstants.Goblin, 18, 18, 2);
        public static Unit CreateTrollEnemy() => new Troll(GameConstants.Troll, 40, 40, 5);
        public static Unit CreateDragonEnemy() => new Dragon(GameConstants.Dragon, 80, 80, 10);
    }
}
