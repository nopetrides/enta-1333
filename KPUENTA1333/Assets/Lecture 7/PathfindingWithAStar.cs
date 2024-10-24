using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;


/// <summary>
/// From https://github.com/anneomcl/PathfindingBasics
/// </summary>
public class PathfindingWithAStar
{
    public PathfindingWithAStar(GameGrid grid, Dictionary<Vector2, GridCell> cells)
    {
        nodeParents = new Dictionary<Vector2, Vector2>();
        walkablePositions = new Dictionary<Vector2, bool>();
        obstacles = new Dictionary<Vector2,int>();
        _grid = grid;
        UpdateWalkableObstacles(cells);
    }

    // refresh each time the grid updates
    public IDictionary<Vector2, bool> walkablePositions;
    public IDictionary<Vector2, int> obstacles;

    IDictionary<Vector2, Vector2> nodeParents;
    private GameGrid _grid;

    public void UpdateWalkableObstacles(Dictionary<Vector2, GridCell> cells)
    {
        foreach (var cell in cells)
        {
            if (!walkablePositions.ContainsKey(cell.Key))
            {
                walkablePositions.Add(cell.Key, true);
            }
            walkablePositions[cell.Key] = cell.Value.Walkable;

            if (!walkablePositions.ContainsKey(cell.Key))
            {
                walkablePositions.Add(cell.Key, true);
            }
            walkablePositions[cell.Key] = cell.Value.Walkable;

            if (!obstacles.ContainsKey(cell.Key))
            {
                obstacles.Add(cell.Key, 1);
            }
            obstacles[cell.Key] = cell.Value.ObstacleLevel;
        }
    }

    private bool CanMove(Vector2 nextPosition)
    {
        return (walkablePositions.ContainsKey(nextPosition) ? walkablePositions[nextPosition] : false);
    }

    private IList<Vector2> GetWalkableNodes(Vector2 curr)
    {
        IList<Vector2> walkableNodes = new List<Vector2>();

        IList<Vector2> possibleNodes = new List<Vector2>() {
            new Vector2 (curr.x + 1, curr.y),
            new Vector2 (curr.x - 1, curr.y),
            new Vector2 (curr.x, curr.y + 1),
            new Vector2 (curr.x, curr.y - 1),
            new Vector2 (curr.x + 1, curr.y + 1),
            new Vector2 (curr.x + 1, curr.y - 1),
            new Vector2 (curr.x - 1, curr.y + 1),
            new Vector2 (curr.x - 1, curr.y - 1)
        };

        foreach (Vector2 node in possibleNodes)
        {
            if (CanMove(node))
            {
                walkableNodes.Add(node);
            }
        }

        return walkableNodes;
    }

    private int HeuristicCostEstimate(Vector2 node, Vector2 goal, string heuristic)
    {
        switch (heuristic)
        {
            case "euclidean":
                return EuclideanEstimate(node, goal);
            case "manhattan":
                return ManhattanEstimate(node, goal);
        }

        return -1;
    }

    private int EuclideanEstimate(Vector2 node, Vector2 goal)
    {
        return (int)Mathf.Sqrt(Mathf.Pow(node.x - goal.x, 2) +
            Mathf.Pow(node.y - goal.y, 2));
    }

    private int ManhattanEstimate(Vector2 node, Vector2 goal)
    {
        return (int)(Mathf.Abs(node.x - goal.x) +
            Mathf.Abs(node.y - goal.y));
    }

    private int Weight(Vector2 node)
    {
        if (obstacles.Keys.Contains(node))
        {
            return obstacles[node];
        }
        else
        {
            return 1;
        }
    }

    public IList<Vector2> FindShortestPath(PathfindingType algorithm, Vector3 currentPosition, Vector3 goalPosition)
    {
        Vector2 startCellId = _grid.CellIdFromPosition(currentPosition);
        Vector2 goalCellid = _grid.CellIdFromPosition(goalPosition);

        IList<Vector2> path = new List<Vector2>();
        Vector2 goal;
        switch (algorithm)
        {
            case PathfindingType.AStarEuclid:
                goal = FindShortestPathAStar(startCellId, goalCellid, "euclidean");
                break;
            default:
                goal = FindShortestPathAStar(startCellId, goalCellid, "manhattan");
                break;
        }
        

        if (goal == startCellId || !nodeParents.ContainsKey(nodeParents[goal]))
        {
            //No solution was found.
            return null;
        }

        Vector2 curr = goal;
        while (curr != startCellId)
        {
            path.Add(curr);
            curr = nodeParents[curr];
        }

        return path;
    }

