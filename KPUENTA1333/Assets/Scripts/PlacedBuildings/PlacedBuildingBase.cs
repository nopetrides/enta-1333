using UnityEngine;

public class PlacedBuildingBase : MonoBehaviour
{
    [SerializeField] private BuildingData ScriptedObjectData;

    private int CurrentHp;
    private int BuildingLevel;

    private void Start()
    {
        CurrentHp = ScriptedObjectData.MaxHp[0];
        BuildingLevel = 1;
    }

    public void CalculateDamage(int damageReceived)
    {
        damageReceived -= ScriptedObjectData.Armor;
        TakeDamage(damageReceived);
    }
    
    private void TakeDamage(int damageTaken)
    {
        CurrentHp -= damageTaken;
    }

    public void CanLevelUp()
    {
        ScriptedObjectData.CanLevelUp(BuildingLevel);
    }
}
