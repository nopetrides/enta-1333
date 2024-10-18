

using UnityEngine;

public class PathNode
{
    public int CellSize { get; private set; }
    
    
    public PathNode Parent;
    public Vector2 Position;

    public Vector2 Center => new(Position.x + CellSize / 2f, Position.y + CellSize / 2f);

    public Vector2 GetCenter()
    {
        return new Vector2(Position.x + CellSize / 2f, Position.y + CellSize / 2f);
    }

    public float DistanceToTarget, Cost, Weight;
    public float CostToTarget
    {
        get
        {
            if (DistanceToTarget != -1 && Cost != -1)
                return DistanceToTarget + Cost;
            else
                return -1;
        }
    }
    public bool Walkable;
    
    
    public PathNode(int cellSize, Vector2 pos, bool walkable, float weight = 1)
    {
        CellSize = cellSize;
        Parent = null;
        Position = pos;
        DistanceToTarget = -1;
        Cost = 1;
        Weight = weight;
        Walkable = walkable;
    }
}