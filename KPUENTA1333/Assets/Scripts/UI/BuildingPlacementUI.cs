using UnityEngine;

public class BuildingPlacementUI : MonoBehaviour
{
    [SerializeField] private BuildingPlacementManager _buildingPlacementManager;

    [SerializeField] private SelectBuildingButton SelectBuildingButton;
    [SerializeField] private Transform ScrollRectContent;

    private void Start()
    {
        foreach(var building in _buildingPlacementManager.AllBuildings.Data)
        {
            SelectBuildingButton button = Instantiate(
                SelectBuildingButton, ScrollRectContent);
            button.Setup(building, _buildingPlacementManager);
        }
    }
}