    private Vector2 FindShortestPathAStar(Vector2 startPosition, Vector2 goalPosition, string heuristic)
    {

        uint nodeVisitCount = 0;
        float timeNow = Time.realtimeSinceStartup;

        // A* tries to minimize f(x) = g(x) + h(x), where g(x) is the distance from the start to node "x" and
        //    h(x) is some heuristic that must be admissible, meaning it never overestimates the cost to the next node.
        //    There are formal logical proofs you can look up that determine how heuristics are and are not admissible.

        IEnumerable<Vector2> validNodes = walkablePositions
            .Where(x => x.Value == true)
            .Select(x => x.Key);

        // Represents h(x) or the score from whatever heuristic we're using
        IDictionary<Vector2, int> heuristicScore = new Dictionary<Vector2, int>();

        // Represents g(x) or the distance from start to node "x" (Same meaning as in Dijkstra's "distances")
        IDictionary<Vector2, int> distanceFromStart = new Dictionary<Vector2, int>();

        foreach (Vector2 vertex in validNodes)
        {
            heuristicScore.Add(new KeyValuePair<Vector2, int>(vertex, int.MaxValue));
            distanceFromStart.Add(new KeyValuePair<Vector2, int>(vertex, int.MaxValue));
        }

        heuristicScore[startPosition] = HeuristicCostEstimate(startPosition, goalPosition, heuristic);
        distanceFromStart[startPosition] = 0;

        // The item dequeued from a priority queue will always be the one with the lowest int value
        //    In this case we will input nodes with their calculated distances from the start g(x),
        //    so we will always take the node with the lowest distance from the queue.
        SimplePriorityQueue<Vector2, int> priorityQueue = new SimplePriorityQueue<Vector2, int>();
        priorityQueue.Enqueue(startPosition, heuristicScore[startPosition]);

        while (priorityQueue.Count > 0)
        {
            // Get the node with the least distance from the start
            Vector2 curr = priorityQueue.Dequeue();
            nodeVisitCount++;

            // If our current node is the goal then stop
            if (curr == goalPosition)
            {
                //Debug.Log("A*" + heuristic + ": " + distanceFromStart[goalPosition]);
                //Debug.Log("A*" + heuristic + " time: " + (Time.realtimeSinceStartup - timeNow).ToString());
                //Debug.Log(string.Format("A* {0} visits: {1} ({2:F2}%)", heuristic, nodeVisitCount, (nodeVisitCount / (double)walkablePositions.Count) * 100));
                return goalPosition;
            }

            IList<Vector2> neighbors = GetWalkableNodes(curr);

            foreach (Vector2 node in neighbors)
            {
                // Get the distance so far, add it to the distance to the neighbor
                int currScore = distanceFromStart[curr] + Weight(node);

                // If our distance to this neighbor is LESS than another calculated shortest path
                //    to this neighbor, set a new node parent and update the scores as our current
                //    best for the path so far.
                if (currScore < distanceFromStart[node])
                {
                    nodeParents[node] = curr;
                    distanceFromStart[node] = currScore;

                    int hScore = distanceFromStart[node] + HeuristicCostEstimate(node, goalPosition, heuristic);
                    heuristicScore[node] = hScore;

                    // If this node isn't already in the queue, make sure to add it. Since the
                    //    algorithm is always looking for the smallest distance, any existing entry
                    //    would have a higher priority anyway.
                    if (!priorityQueue.Contains(node))
                    {
                        priorityQueue.Enqueue(node, hScore);
                    }
                }
            }
        }

        return startPosition;
    }

    public enum PathfindingType
    {
        AStarEuclid,
        AStarManhattan
    }
}
