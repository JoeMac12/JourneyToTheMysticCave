using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyToTheMysticCave_Beta
{
    // Quests
    public enum QuestType
    {
        CollectGold,
        CollectSwords,
        BuyMagicScroll,
        KillBoss
    }

    // Type, details and requirements of quests
    public class Quest
    {
        public string Description { get; private set; }
        public bool IsCompleted { get; private set; }
        public QuestType Type { get; private set; }
        public int RequiredAmount { get; private set; }
        public int CurrentAmount { get; private set; }

        public Quest(string description, QuestType type, int requiredAmount = 1)
        {
            Description = description;
            Type = type;
            RequiredAmount = requiredAmount;
            CurrentAmount = 0;
            IsCompleted = false;
        }

        // Update quest progress when called
        public void UpdateProgress(int amount)
        {
            CurrentAmount += amount;
            if (CurrentAmount >= RequiredAmount && !IsCompleted)
            {
                IsCompleted = true;
            }
        }
    }
}
