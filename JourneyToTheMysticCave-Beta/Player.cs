using System;
using System.Windows.Input;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyToTheMysticCave_Beta
{
    internal class Player : GameEntity
    {
        int dirX;
        int dirY;
        bool inDeep = false;
        int moveCount;

        public bool attackedEnemy = false;
        public bool itemPickedUp = false;
        private Enemy lastEncountered;
        private ShopManager shopManager;
        public QuestManager QuestManager { get; private set; }
        public int SwordsCollected { get; private set; }

        public int Gold { get; private set; }

        public Enemy GetLastEnountered()
        {
            return lastEncountered;
        }

        Map map;
        GameStats gameStats;
        EnemyManager enemyManager;
        LegendColors legendColors;
        LevelManager levelManager;
        ItemManager itemManager;


        public Player()
        {
            healthSystem = new HealthSystem();
        }

        public void Init(Map map, GameStats gameStats, LegendColors legendColors, EnemyManager enemyManager, LevelManager levelManager, ItemManager itemManager, ShopManager shopManager, QuestManager questManager)
        {
            this.map = map;
            this.gameStats = gameStats;
            this.legendColors = legendColors;
            this.enemyManager = enemyManager;
            this.levelManager = levelManager;
            this.itemManager = itemManager;
            this.shopManager = shopManager;
            this.QuestManager = questManager;

            healthSystem.health = gameStats.PlayerHealth;
            character = gameStats.PlayerCharacter;
            pos = gameStats.PlayerPos;
            damage = gameStats.PlayerDamage;
            name = gameStats.PlayerName;
        }

        public void Update()
        {
            Movement();
        }

        public void Draw()
        {
            Console.SetCursorPosition(pos.x, pos.y);

            legendColors.MapColor(character);
            if (map.GetCurrentMapContent()[pos.y, pos.x] == 'P')
                Console.BackgroundColor = ConsoleColor.DarkGreen;
            else if (map.GetCurrentMapContent()[pos.y, pos.x] == '~')
                Console.BackgroundColor = ConsoleColor.Blue;

            Console.Write(character);
            Console.ResetColor();
            Console.CursorVisible = false;
        }

        private void Movement()
        {
            if (!healthSystem.mapDead)
            {
                PlayerInput();

                int newX = pos.x + dirX;
                int newY = pos.y + dirY;

                if (CheckBoundaries(newX, newY))
                {
                    lastEncountered = GetEnemyAtPosition(newX, newY);
                    if (lastEncountered != null)
                        AttackEnemy(lastEncountered);
                    if(!attackedEnemy)
                    {
                        CheckFloor(newX, newY);
                    }
                    attackedEnemy = false;
                }
            }
        }

        private void PlayerInput()
        {
            ConsoleKeyInfo input = Console.ReadKey(true); // Read key without displaying it

            dirX = 0;
            dirY = 0;

            switch (input.Key)
            {
                case ConsoleKey.W: dirY = -1; break;
                case ConsoleKey.S: dirY = 1; break;
                case ConsoleKey.A: dirX = -1; break;
                case ConsoleKey.D: dirX = 1; break;
                case ConsoleKey.Q: dirY = -1; dirX = -1; break;
                case ConsoleKey.E: dirY = -1; dirX = 1; break;
                case ConsoleKey.Z: dirY = 1; dirX = -1; break;
                case ConsoleKey.C: dirY = 1; dirX = 1; break;
                case ConsoleKey.Spacebar: return; // using for testing, player doesn't move
                case ConsoleKey.Escape: System.Environment.Exit(0); return;
                case ConsoleKey.R:TryEnterShop(); break;
            }
        }

        private Enemy GetEnemyAtPosition(int x, int y)
        {
            foreach (Enemy enemy in enemyManager.enemies)
            {
                if (enemy.pos.x == x && enemy.pos.y == y)
                {
                    if ((enemy is Ranger && levelManager.mapLevel == 0) ||
                        (enemy is Mage && levelManager.mapLevel == 1) ||
                        (enemy is Melee && levelManager.mapLevel == 2) ||
                        (enemy is Boss && enemyManager.AreAllMeleeDead()))
                    {
                        return enemy;
                    }
                }
            }
            return null;
        }

        private void AttackEnemy(Enemy enemy)
        {
            enemy.healthSystem.TakeDamage(damage, "Attacked");
            attackedEnemy = true;
        }

        private bool CheckBoundaries(int x, int y)
        {
            return map.GetCurrentMapContent()[y, x] != '#' && map.GetCurrentMapContent()[y, x] != '^';
        }

        private void CheckFloor(int x, int y)
        {
            if (map.GetCurrentMapContent()[y, x] == '~' && inDeep == false)
            {
                inDeep = true;
                pos = new Point2D { x = x, y = y };
                moveCount = 0;
            }
            else if (map.GetCurrentMapContent()[y, x] == 'P')
                healthSystem.TakeDamage(gameStats.PoisonDamage, "Floor");

            if(!inDeep)
                pos = new Point2D { x = x, y = y };

            if (moveCount == 1)
            {
                moveCount = 0;
                inDeep = false;
            }
            moveCount++;
        }

        // Collect gold
        public void AddGold(int amount)
        {
            Gold += amount;
            QuestManager.UpdateQuest(QuestType.CollectGold, amount);
        }

        // Use gold
        public bool SpendGold(int amount)
        {
            if (Gold >= amount)
            {
                Gold -= amount;
                return true;
            }
            return false;
        }

        // Only for the sword quest
        public void CollectSword()
        {
            SwordsCollected++;
            QuestManager.UpdateQuest(QuestType.CollectSwords);
        }

        // Enter the shop if near it
        private void TryEnterShop()
        {
            Shop nearbyShop = GetNearbyShop();
            if (nearbyShop != null)
            {
                OpenShopWindow(nearbyShop);
            }
        }

        // Check when the player is near a shop
        private Shop GetNearbyShop()
        {
            foreach (Shop shop in shopManager.Shops)
            {
                int distance = Math.Abs(pos.x - shop.pos.x) + Math.Abs(pos.y - shop.pos.y);
                if (distance <= 1)
                {
                    return shop;
                }
            }
            return null;
        }

        // Opens the shop in a different window
        private void OpenShopWindow(Shop shop)
        {
            bool shopping = true;
            while (shopping)
            {
                Console.Clear();
                Console.WriteLine($"Welcome to {shop.name}!");
                Console.WriteLine($"Your Gold: {Gold}");
                Console.WriteLine("\nAvailable Items:");
                for (int i = 0; i < shop.Items.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {shop.Items[i].Name} - {shop.Items[i].Price} gold");
                }
                Console.WriteLine("\nPress the number of the item to buy it. Press 'Q' to exit the shop.");

                ConsoleKeyInfo input = Console.ReadKey(true);
                if (input.Key == ConsoleKey.Q) // Exit shop
                {
                    shopping = false;
                }
                else if (char.IsDigit(input.KeyChar))
                {
                    int selection = int.Parse(input.KeyChar.ToString()) - 1;
                    if (selection >= 0 && selection < shop.Items.Count)
                    {
                        BuyItem(shop.Items[selection]);
                    }
                }
            }
            Console.Clear();
        }

        // Buy shop item
        private void BuyItem(ShopItem item)
        {
            if (SpendGold(item.Price))
            {
                Console.WriteLine($"\nYou bought {item.Name}!");
                if (item.Name == "Magic Scroll")
                {
                    QuestManager.UpdateQuest(QuestType.BuyMagicScroll);
                }
            }
            else
            {
                Console.WriteLine("\nNot enough gold!");
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
        }
    }
}