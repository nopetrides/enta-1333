using UnityEngine;
using UnityEngine.Serialization;

public class BuildingPlacementUI : MonoBehaviour
{
    [FormerlySerializedAs("_buildingPlacementManager")] [SerializeField] private BuildingPlacementManager BuildingPlacementManager;

    [SerializeField] private SelectBuildingButton SelectBuildingButton;
    [SerializeField] private Transform ScrollRectContent;

    private void Start()
    {
        foreach(var building in BuildingPlacementManager.AllBuildings.DataList)
        {
            SelectBuildingButton button = Instantiate(
                SelectBuildingButton, ScrollRectContent);
            button.Setup(building, BuildingPlacementManager);
        }
    }
}
