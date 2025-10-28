

namespace AI_Strategy
{
    public class SoldierManager
    {
        private Soldat soldier;
        private Graph graph;
        
        
        public SoldierManager(Soldat unit, Graph graph)
        {
            this.soldier = unit;
            this.graph = graph;
        }

        public void CalculatePath()
        {
            
        }
        
        public void Move()
        {
            this.soldier.MoveToPosition(0, 0);
        }
    }
}