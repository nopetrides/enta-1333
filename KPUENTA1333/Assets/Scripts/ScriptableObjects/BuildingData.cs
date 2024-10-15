using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(
    fileName = "BuildingData",
    menuName = "Create Scriptable Objects/Building Data")]
public class BuildingData : ScriptableObject
{
    [FormerlySerializedAs("_maxHp")] [SerializeField] private int[] MaxHp;
    [FormerlySerializedAs("_armor")] [SerializeField] private int Armor;
    [FormerlySerializedAs("_buildingGhostPrefab")] [SerializeField] private GameObject BuildingGhostPrefab;
    [FormerlySerializedAs("_buildingPlacedPrefab")] [SerializeField] private PlacedBuildingBase BuildingPlacedPrefab;
    [FormerlySerializedAs("_buildingSprite")] [SerializeField] private Sprite BuildingSprite;
    [FormerlySerializedAs("_kindOfStructure")] [SerializeField] private BuildingType KindOfStructure;
    [FormerlySerializedAs("_buildingName")] [SerializeField] private string BuildingName;

    public int[] MaxHpPerLevel => MaxHp;
    public int CurrentArmor => Armor;
    public GameObject BuildingGhost => BuildingGhostPrefab;
    public PlacedBuildingBase BuildingPlacedBase => BuildingPlacedPrefab;
    public Sprite UiSprite => BuildingSprite;
    public BuildingType StructureType => KindOfStructure;
    public string StructureName => BuildingName;

    public bool CanLevelUp(int currentLevel)
    {
        if (currentLevel <= 0)
            return false;

        bool isMaxLevel = currentLevel < MaxHp.Length;
        return !isMaxLevel;
    }
}

public enum BuildingType
{
    None = 0,
    Decor = 1,
    Heart = 2,
    Defensive = 3,
    Passive = 4,
    ResourceProduction = 5,
}