using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingWithAStar
{
    public int CellSize { get; private set; }
    public Stack<PathNode> FindPath(Vector2 pathStart, Vector2 pathEnd)
        {
            PathNode start = new PathNode(CellSize, new Vector2((int)(pathStart.x/ CellSize), (int) (pathStart.y/CellSize)), true);
            PathNode end = new PathNode(CellSize, new Vector2((int)(pathEnd.x / CellSize), (int)(pathEnd.y /CellSize)), true);

            Stack<PathNode> path = new Stack<PathNode>();
            PriorityQueue<PathNode, float> openList = new <PathNode,float>();
            List<PathNode> ClosedList = new List<PathNode>();
            List<PathNode> adjacencies;
            PathNode current = start;
           
            // add start node to Open List
            openList.Enqueue(start, start.CostToTarget);

            while(openList.Count != 0 && !ClosedList.Exists(x => x.Position == end.Position))
            {
                current = openList.Dequeue();
                ClosedList.Add(current);
                adjacencies = GetAdjacentNodes(current);

                foreach(PathNode n in adjacencies)
                {
                    if (!ClosedList.Contains(n) && n.Walkable)
                    {
                        bool isFound = false;
                        foreach (var oLNode in openList.UnorderedItems)
                        {
                            if (oLNode.Element == n)
                            {
                                isFound = true;
                            }
                        }
                        if (!isFound)
                        {
                            n.Parent = current;
                            n.DistanceToTarget = Math.Abs(n.Position.X - end.Position.X) + Math.Abs(n.Position.Y - end.Position.Y);
                            n.Cost = n.Weight + n.Parent.Cost;
                            openList.Enqueue(n, n.CostToTarget);
                        }
                    }
                }
            }
            
            // construct path, if end was not closed return null
            if(!ClosedList.Exists(x => x.Position == end.Position))
            {
                return null;
            }

            // if all good, return path
            PathNode temp = ClosedList[ClosedList.IndexOf(current)];
            if (temp == null) return null;
            do
            {
                path.Push(temp);
                temp = temp.Parent;
            } while (temp != start && temp != null) ;
            return path;
        }
}
