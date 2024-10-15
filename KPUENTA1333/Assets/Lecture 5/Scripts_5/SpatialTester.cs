using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SpatialTester : MonoBehaviour
{
    [FormerlySerializedAs("_gameManager")] [SerializeField] private GameManager GameManager;

    [FormerlySerializedAs("_factionUnitA")] [SerializeField] private CellUnit FactionUnitA;
    [FormerlySerializedAs("_unitParentA")] [SerializeField] private Transform UnitParentA;
    [FormerlySerializedAs("_factionUnitB")] [SerializeField] private CellUnit FactionUnitB;
    [FormerlySerializedAs("_unitParentB")] [SerializeField] private Transform UnitParentB;

    [FormerlySerializedAs("_unitsToTestWith")] [SerializeField] private int UnitsToTestWith;
    [FormerlySerializedAs("_groundMask")] [SerializeField] private LayerMask GroundMask;

    // todo, use pooling
    private List<CellUnit> _unitsListA = new();
    private List<CellUnit> _unitsListB = new();

    private void Start()
    {
        GenerateUnits(1, FactionUnitA, UnitParentA, ref _unitsListA);
        GenerateUnits(2, FactionUnitB, UnitParentB, ref _unitsListB);
    }

    private void GenerateUnits(int faction, CellUnit unitPrefab, Transform parent, ref List<CellUnit> list)
    {
        int mapWidth = GameManager.GameGrid.Width * GameManager.GameGrid.CellSize;
        int mapHeight = GameManager.GameGrid.Height * GameManager.GameGrid.CellSize;

        for (int i = 0; i < UnitsToTestWith; i++)
        {
            Vector3 randomPos = new Vector3(Random.Range(-mapWidth, mapWidth), 5f, Random.Range(-mapHeight, mapHeight));

            CellUnit cellUnit = Instantiate<CellUnit>(unitPrefab, randomPos, Quaternion.identity, parent);

            cellUnit.Setup(faction, i, GameManager.GameGrid);

            list.Add(cellUnit);
        }
    }

    private void Update()
    {
        foreach(var unit in _unitsListA)
        {
            unit.RandomMove();
        }

        foreach (var unit in _unitsListB)
        {
            CellUnit closestEnemy = GameManager.GameGrid.FindClosestOtherFactionUnit(unit);

            if (closestEnemy != null)
            {
                unit.MoveToEnemy(closestEnemy);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (GameManager == null || GameManager.GameGrid == null)
        {
            return;
        }
        Gizmos.color = Color.cyan;
        int w = GameManager.GameGrid.Width;
        int h = GameManager.GameGrid.Height;
        int size = GameManager.GameGrid.CellSize;
        float halfSize = size / 2f;

        float posX = 0 - ((w * size) + halfSize);
        for (int i = -w; i <= w + 1; i++)
        {
            float posZ = 0 - ((h * size) + halfSize);
            for (int j = -h; j <= h + 1; j++)
            {
                posZ += size;

                Gizmos.DrawWireCube((new Vector3(posX, 0, posZ)), Vector3.one * size);

                //Debug.Log($"{posX},{posZ}");
            }
            posX += size;
        }

        Gizmos.color = Color.yellow;

        RaycastHit hitInfo;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hitInfo, 20000, GroundMask))
        {
            var pos = GameManager.GameGrid.GetCellWorldCenter(hitInfo.point);

            Gizmos.DrawWireCube(pos, Vector3.one * GameManager.GameGrid.CellSize);
        }
    }
}
