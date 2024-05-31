using GamePrototype.Combat;
using GamePrototype.Dungeon;
using GamePrototype.Units;
using GamePrototype.Utils;
using System.Xml.Linq;

namespace GamePrototype.Game
{
    public sealed class GameLoop
    {
        private Unit _player;
        private DungeonRoom _dungeon;
        private readonly CombatManager _combatManager = new CombatManager();
        private DifficultyLevel _difficultyLevel;
        
        public void StartGame() 
        {
            Initialize();
            Console.WriteLine("Entering the dungeon");
            StartGameLoop();
        }

        #region Game Loop

        private void Initialize()
        {
            Console.WriteLine("Welcome, player!");
            //Реализация зависит от уровня сложности ((easy, hard)). Уровень сложности задаётся в начале (через enum) игроком.
            _difficultyLevel = ChooseDifficult();

            switch (_difficultyLevel)
            {
                case DifficultyLevel.Easy:
                    var easyDungeon = new EasyDungeon("Small dungeon");
                    _dungeon = easyDungeon.BuildDungeon();
                    break;
                case DifficultyLevel.Normal:
                    var normalDungeon = new NormalDungeon("Average dungeon");
                    _dungeon = normalDungeon.BuildDungeon();
                    break;
                case DifficultyLevel.Hard:
                    var hardDungeon = new HardDungeon("Large dungeon");
                    _dungeon = hardDungeon.BuildDungeon();
                    break;
            }

            Console.WriteLine("Enter your name");
            _player = UnitFactoryDemo.CreatePlayer(Console.ReadLine());
            Console.WriteLine($"Hello {_player.Name}");

        }

        private void StartGameLoop()
        {
            var currentRoom = _dungeon;
            
            while (currentRoom.IsFinal == false) 
            {
                StartRoomEncounter(currentRoom, out var success);
                if (!success) 
                {
                    Console.WriteLine("Game over!");
                    return;
                }
                DisplayRouteOptions(currentRoom);
                while (true)
                {
                    if (Enum.TryParse<Direction>(Console.ReadLine(), out var direction))
                    {
                        if (currentRoom.Rooms.ContainsKey(direction)) //add если было указано значение, которое есть в словаре, но для него отсутствовал переход в следующую комнату, была ошибка
                        {
                            currentRoom = currentRoom.Rooms[direction];
                            Console.WriteLine($"You are in the {currentRoom.Name} room");
                            break;
                        }
                        else
                        { 
                            Console.WriteLine("Wrong direction!");
                        }
                    }    
                    else 
                    {
                        Console.WriteLine("Wrong direction!");
                    }
                }
            }
            Console.WriteLine($"Congratulations, {_player.Name}");
            Console.WriteLine("Result: ");
            Console.WriteLine(_player.ToString());
        }

        private void StartRoomEncounter(DungeonRoom currentRoom, out bool success)
        {
            success = true;
            if (currentRoom.Loot != null) 
            {
                Console.WriteLine($"You are find {currentRoom.Loot.Name}");
                _player.AddItemToInventory(currentRoom.Loot);
            }
            if (currentRoom.Enemy != null) 
            {
                Console.WriteLine($"{currentRoom.Enemy.Name} is here");
                if (_combatManager.StartCombat(_player, currentRoom.Enemy) == _player)
                {
                    _player.HandleCombatComplete();
                    LootEnemy(currentRoom.Enemy);
                }
                else 
                {
                    success = false;
                }
            }

            void LootEnemy(Unit enemy)
            {
                _player.AddItemsFromUnitToInventory(enemy);
            }
        }

        private void DisplayRouteOptions(DungeonRoom currentRoom)
        {
            Console.WriteLine("Where to go?");
            foreach (var room in currentRoom.Rooms)
            {
                Console.Write($"{room.Key} - {(int) room.Key}\t");
            }
        }

        private DifficultyLevel ChooseDifficult()
        {
            
            while (true)
            {
                Console.WriteLine("Choose the difficult: 1 - Easy, 2 - Normal, 3 - Hard");
                if (Enum.TryParse(Console.ReadLine(), out DifficultyLevel value))
                {
                    //Почему-то попадает сюда при любом значении. По идее не должно. Не нашел альтернативы, как сделать проверку.
                    _difficultyLevel = value;
                    Console.WriteLine($"Difficulty level is {_difficultyLevel}");
                    return _difficultyLevel;
                }
                else
                {
                    Console.WriteLine("Wrong level!");
                }
            }        
        }
        #endregion
    }
}
