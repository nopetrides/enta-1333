using System;
using UnityEngine;

public class PlacedBuildingBase : MonoBehaviour
{
    [SerializeField] private BuildingData ScriptedObjectData;

    private int _currentHp;
    private int _buildingLevel;

    protected PlayerBuildingManager Manager;
    protected Player Owner;

    private void Start()
    {
        _currentHp = ScriptedObjectData.MaxHpPerLevel[0];
        _buildingLevel = 1;
    }

    public void SetManager(PlayerBuildingManager manager, ref Action onTick, Player owner)
    {
        Manager = manager;
        onTick += Tick;
        Owner = owner;
    }

    public int GetFaction()
    {
        return Owner.PlayerFaction;
    }

    public void CalculateDamage(int damageReceived)
    {
        damageReceived -= ScriptedObjectData.CurrentArmor;
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

    public virtual void OnPlaced() { }
    public virtual void OnRemoved() { }
}
