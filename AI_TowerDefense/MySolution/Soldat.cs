using GameFramework;
using System.Collections.Generic;
namespace AI_Strategy
{
    public class Soldat : Soldier
    {
        public override void Move()
        {
            if (speed > 0 && posY < PlayerLane.HEIGHT)
            {
                int x = posX;
                int y = posY;
                for (int i = speed; i > 0; i--)
                {
                    if (MoveTo(x, y)) return;
                    if (MoveTo(x - i, y)) return;
                    if (MoveTo(x + i, y)) return;
                    if (MoveTo(x, y + i)) return;
                    if (MoveTo(x - i, y + i)) return;
                    if (MoveTo(x + i, y + i)) return;
                    if (MoveTo(x - i, y - i)) return;
                    if (MoveTo(x + i, y - i)) return;
                    if (MoveTo(x, y - i)) return;
                }
            }
        }
        
        
    }
}