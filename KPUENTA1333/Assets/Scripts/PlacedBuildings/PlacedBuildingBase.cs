using System;
using UnityEngine;

public class PlacedBuildingBase : MonoBehaviour
{
    [SerializeField] private BuildingData ScriptedObjectData;

    private int _currentHp;
    private int _buildingLevel;

    protected PlayerBuildingManager _manager;
    protected Player _owner;

    private void Start()
    {
        _currentHp = ScriptedObjectData.MaxHp[0];
        _buildingLevel = 1;
    }

    public void SetManager(PlayerBuildingManager manager, ref Action onTick, Player owner)
    {
        _manager = manager;
        onTick += Tick;
        _owner = owner;
    }

    public void CalculateDamage(int damageReceived)
    {
        damageReceived -= ScriptedObjectData.Armor;
        TakeDamage(damageReceived);
    }
    
    private void TakeDamage(int damageTaken)
    {
        _currentHp -= damageTaken;
    }

    public void CanLevelUp()
    {
        ScriptedObjectData.CanLevelUp(_buildingLevel);
    }

    protected virtual void Tick()
    {
    }
}
