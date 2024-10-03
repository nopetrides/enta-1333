using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridCell
{
    Dictionary<int, Dictionary<string, CellUnit>> UnitsInCellByFaction = new Dictionary<int, Dictionary<string, CellUnit>>();
    
    public void AddUnitToCell(CellUnit unit)
    {
        if (!UnitsInCellByFaction.ContainsKey(unit.Faction)) 
        {
            UnitsInCellByFaction[unit.Faction] = new();
        }
        
        if (UnitsInCellByFaction[unit.Faction].ContainsKey(unit.name))
        {
            Debug.LogError("Trying to add unit to a grid cell they already belong to!");
            return;
        }

        UnitsInCellByFaction[unit.Faction].Add(unit.name, unit);

        if (unit.CurrentCell != null)
        {
            unit.CurrentCell.RemoveUnitFromCell(unit);
        }

        unit.SetCell(this);
    }

    public void RemoveUnitFromCell(CellUnit unit)
    {
        UnitsInCellByFaction[unit.Faction].Remove(unit.name);
    }

    public List<CellUnit> GetOtherFactionUnits(int factionToIgnore)
    {
        // filter by faction
        var otherFactions = UnitsInCellByFaction.Where(x => x.Key != factionToIgnore);
        // get dictionary of units in each faction
        var factionLists = otherFactions.Select(x => x.Value);
        // get all units in the dictionary
        return factionLists.SelectMany(x => x.Values).ToList();
    }
}