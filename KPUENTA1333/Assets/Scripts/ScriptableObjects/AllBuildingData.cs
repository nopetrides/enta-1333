using UnityEngine;

[CreateAssetMenu(
    fileName = "AllBuildingsData",
    menuName = "Create Scriptable Objects/All Buildings Data")]
public class AllBuildingData : ScriptableObject
{
    [SerializeField] private BuildingData[] _data;
    public BuildingData[] Data => _data;
}
