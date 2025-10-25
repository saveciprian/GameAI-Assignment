using System;
using GameFramework;
using System.Collections.Generic;

namespace AI_Strategy
{
    /*
     * very simple example strategy based on random placement of units.
     */
    public class LeTudorStrategy : AbstractStrategy
    {
        private static Random random = new Random();

        public LeTudorStrategy(Player player) : base(player)
        {
        }

        /*
         * example strategy for deploying Towers based on random placement and budget.
         * Your one should be better!
         */
        public override void DeployTowers()
        {
            if (player.Gold > 8)
            {
                //Boolean positioned = false;
                int count = 0;
                while (/*!positioned && */count < 20)
                {
                    count++;
                    int x = random.Next(2, PlayerLane.WIDTH - 2);
                    int y = random.Next(PlayerLane.HEIGHT - 1) + 1; // has to leave soldier deploy lane empty
                    if (player.HomeLane.GetCellAt(x, y).Unit == null)
                    {
                        //positioned = true;
                        player.TryBuyTower<Turn>(x, y);
                    }
                }
            }
        }

        /*
         * example strategy for deploying Soldiers based on random placement and budget.
         * Yours should be better!
         */
        public override void DeploySoldiers()
        {
            int round = 0;
            while (player.Gold > 5 && round < 5)
            {
                round++;
                bool positioned = false;
                int count = 0;
                while (!positioned && count < 10)
                {
                    count++;
                    int x = random.Next(PlayerLane.WIDTH);
                    int y = 0;
                    if (player.EnemyLane.GetCellAt(x, y).Unit == null)
                    {
                        positioned = true;
                        player.TryBuySoldier<Soldat>(x);
                    }
                }
            }
        }

        /*
         * called by the game play environment. The order in which the array is returned here is
         * the order in which soldiers will plan and perform their movement.
         *
         * The default implementation does not change the order. Do better!
         */
        public override List<Soldier> SortedSoldierArray(List<Soldier> unsortedList)
        {
            return unsortedList;
        }

        /*
         * called by the game play environment. The order in which the array is returned here is
         * the order in which towers will plan and perform their action.
         *
         * The default implementation does not change the order. Do better!
         */
        public override List<Tower> SortedTowerArray(List<Tower> unsortedList)

        {
            return unsortedList;
        }
    }
}
