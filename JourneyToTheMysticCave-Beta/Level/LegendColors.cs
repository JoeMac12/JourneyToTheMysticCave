using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace JourneyToTheMysticCave_Beta
{
    internal class LegendColors
    {
        public GameStats gameStats;
        public Map map;
        public LevelManager levelManager;

        int columnCount;
        int rowCount = 0;
        int level;

        public void Init(GameStats gamestats, Map map, LevelManager levelManager)
        {
            this.gameStats = gamestats;
            this.map = map;
            this.levelManager = levelManager;
        }

        public void Update()
        {
            columnCount = map.GetMapColumnCount() + 2;
            rowCount = 0;
            level = levelManager.mapLevel;
        }

        public void Draw()
        {
            columnCount = map.GetMapColumnCount() + 2;
            rowCount = 0;
            Legend();
        }

        private void Legend() // displays legend on the bottom of the map.
        {
            Console.SetCursorPosition(columnCount, rowCount++);
            Console.WriteLine("+------------------------+");
            Console.SetCursorPosition(columnCount, rowCount++);
            Console.WriteLine("Map Legend:");
            Console.SetCursorPosition(columnCount, rowCount++);
            DisplayChar(gameStats.PlayerCharacter, gameStats.PlayerName);
            Console.SetCursorPosition(columnCount, rowCount++);
            if (level == 0)
                DisplayChar(gameStats.RangedCharacter, gameStats.RangerName);
            if (level == 1)
                DisplayChar(gameStats.MageCharacter, gameStats.MageName);
            if (level == 2)
            {
                DisplayChar(gameStats.MeleeCharacter, gameStats.MeleeName);
                Console.SetCursorPosition(columnCount, rowCount++);
                DisplayChar(gameStats.BossCharacter, gameStats.BossName);
            }
            Console.SetCursorPosition(columnCount, rowCount++);
            DisplayChar(gameStats.MoneyCharacter, gameStats.MoneyName);
            Console.SetCursorPosition(columnCount, rowCount++);
            DisplayChar(gameStats.GoldCharacter, gameStats.GoldName);
            Console.SetCursorPosition(columnCount, rowCount++);
            DisplayChar(gameStats.ShopCharacter, gameStats.ShopName);
            Console.SetCursorPosition(columnCount, rowCount++);
            DisplayChar(gameStats.PotionCharacter, gameStats.PotionName);
            Console.SetCursorPosition(columnCount, rowCount++);
            DisplayChar(gameStats.TrapCharacter, gameStats.TrapName);
            Console.SetCursorPosition(columnCount, rowCount++);
            DisplayChar(gameStats.SwordCharacter, gameStats.SwordName);
            Console.SetCursorPosition(columnCount, rowCount++);
            DisplayChar('*', "Next Area");
            Console.SetCursorPosition(columnCount, rowCount++);
            DisplayChar('~', "Deep Water");
            Console.SetCursorPosition(columnCount, rowCount++);
            DisplayChar('P', "Poison Spill");
            Console.SetCursorPosition(columnCount, rowCount++);
            DisplayChar('^', "Mountains");
            Console.SetCursorPosition(columnCount, rowCount++);
            DisplayChar('#', "Walls");
            Console.SetCursorPosition(columnCount, rowCount++);
            Console.WriteLine("+------------------------+");
        }

        private void DisplayChar(char symbol, string description)
        {
            MapColor(symbol);
            Console.Write(symbol);
            Console.ResetColor();
            Console.Write($" = {description}\n");
            Console.WriteLine();
        }

        public void MapColor(char c)    // handles map color
        {
            switch (c)
            {
                case '#': // Boundaries
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    break;
                case '^': // Mountain
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.BackgroundColor = ConsoleColor.Gray;
                    break;
                case '~': // Water
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.BackgroundColor = ConsoleColor.Blue;
                    break;
                case var _ when c == gameStats.SwordCharacter: // Sword (item)
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case var _ when c == gameStats.MoneyCharacter: // money (item)
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Green;
                    break;
                case var _ when c == gameStats.TrapCharacter: // trap (item)
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;
                case var _ when c == gameStats.MeleeCharacter: // Slime
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case var _ when c == gameStats.RangedCharacter: // Ranged enemy
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                case var _ when c == gameStats.MageCharacter: // Mage
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                case var _ when c == gameStats.BossCharacter: // boss - enemy
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                case var _ when c == gameStats.PlayerCharacter: // (Player)
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case var _ when c == gameStats.PotionCharacter: // Potion (item)
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case '*': // next area
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case 'P': // poison floor
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    break;
                case var _ when c == gameStats.GoldCharacter: // Gold
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case var _ when c == gameStats.ShopCharacter: // Shop
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
            }
        }
    }
}
