using GamePrototype.Dungeon;
using GamePrototype.Items.EconomicItems;
using GamePrototype.Items.EquipItems;

namespace GamePrototype.Utils
{
    internal class NormalDungeon : DungeonBuilder
    {
        public NormalDungeon(string name) : base(name)
        {
        }

        public override DungeonRoom BuildDungeon()
        {
            var enter = new DungeonRoom("Enter");
            var monsterRoom = new DungeonRoom("Monster", UnitFactoryDemo.CreateGoblinEnemy());
            var emptyRoom = new DungeonRoom("Empty");
            var lootRoom = new DungeonRoom("Loot1", new Gold());
            var lootStoneRoom = new DungeonRoom("Loot1", new Grindstone("Stone"));
            var finalRoom = new DungeonRoom("Final", new Grindstone("Stone1"));
            var forge = new DungeonRoom("Forge", new Grindstone("StoneForTest")); //add
            //Реализовать новые виды брони 
            var armore = new DungeonRoom("Armore", new Armour(12, 20, 20, "Full Plate Armour")); //add
            //Реализовать новые виды оружия
            var library = new DungeonRoom("Library", new Weapon(12, 20, 20, "Magic book")); //add
            var bridge = new DungeonRoom("Bridge", UnitFactoryDemo.CreateTrollEnemy());

            enter.TrySetDirection(Direction.Forward, forge); //add
            enter.TrySetDirection(Direction.Right, library); //add
            enter.TrySetDirection(Direction.Left, armore); //add

            forge.TrySetDirection(Direction.Right, monsterRoom); //change
            forge.TrySetDirection(Direction.Left, emptyRoom); //change

            armore.TrySetDirection(Direction.Forward, bridge); //add
            bridge.TrySetDirection(Direction.Forward, emptyRoom); //add

            monsterRoom.TrySetDirection(Direction.Forward, lootRoom);
            monsterRoom.TrySetDirection(Direction.Left, emptyRoom);

            emptyRoom.TrySetDirection(Direction.Forward, lootStoneRoom);

            lootRoom.TrySetDirection(Direction.Forward, finalRoom);
            lootStoneRoom.TrySetDirection(Direction.Forward, finalRoom);
            library.TrySetDirection(Direction.Forward, finalRoom); //add
            

            return enter;
        }
    }
}
