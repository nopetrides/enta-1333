using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpatialTester : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;

    [SerializeField] private CellUnit _factionUnitA;
    [SerializeField] private Transform _unitParentA;
    [SerializeField] private CellUnit _factionUnitB;
    [SerializeField] private Transform _unitParentB;

    [SerializeField] private int _unitsToTestWith;
    [SerializeField] private LayerMask _groundMask;

    // todo, use pooling
    private List<CellUnit> _unitsListA = new();
    private List<CellUnit> _unitsListB = new();

    private void Start()
    {
        GenerateUnits(1, _factionUnitA, _unitParentA, ref _unitsListA);
        GenerateUnits(2, _factionUnitB, _unitParentB, ref _unitsListB);
    }

    private void GenerateUnits(int faction, CellUnit unitPrefab, Transform parent, ref List<CellUnit> list)
    {
        int mapWidth = _gameManager.GameGrid.Width * _gameManager.GameGrid.CellSize;
        int mapHeight = _gameManager.GameGrid.Height * _gameManager.GameGrid.CellSize;

        for (int i = 0; i < _unitsToTestWith; i++)
        {
            Vector3 randomPos = new Vector3(Random.Range(-mapWidth, mapWidth), 5f, Random.Range(-mapHeight, mapHeight));

            CellUnit cellUnit = Instantiate<CellUnit>(unitPrefab, randomPos, Quaternion.identity, parent);

            cellUnit.Setup(faction, i, _gameManager.GameGrid);

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
            CellUnit closestEnemy = _gameManager.GameGrid.FindClosestOtherFactionUnit(unit);

            if (closestEnemy != null)
            {
                unit.MoveToEnemy(closestEnemy);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (_gameManager == null || _gameManager.GameGrid == null)
        {
            return;
        }
        Gizmos.color = Color.cyan;
        int w = _gameManager.GameGrid.Width;
        int h = _gameManager.GameGrid.Height;
        int size = _gameManager.GameGrid.CellSize;
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

        if (Physics.Raycast(ray, out hitInfo, 20000, _groundMask))
        {
            var pos = _gameManager.GameGrid.GetCellWorldCenter(hitInfo.point);

            Gizmos.DrawWireCube(pos, Vector3.one * _gameManager.GameGrid.CellSize);
        }
    }
}
