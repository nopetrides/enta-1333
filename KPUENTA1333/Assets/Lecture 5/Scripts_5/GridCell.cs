using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridCell
{
    
    private List<CellUnit> _allUnits = new List<CellUnit>();
    Dictionary<int, Dictionary<string, CellUnit>> _unitsInCellByFaction = new Dictionary<int, Dictionary<string, CellUnit>>();
    private PlacedBuildingBase _buildingInCell;
    private GameGrid _parentGrid;
    private int _netHpChangePerTick;
    
    // option 1, damage all units in the damage radius of this building, if it does aoe damage
    // option 2, store a value which is the amount of damage units take when they remain in this cell

    public GridCell(GameGrid grid)
    {
        _parentGrid = grid;
    }
    
    public void AddUnitToCell(CellUnit unit)
    {
        if (!_unitsInCellByFaction.ContainsKey(unit.Faction)) 
        {
            _unitsInCellByFaction[unit.Faction] = new();
        }
        
        if (_unitsInCellByFaction[unit.Faction].ContainsKey(unit.name))
        {
            Debug.LogError("Trying to add unit to a grid cell they already belong to!");
            return;
        }

        _unitsInCellByFaction[unit.Faction].Add(unit.name, unit);

        if (unit.CurrentCell != null)
        {
            unit.CurrentCell.RemoveUnitFromCell(unit);
        }

        unit.SetCell(this);
    }

    public void RemoveUnitFromCell(CellUnit unit)
    {
        _unitsInCellByFaction[unit.Faction].Remove(unit.name);
    }

    public List<CellUnit> GetOtherFactionUnits(int factionToIgnore)
    {
        // filter by faction
        var otherFactions = _unitsInCellByFaction.Where(x => x.Key != factionToIgnore);
        // get dictionary of units in each faction
        var factionLists = otherFactions.Select(x => x.Value);
        // get all units in the dictionary
        return factionLists.SelectMany(x => x.Values).ToList();
    }


    public void OnTick()
    {
        // option 1: DamageSystem.DoDamage(source, targets)
        
        // option 2: unit.UnitManager.DoDamage(source);
        foreach (var unit in _allUnits)
        {
            _parentGrid.Manager.CombatSystem.ChangeHp(this, unit, _netHpChangePerTick);
        }
    }

    public void ModifyHpChangePerTickInCell(int change)
    {
        _netHpChangePerTick += change;
    }

    public void AddBuildingToCell(PlacedBuildingBase buildingInCell)
    {
        _buildingInCell = buildingInCell;
    }
}