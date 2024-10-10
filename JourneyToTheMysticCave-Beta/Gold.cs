using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyToTheMysticCave_Beta
{
    internal class Gold : Item
    {
        public int Value { get; private set; }

        public Gold(int count, char character, string name, LegendColors legendColors, Player player, int value) :
            base(count, character, name, legendColors, player)
        {
            Value = value;
        }

        public override void Update()
        {
            if (player.pos.x == pos.x && player.pos.y == pos.y)
            {
                TryCollect();
                player.AddGold(Value);
            }
        }
    }
}
