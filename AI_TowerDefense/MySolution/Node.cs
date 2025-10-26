namespace AI_Strategy
{
    public class Node
    {
        public int x, y;
        public int cost = 0;
        public Node(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            
            Node other = (Node)obj;
            return x == other.x && y == other.y;
        }
        
        public override int GetHashCode()
        {
            // Combine x and y into a single hash code
            return x * 31 + y;
        }
    }
}