using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyToTheMysticCave_Beta
{
    public class QuestManager
    {
        public List<Quest> Quests { get; private set; }

        // List of quests the player can do
        public QuestManager()
        {
            Quests = new List<Quest>
            {
                new Quest("Collect 50 Gold", QuestType.CollectGold, 50),
                new Quest("Collect 3 Swords", QuestType.CollectSwords, 3),
                new Quest("Buy A Magic Scroll", QuestType.BuyMagicScroll),
                new Quest("Kill The Boss", QuestType.KillBoss)
            };
        }

        // Update quest status
        public void UpdateQuest(QuestType type, int amount = 1)
        {
            Quest quest = Quests.Find(q => q.Type == type);
            if (quest != null)
            {
                quest.UpdateProgress(amount);
            }
        }

        // Check if player did a quest
        public bool IsQuestCompleted(QuestType type)
        {
            Quest quest = Quests.Find(q => q.Type == type);
            return quest != null && quest.IsCompleted;
        }
    }
}
