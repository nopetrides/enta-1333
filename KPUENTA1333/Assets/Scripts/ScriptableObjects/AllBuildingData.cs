using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(
    fileName = "AllBuildingsData",
    menuName = "Create Scriptable Objects/All Buildings Data")]
public class AllBuildingData : ScriptableObject
{
    [FormerlySerializedAs("_data")] [SerializeField] private BuildingData[] Data;
    public BuildingData[] DataList => Data;
}
