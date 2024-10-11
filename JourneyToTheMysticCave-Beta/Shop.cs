using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyToTheMysticCave_Beta
{
    internal class Shop : GameObject
    {
        public List<ShopItem> Items { get; private set; }
        public int ShopId { get; private set; }

        public Shop(char character, string name, Point2D position, int shopId)
        {
            this.character = character;
            this.name = name;
            this.pos = position;
            this.ShopId = shopId;
            Items = new List<ShopItem>();
        }

        public void AddItem(ShopItem item)
        {
            Items.Add(item);
        }
    }

    internal class ShopItem
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public int ShopId { get; set; }

        public ShopItem(string name, int price, int shopId)
        {
            Name = name;
            Price = price;
            ShopId = shopId;
        }
    }
}

