using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyToTheMysticCave_Beta
{
    internal class HUD
    {
        Player player;
        EnemyManager enemyManager;
        ItemManager itemManager;
        LegendColors legendColors;
        Map map;
        QuestManager questManager;
        int columnCount;
        int rowCount;
        string health;


        public void Init(Player player, EnemyManager enemyManager, ItemManager itemManager, Map map, LegendColors legendColors, QuestManager questManager)
        {
            this.player = player;
            this.enemyManager = enemyManager;
            this.itemManager = itemManager;
            this.map = map;
            this.legendColors = legendColors;
            this.questManager = questManager;
        }

        public void Update()
        {
            rowCount = map.GetMapRowCount() + 1;
            columnCount = 0;

            // Clear only the area where the HUD is displayed
            for (int i = 0; i < 9; i++) // Assuming 9 lines for HUD display
            {
                Console.SetCursorPosition(columnCount, rowCount + i);
                Console.Write(new string(' ', Console.WindowWidth));
            }
        }

        public void Draw()
        {
            Console.SetCursorPosition(columnCount, rowCount);
            HUDInfo();
        }

        private void HUDInfo()
        {
            Console.WriteLine("+-------------------------------+");
            Console.WriteLine($"{player.name} Health: {player.healthSystem.health}");
            Console.WriteLine($"{player.name} Damage: {player.damage}");
            Console.WriteLine($"{player.name} Gold: {player.Gold}");
            Console.WriteLine($"Last Enemy Attacked - {EnemyName()}");
            if (EnemyHealth() == 0)
                health = $"{EnemyName()} is dead";
            else
                health = $"{EnemyHealth()}";
            Console.WriteLine($"Enemy Health - {health}");
            Console.Write("Money Picked Up: ");
            foreach(Item item in itemManager.items)
            {
                if (item.GetType().Name == "Money" && item.collected)
                {
                    legendColors.MapColor(item.character);
                    Console.Write(item.character);
                    Console.ResetColor();
                    Console.Write(' ');
                }
            }
            Console.WriteLine();
            Console.WriteLine("+-------------------------------+");

            Console.WriteLine();
            Console.WriteLine();

            DisplayQuestStatus();
        }

        // Current status of quests
        private void DisplayQuestStatus()
        {
            Console.WriteLine("Quests:");
            foreach (var quest in questManager.Quests)
            {
                Console.ForegroundColor = quest.IsCompleted ? ConsoleColor.Green : ConsoleColor.Red;
                Console.WriteLine($"- {quest.Description} ({quest.CurrentAmount}/{quest.RequiredAmount})");
                Console.ResetColor();
            }
        }

        private string EnemyName()
        {
            Enemy enemy = player.GetLastEnountered();
            if( enemy != null )
            {
                string fullTypeName = enemy.GetType().ToString();
                string[] parts = fullTypeName.Split('.');
                string enemyName = parts[parts.Length - 1];
                return enemyName;
            }
            else
                return string.Empty;
        }

        private int? EnemyHealth()
        {
            Enemy enemy = player.GetLastEnountered();
            if (enemy != null)
            {
                int enemyHealth = enemy.healthSystem.health;
                return enemyHealth;
            }
            else
                return null;
        }
    }
}
