using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class CellUnit : MonoBehaviour
{
    [FormerlySerializedAs("_moveSpeed")] [SerializeField] private float MoveSpeed = 5f;

    private int _faction;
    public int Faction => _faction;

    private GridCell _currentCell = null;
    public GridCell CurrentCell => _currentCell;

    private Vector3 _moveTarget;
    private Vector3 _previousPosition;
    private GameGrid _grid;

    /// <summary>
    /// todo Update with max health from scriptable object and only allow health to change via function
    /// </summary>
    private int _health = 20;

    /// <summary>
    /// todo clamp hp to 0 and max hp
    /// </summary>
    /// <param name="change"></param>
    public void ChangeHealth(int change)
    {
        _health += change;
        // todo death check
    }

    public void Setup(int faction, int unitCounter, GameGrid gameGrid)
    {
        _faction = faction;
        name = $"P{faction}_{name}_{unitCounter}";
        _grid = gameGrid;
        GetNewRandomTarget();
    }

    public void SetCell(GridCell gridCell)
    {
        _currentCell = gridCell;
    }

    public void RandomMove()
    {
        // lazy translate move instead of physics
        transform.Translate(Vector3.forward * Time.deltaTime * MoveSpeed);

        _grid.UpdateUnitCell(this, _previousPosition);

        _previousPosition = transform.position;

        if ((transform.position - _moveTarget).magnitude < 10f)
        {
            GetNewRandomTarget();
        }
    }

    private void GetNewRandomTarget()
    {
        int mapWidth = _grid.Width * _grid.CellSize;
        int mapHeight = _grid.Height * _grid.CellSize;

        _moveTarget = new Vector3(Random.Range(-mapWidth, mapWidth), transform.position.y, Random.Range(-mapHeight, mapHeight));

        transform.rotation = Quaternion.LookRotation(_moveTarget - transform.position);
    }

    public void MoveToEnemy(CellUnit otherUnit)
    {
        // in same cell
        if (_grid.CellIdFromPosition(transform.position) == _grid.CellIdFromPosition(otherUnit.transform.position))
        {
            transform.rotation = Quaternion.LookRotation(otherUnit.transform.position - transform.position);
        }
        else
        {
            // else find path

            var path = _grid.Pathfinder.FindShortestPath(PathfindingWithAStar.PathfindingType.AStarEuclid, transform.position, otherUnit.transform.position);

            if (path == null)
            {
                Debug.LogWarning("Failed to find path!");
                return;
            }
            var firstNode = path.FirstOrDefault();
            if (firstNode == null)
            {
                Debug.LogWarning("Failed to find node!");
                return;
            }

            transform.rotation = Quaternion.LookRotation(_grid.GetCellPositionFromId(firstNode));
        }

        transform.Translate(Vector3.forward * Time.deltaTime * MoveSpeed);

        _grid.UpdateUnitCell(this, _previousPosition);

        _previousPosition = transform.position;
    }

    public void MoveToEnemy(PlacedBuildingBase building)
    {
        // same cell
        if (_grid.CellIdFromPosition(transform.position) == _grid.CellIdFromPosition(building.transform.position))
        {
            transform.rotation = Quaternion.LookRotation(building.transform.position - transform.position);
        }
        else
        {
            var path = _grid.Pathfinder.FindShortestPath(PathfindingWithAStar.PathfindingType.AStarEuclid, transform.position, building.transform.position);

            if (path == null)
            {
                Debug.LogWarning("Failed to find path!");
                return;
            }
            var firstNode = path.FirstOrDefault();
            if (firstNode == null)
            {
                Debug.LogWarning("Failed to find node!");
                return;
            }

            transform.rotation = Quaternion.LookRotation(_grid.GetCellPositionFromId(firstNode));
        }

        transform.Translate(Vector3.forward * Time.deltaTime * MoveSpeed);

        _grid.UpdateUnitCell(this, _previousPosition);

        _previousPosition = transform.position;
    }
}
