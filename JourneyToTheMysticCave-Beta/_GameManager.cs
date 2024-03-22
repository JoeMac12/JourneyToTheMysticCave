﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyToTheMysticCave_Beta
{
    internal class _GameManager
    {
        #region Declarations
        Map map = new Map();
        GameStats gameStats = new GameStats();
        Gamelog gamelog = new Gamelog();
        Player player = new Player();
        LevelManager levelManager = new LevelManager();
        LegendColors legendColors = new LegendColors();
        HUD hUD = new HUD();

        EnemyManager enemyManager = new EnemyManager();
        ItemManager itemManager = new ItemManager();
        #endregion

        public _GameManager()
        {
            Init();
        }

        public void Gameplay()
        {
            map.Update();
            hUD.Update();
            Draw();
            legendColors.Update();

            while (true)
            {
                Update();
                Draw();
            }
        }

        private void Init()
        {
            levelManager.Init(player);
            map.Init(levelManager, legendColors, player, enemyManager, itemManager);
            gameStats.Init(levelManager, map);
            player.Init(map, gameStats, legendColors, enemyManager, levelManager, itemManager);
            legendColors.Init(gameStats, map, levelManager);
            enemyManager.Init(gameStats, levelManager, legendColors, gamelog, player, map);
            itemManager.Init(gameStats, levelManager, legendColors, gamelog, player, map, enemyManager);
            gamelog.Init(player, enemyManager, itemManager, gameStats, map);
            hUD.Init(player, enemyManager, itemManager, map, legendColors);
        }

        private void Update()
        {
            player.Update();
            levelManager.Update();
            map.Update();
            legendColors.Update();
            enemyManager.Update();
            hUD.Update();
            gamelog.Update();
            itemManager.Update();
        }

        private void Draw()
        {
            map.Draw();
            player.Draw();
            legendColors.Draw();
            enemyManager.Draw();
            hUD.Draw();
            gamelog.Draw();
            itemManager.Draw();
        }
    }
}
