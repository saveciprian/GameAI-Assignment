using System;
using System.Collections.Generic;
using GameFramework;

namespace AI_Strategy
{
    public class Graph
    {
        public List<Node> nodes;
        protected Player player;

        public enum Mode
        {
            Home,
            Enemy
        }
        
        public Graph(Player player)
        {
            this.player = player;
            nodes = new List<Node>();
            for (int y = 0; y < PlayerLane.HEIGHT; y++)
            {
                for (int x = 0; x < PlayerLane.WIDTH; x++)
                {
                    nodes.Add(new Node(x, y));
                }
            }
        }

        public void CheckColumns()
        {
            for (int x = 0; x < PlayerLane.WIDTH; x++)
            {
                Node top = nodes[x];
                Node bottom = nodes[(PlayerLane.HEIGHT - 1) * PlayerLane.WIDTH + x];
                this.GetPath(top, bottom, Mode.Enemy);
            }
            DebugLogger.Log("-----------------------------------");
        }

        public void GetPath(Node start, Node end, Mode mode)
        {
            PriorityQ<Node> frontier = new PriorityQ<Node>();
            frontier.Enqueue(start, 0);
            Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
            Dictionary<Node, int> costSoFar = new Dictionary<Node, int>();
            cameFrom.Add(start, null);
            costSoFar.Add(start, 0);
            
            while (frontier.Count > 0)
            {
                Node current = frontier.Dequeue();
                if (current == end)
                {
                    break;
                }

                foreach (Node next in this.GetNeighbors(current, mode))
                {
                    int newCost = costSoFar[current] + next.cost;
                    if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                    {
                        costSoFar[next] = newCost;
                        int priority = newCost;
                        frontier.Enqueue(next, priority);
                        cameFrom[next] = current;
                    }
                }
            }

            if (costSoFar.ContainsKey(end))
            {
                DebugLogger.Log("Cost to reach end: " + costSoFar[end] + "");
            }
            else
            {
                DebugLogger.Log("No path found!");
            }
        }
        
        public Node GetNode(int x, int y)
        {
            return nodes.Find(n => n.x == x && n.y == y);
        }

        public void DebugPrint(int x, int y)
        {
            Node displayNode = GetNode(x, y);
            List<Node> neighborsTest = this.GetNeighbors(displayNode, Mode.Enemy);
            DebugLogger.Log("Node: " + displayNode.x + ", " + displayNode.y + "");
            foreach (Node node in neighborsTest)
            {
                DebugLogger.Log("Neighbors: " + node.x + ", " + node.y + " | Cost: " + node.cost);
            }
        }
        
        
        public List<Node> GetNeighbors(Node node, Mode mode)
        {
            int[][] directions = new int[][]
            {
                new int[] { -1,  1 }, //top left
                new int[] {  0,  1 }, //top
                new int[] {  1,  1 }, //top right
                new int[] { -1,  0 }, //left
                new int[] {  1,  0 }, //right
                new int[] { -1, -1 }, //bottom left
                new int[] {  0, -1 }, //bottom
                new int[] {  1, -1 }  //bottom right
            };
            
            List<Node> result = new List<Node>();
            
            foreach(int[] direction in directions)
            {
                int nx = node.x + direction[0];
                int ny = node.y + direction[1];

                if (nx < 0 || ny < 0 || nx >= PlayerLane.WIDTH || ny >= PlayerLane.HEIGHT) continue;
                
                Node neighbor = GetNode(nx, ny);
                if(neighbor == null) continue;

                if (mode == Mode.Enemy)
                {
                    if (player.EnemyLane.GetCellAt(neighbor.x, neighbor.y).Unit == null)
                    {
                        neighbor.cost = GetNodeCost(neighbor, Mode.Enemy);
                        result.Add(neighbor);
                    } 
                }
                else
                {
                    neighbor.cost = GetNodeCost(neighbor, Mode.Home);
                    if(player.HomeLane.GetCellAt(neighbor.x, neighbor.y).Unit == null) result.Add(neighbor);
                }
            }
            
            return result;
        }

        public int GetNodeCost(Node node, Mode mode)
        {
            int range = 2;
            PlayerLane lane = (mode == Mode.Enemy) ? player.EnemyLane : player.HomeLane;
            Unit unit = null;
            int cost = 0;
            
            for (int x = node.x - range; x <= node.x + range; x++)
            {
                for (int y = node.y - range; y <= node.y + range; y++)
                {
                    Cell cell = lane.GetCellAt(x, y);
                    if (cell == null)
                    {
                        continue;
                    }

                    unit = cell.Unit;
                    if (mode == Mode.Enemy)
                    {
                        if (unit != null && unit.Type == "T" && unit.Health > 0)
                        {
                            if (Math.Abs(node.x - x) == 2 || Math.Abs(node.y - y) == 2) cost += 2 * unit.Health;
                            else cost += 1 * unit.Health;
                        }
                        
                    } 
                    
                    else if (mode == Mode.Home)
                    {
                        if (unit != null && unit.Type == "S" && unit.Health > 0)
                        {
                            // unitsInRange.Add(unit);
                        }
                    }
                }
            }

            // foreach (var entity in unitsInRange)
            // {
            //     if (Math.Abs(node.x - entity.PosX) == 2 || Math.Abs(node.y - entity.PosY) == 2) cost += 2 * entity.Health;
            //     else cost += 1 * entity.Health;
            // }
            
            
            return cost;
        }
        
        // protected bool TryGetTarget(out Unit unit)
        // {
        //     List<Unit> targetsInRange = new List<Unit>();
        //     for (int x = posX - range; x <= posX + range; x++)
        //     {
        //         for (int y = posY - range; y <= posY + range; y++)
        //         {
        //             Cell cell = lane.GetCellAt(x, y);
        //             if (cell == null)
        //             {
        //                 continue;
        //             }
        //
        //             unit = cell.Unit;
        //             if (unit != null && unit.player != player && unit.health > 0)
        //             {
        //                 targetsInRange.Add(unit);
        //                 //return true;
        //             }
        //         }
        //     }
        //
        //     if (targetsInRange.Count > 0)
        //     {
        //         targetsInRange = SortTargetsInRange(targetsInRange);
        //         unit = targetsInRange[0];
        //         return true;
        //     }
        //
        //     unit = null;
        //     return false;
        // }

    }
}