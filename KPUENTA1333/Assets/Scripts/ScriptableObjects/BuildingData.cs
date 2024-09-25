using UnityEngine;

[CreateAssetMenu(
    fileName = "BuildingData",
    menuName = "Create Scriptable Objects/Building Data")]
public class BuildingData : ScriptableObject
{
    [SerializeField] private int[] _maxHp;
    [SerializeField] private int _armor;
    [SerializeField] private GameObject _buildingGhostPrefab;
    [SerializeField] private PlacedBuildingBase _buildingPlacedPrefab;
    [SerializeField] private Sprite _buildingSprite;
    [SerializeField] private BuildingType _kindOfStructure;
    [SerializeField] private string _buildingName;

    public int[] MaxHp => _maxHp;
    public int Armor => _armor;
    public GameObject BuildingGhostPrefab => _buildingGhostPrefab;
    public PlacedBuildingBase BuildingPlacedPrefab => _buildingPlacedPrefab;
    public Sprite BuildingSprite => _buildingSprite;
    public BuildingType KindOfStructure => _kindOfStructure;
    public string BuildingName => _buildingName;

    public bool CanLevelUp(int currentLevel)
    {
        if (currentLevel <= 0)
            return false;

        bool isMaxLevel = currentLevel < _maxHp.Length;
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