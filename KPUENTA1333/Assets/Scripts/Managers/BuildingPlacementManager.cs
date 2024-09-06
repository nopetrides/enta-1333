using UnityEngine;

/// <summary>
/// This is the controller for placing buildings.
/// It receives input via the <see cref="BuildingPlacementUI"/>
/// </summary>
public class BuildingPlacementManager : MonoBehaviour
{
    [SerializeField] private AllBuildingData _allBuildingData;
    [SerializeField] private LayerMask GroundMask;
    public AllBuildingData AllBuildings => _allBuildingData;
    
    private BuildingData _buildingToPlace = null;
    private GameObject _placementGhost = null;

    /// <summary>
    /// Called by the <see cref="BuildingPlacementUI"/>
    /// </summary>
    public void OnNewBuildingSelected(BuildingData building)
    {
        _buildingToPlace = building;
    }

    /// <summary>
    /// If we have a <see cref="_buildingToPlace"/> then show the ghost for it at the mouse position
    /// This will need to calculate where ground is.
    /// </summary>
    private void Update()
    {
        if (_buildingToPlace == null)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 20, GroundMask))
        {
            Debug.Log(hitInfo.collider.name);
            
            if (_placementGhost == null )
                _placementGhost = Instantiate(_buildingToPlace.BuildingGhostPrefab, transform);
            _placementGhost.transform.position = hitInfo.point;
        }
    }

    private void PlaceBuilding()
    {

    }
}
