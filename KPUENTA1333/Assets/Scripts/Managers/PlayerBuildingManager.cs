using System;
using System.Collections.Generic;

public class PlayerBuildingManager
{
    private Player _owner;

    private List<PlacedBuildingBase> _ownedBuildings = new();

    private Action Tick;

    public PlayerBuildingManager(Player owner)
    {
        _owner = owner;
    }

    public void AddBuilding(PlacedBuildingBase placedBuilding)
    {
        _ownedBuildings.Add(placedBuilding);
        placedBuilding.SetManager(this, ref Tick, _owner);
    }

    public void OnUpdate()
    {
        Tick?.Invoke();
    }
}
