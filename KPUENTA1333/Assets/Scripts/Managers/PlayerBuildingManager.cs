using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuildingManager
{
    private Player _owner;
    private GameManager _gameManager;

    private List<PlacedBuildingBase> _ownedBuildings = new();

    private Action _tick;

    public PlayerBuildingManager(Player owner, GameManager gameManager)
    {
        _owner = owner;
        _gameManager = gameManager;
    }

    public void AddBuilding(PlacedBuildingBase placedBuilding)
    {
        _ownedBuildings.Add(placedBuilding);
        placedBuilding.SetManager(this, ref _tick, _owner);
        var cellsBuildingIsOn = _gameManager.GameGrid.GetCellsAroundPosition(placedBuilding.transform.position, 0);
        foreach (var cell in cellsBuildingIsOn)
        {
            cell.AddBuildingToCell(placedBuilding);
        }
        placedBuilding.OnPlaced();
    }

    public void OnUpdate()
    {
        _tick?.Invoke();
    }


    public void AddBuildingCellEffect(Vector3 position, int range, int unitHealthChange)
    {
        var cells = _gameManager.GameGrid.GetCellsAroundPosition(position, range);
        foreach (var cell in cells)
        {
            cell.ModifyHpChangePerTickInCell(unitHealthChange);
        }
    }

    public void RemoveBuildingCellEffect(Vector3 position, int range, int unitHealthChange)
    {
        var cells = _gameManager.GameGrid.GetCellsAroundPosition(position, range);
        foreach (var cell in cells)
        {
            cell.ModifyHpChangePerTickInCell(-unitHealthChange);
        }
    }
}
