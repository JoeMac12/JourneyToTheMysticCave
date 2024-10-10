using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace JourneyToTheMysticCave_Beta
{
    internal class GameStats
    {
        LevelManager levelManager;
        Random random = new Random();
        Map map;

        #region PlayerStat Declarations
        public string PlayerName { get; set; }
        public char PlayerCharacter { get; set; }
        public int PlayerDamage { get; set; }
        public int PlayerHealth { get; set; }
        public Point2D PlayerPos { get; set; }

        #endregion

        #region RangerStat Declarations
        public int RangerCount { get; set; }
        public char RangedCharacter { get; set; }
        public string RangerName { get; set; }
        public int RangerDamage { get; set; }
        private int rangerMaxHp;
        private int rangerMinHp;
        public string RangerAttack { get; set; }
        #endregion

        #region MageStat Declarations
        public int MageCount { get; set; }
        public char MageCharacter { get; set; }
        public string MageName { get; set; }
        public int MageDamage { get; set; }
        private int mageMaxHp;
        private int mageMinHp;
        public string MageAttack { get; set; }
        #endregion

        #region MeleeStat Declarations
        public int MeleeCount { get; set; }
        public char MeleeCharacter { get; set; }
        public string MeleeName { get; set; }
        public int MeleeDamage { get; set; }
        private int meleeMaxHp;
        private int meleeMinHp;
        public string MeleeAttack { get; set; }
        #endregion

        #region BossStat Declarations
        public int BossCount { get; set; }
        public char BossCharacter { get; set; }
        public string BossName { get; set; }
        public int BossDamage { get; set; }
        public int BossHealth { get; set; }
        public string BossAttack { get; set; }
        #endregion

        #region MoneyStat Declarations
        public int MoneyCount { get; set; }
        public char MoneyCharacter { get; set; }
        public string MoneyName { get; set; }
        #endregion

        #region PotionStat Declarations
        public int PotionCount { get; set; }
        public char PotionCharacter { get; set; }
        public string PotionName { get; set; }
        public int PotionHeal { get; set; }
        #endregion

        #region TrapStat Declarations
        public int TrapCount { get; set; }
        public char TrapCharacter { get; set; }
        public string TrapName { get; set; }
        public int TrapDamage { get; set; }
        #endregion

        #region SwordStat Declarations
        public int SwordCount { get; set; }
        public char SwordCharacter { get; set; }
        public string SwordName { get; set; }
        public int SwordMultiplier { get; set; }
        #endregion

        #region GoldStat Declarations
        public int GoldCount { get; set; }
        public char GoldCharacter { get; set; }
        public string GoldName { get; set; }
        public int MinGoldValue { get; set; }
        public int MaxGoldValue { get; set; }
        #endregion

        public int PoisonDamage;

        public void Init(LevelManager levelManager, Map map)
        {
            this.levelManager = levelManager;
            this.map = map;

            GameConfig();
        }

        public void GameConfig()
        {
            //Player Configs/Stats
            PlayerCharacter = 'H';
            PlayerName = "Harold";
            //PlayerHealth = 300; //for testing only
            PlayerHealth = 100; 
            PlayerDamage = 10;
            PlayerPos = new Point2D { x = 2, y = 5 };

            // Ranger Configs/Stats
            RangerCount = 3;
            RangedCharacter = 'R';
            RangerName = "Ranger";
            RangerDamage = 1;
            rangerMinHp = 35;
            rangerMaxHp = 60;
            RangerAttack = $"by Ranger arrow - {RangerDamage} damage";

            // Mage Configs/Stats
            MageCount = 3;
            MageCharacter = 'M';
            MageName = "Mage";
            MageDamage = 3;
            mageMinHp = 40;
            mageMaxHp = 65;
            MageAttack = $"by mage magic - {MageDamage} damage";

            // Melee Configs/Stats
            MeleeCount = 30;
            MeleeCharacter = 'S';
            MeleeName = "Slime";
            MeleeDamage = 1;
            meleeMinHp = 2;
            meleeMaxHp = 10;
            MeleeAttack = $"by Slime sludge - {MeleeDamage} damage";

            // Boss Configs/Stats
            BossCount = 1;
            BossCharacter = 'B';
            BossName = "Boss";
            BossDamage = 6;
            BossHealth = 200;
            BossAttack = $"by a giant fist - {BossDamage} damage";

            // Money Configs
            MoneyCount = 6;
            MoneyCharacter = '$';
            MoneyName = "Money";

            // Potion Configs
            PotionCount = 6;
            PotionName = "Potion";
            PotionCharacter = '6';
            PotionHeal = 10;

            // Trap Configs
            TrapCount = 30;
            TrapCharacter = 'T';
            TrapName = "Trap";
            TrapDamage = 4;

            // Sword Configs
            SwordCount = 3;
            SwordCharacter = 't';
            SwordName = "Sword";
            SwordMultiplier = 10;

            // Floor Damage
            PoisonDamage = 5;

            GoldCount = 10;
            GoldCharacter = 'G';
            GoldName = "Gold";
            MinGoldValue = 1;
            MaxGoldValue = 10;
        }

        public int GiveHealth(Random random, string type)
        {
            int health;
            switch (type)
            {
                case "Ranger":
                    health = random.Next(rangerMinHp, rangerMaxHp);
                    return (health);
                case "Mage":
                    health = random.Next(mageMinHp, mageMaxHp);
                    return (health);
                case "Melee":
                    health = (random.Next(meleeMinHp, meleeMaxHp));
                    return (health);
                default: return 0;
            }
        }

        public Point2D PlaceCharacters(int levelNumber, Random random)
        {
            int x, y;

            do
            {
                x = random.Next(0, levelManager.AllMapContents[levelNumber].GetLength(1));
                y = random.Next(0, levelManager.AllMapContents[levelNumber].GetLength(0));
            } while (!CheckInitialPlacement(x, y, levelNumber));

            return new Point2D { x = x, y = y };
        }

        private bool CheckInitialPlacement(int x, int y, int levelNumber)
        {
            return levelManager.InitialBoundaries(x, y, levelNumber) && map.EmptySpace(x,y, levelNumber);
        }
    }
}