using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyToTheMysticCave_Beta
{
    internal class EnemyManager
    {
        public GameStats stats;
        public LevelManager levelManager;
        LegendColors legendColors;
        Gamelog log;
        Player player;
        Random random = new Random();
        Map map;
        public List<Enemy> enemies;
        public int meleeCount;
        bool firstDead = false;

        public EnemyManager()
        {
            enemies = new List<Enemy>();
        }

        public void Init(GameStats stats, LevelManager levelManager, LegendColors legendColors, Gamelog log, Player player, Map map)
        {
            this.stats = stats;
            this.levelManager = levelManager;
            this.legendColors = legendColors;
            this.log = log;
            this.player = player;
            this.map = map;

            for (int i = 0; i < stats.RangerCount; i++)
                enemies.Add(new Ranger(stats.RangerCount, stats.RangedCharacter, stats.RangerName, stats.RangerDamage, stats.RangerAttack, legendColors, player, log, this, map, stats));
            for (int i = 0; i < stats.MageCount; i++)
                enemies.Add(new Mage(stats.MageCount, stats.MageCharacter, stats.MageName, stats.MageDamage, stats.MageAttack, legendColors, player, log, map, this, stats));
            for (int i = 0; i < stats.MeleeCount; i++)
                enemies.Add(new Melee(stats.MeleeCount, stats.MeleeCharacter, stats.MeleeName, stats.MeleeDamage, stats.MeleeAttack, legendColors, player, log, this, map, stats));
            for (int i = 0; i < stats.BossCount; i++)
                enemies.Add(new Boss(stats.BossCount, stats.BossCharacter, stats.BossName, stats.BossDamage, stats.BossAttack, legendColors, player, log, this, map, stats));

            foreach (Enemy enemy in enemies)
            {
                switch (enemy.GetType().Name)
                {
                    case nameof(Ranger):
                        enemy.healthSystem = new HealthSystem();
                        enemy.healthSystem.health = stats.GiveHealth(random, "Ranger");
                        enemy.pos = stats.PlaceCharacters(0, random);
                        break;
                    case nameof(Mage):
                        enemy.healthSystem = new HealthSystem();
                        enemy.healthSystem.health = stats.GiveHealth(random, "Mage");
                        enemy.pos = stats.PlaceCharacters(1, random);
                        break;
                    case nameof(Melee):
                        enemy.healthSystem = new HealthSystem();
                        enemy.healthSystem.health = stats.GiveHealth(random, "Melee");
                        enemy.pos = stats.PlaceCharacters(2, random);
                        meleeCount++;
                        break;
                    case nameof(Boss):
                        enemy.healthSystem = new HealthSystem();
                        enemy.healthSystem.health = stats.BossHealth;
                        break;
                }
            }
        }

        public void Update()
        {
            foreach (Enemy enemy in enemies)
            {
                if (levelManager.mapLevel == 0 && enemy.GetType().Name == nameof(Ranger))
                {
                    enemy.Update(random);
                }
                else if (levelManager.mapLevel == 1 && enemy.GetType().Name == nameof(Mage))
                {
                    enemy.Update(random);
                }
                else if (levelManager.mapLevel == 2)
                {
                    if (enemy.GetType().Name == nameof(Melee))
                        enemy.Update(random);
                    else if (enemy.GetType().Name == nameof(Boss) && AreAllMeleeDead())
                    {
                        if (AreAllMeleeDead() && firstDead == false)
                        {
                            enemy.pos = stats.PlaceCharacters(2, random);
                            firstDead = true;
                        }
                        enemy.Update(random);
                        if (enemy.healthSystem.dead)
                        {
                            player.QuestManager.UpdateQuest(QuestType.KillBoss);
                        }
                    }
                }
            }
        }

        public void Draw()
        {
            foreach (Enemy enemy in enemies)
            {
                if (levelManager.mapLevel == 0 && enemy.GetType().Name == nameof(Ranger))
                {
                    enemy.Draw();
                }
                else if (levelManager.mapLevel == 1 && enemy.GetType().Name == nameof(Mage))
                {
                    enemy.Draw();
                }
                else if (levelManager.mapLevel == 2)
                {
                    if (enemy.GetType().Name == nameof(Melee))
                        enemy.Draw();
                    else if (enemy.GetType().Name == nameof(Boss) && AreAllMeleeDead())
                    {
                        if (AreAllMeleeDead() && firstDead == false)
                        {
                            enemy.pos = stats.PlaceCharacters(2, random);
                            firstDead = true;
                        }
                        enemy.Draw();
                    }
                }
            }
        }

        public bool AreAllEnemiesDead()
        {
            return enemies.All(enemy => !enemy.IsAlive);
        }

        public bool AreAllMeleeDead()
        {
            return !enemies.Any(enemy => enemy is Melee && enemy.IsAlive);
        }
    }
}
