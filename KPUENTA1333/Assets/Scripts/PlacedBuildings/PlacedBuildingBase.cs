using System;
using UnityEngine;

public class PlacedBuildingBase : MonoBehaviour, ISaveData
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

    public string OnSave(ISaveData dataToSerialize)
    {
        SavedBuildingData data = new SavedBuildingData();
        data.CurrentHp = _currentHp;
        data.BuildingLevel = _buildingLevel;
        data.Position = transform.position;
        return JsonUtility.ToJson(data);
    }

    public ISaveData OnLoad(string serializedData)
    {
        SavedBuildingData data = JsonUtility.FromJson<SavedBuildingData>(serializedData);
        _currentHp = data.CurrentHp;
        _buildingLevel = data.BuildingLevel;
        transform.position = data.Position;
        return data;
    }
}
