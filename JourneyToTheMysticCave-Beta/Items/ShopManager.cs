using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyToTheMysticCave_Beta
{
    internal class ShopManager
    {
        public List<Shop> Shops { get; private set; }

        public ShopManager()
        {
            Shops = new List<Shop>();
        }

        public void InitializeShops()
        {
            for (int i = 0; i < 3; i++)
            {
                Shop shop = new Shop('S', $"Shop {i + 1}", new Point2D { x = 0, y = 0 });

                shop.AddItem(new ShopItem("Item 1", 10));
                shop.AddItem(new ShopItem("Item 2", 20));
                shop.AddItem(new ShopItem("Item 3", 30));

                Shops.Add(shop);
            }
        }

        public void Update()
        {
        }

        public void Draw()
        {
            foreach (var shop in Shops)
            {
                Console.SetCursorPosition(shop.pos.x, shop.pos.y);
                Console.Write(shop.character);
            }
        }
    }
}
