﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyToTheMysticCave_Beta
{
    internal class ShopManager
    {
        private Map map;
        private Random random;
        private GameStats gameStats;
        private LegendColors legendColors;
        public List<Shop> Shops { get; private set; }

        public ShopManager(Map map, GameStats gameStats, LegendColors legendColors)
        {
            this.map = map;
            this.gameStats = gameStats;
            this.legendColors = legendColors;
            this.random = new Random();
            Shops = new List<Shop>();
        }

        // Create 3 shops on each level
        public void InitializeShops()
        {
            PlaceShop(0);
            PlaceShop(1);
            PlaceShop(2);
        }

        // Place shops randomly on the levels and add buyable items
        private void PlaceShop(int level)
        {
            char[,] currentMap = map.GetCurrentMapContent();
            int maxAttempts = 100;

            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                int x = random.Next(1, currentMap.GetLength(1) - 1);
                int y = random.Next(1, currentMap.GetLength(0) - 1);

                if (IsValidShopPosition(x, y, currentMap))
                {
                    Shop shop = new Shop(gameStats.ShopCharacter, $"Shop {Shops.Count + 1}", new Point2D { x = x, y = y }, Shops.Count);

                    // Items for each shop. They don't do anything but they are cool I guess
                    switch (Shops.Count)
                    {
                        case 0:
                            shop.AddItem(new ShopItem("Magic Scroll", 30, 0));
                            shop.AddItem(new ShopItem("Elixir", 25, 0));
                            shop.AddItem(new ShopItem("Mana Crystal", 10, 0));
                            break;
                        case 1:
                            shop.AddItem(new ShopItem("Dagger", 15, 1));
                            shop.AddItem(new ShopItem("Staff", 20, 1));
                            shop.AddItem(new ShopItem("Bow", 25, 1));
                            break;
                        case 2:
                            shop.AddItem(new ShopItem("Helmet", 15, 2));
                            shop.AddItem(new ShopItem("Chestplate", 45, 2));
                            shop.AddItem(new ShopItem("Boots", 30, 2));
                            break;
                    }

                    Shops.Add(shop);
                    break;
                }
            }
        }

        // Make sure it's in a valid spot to spawn and use
        private bool IsValidShopPosition(int x, int y, char[,] map)
        {
            return map[y, x] == ' ' && !IsAdjacentToWall(x, y, map);
        }

        // Check nearby walls for shop spawns
        private bool IsAdjacentToWall(int x, int y, char[,] map)
        {
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    int nx = x + dx;
                    int ny = y + dy;
                    if (nx >= 0 && nx < map.GetLength(1) && ny >= 0 && ny < map.GetLength(0))
                    {
                        if (map[ny, nx] == '#')
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public void Update()
        {
        }

        public void Draw()
        {
            foreach (var shop in Shops)
            {
                Console.SetCursorPosition(shop.pos.x, shop.pos.y);
                legendColors.MapColor(shop.character);
                Console.Write(shop.character);
                Console.ResetColor();
            }
        }
    }
}
