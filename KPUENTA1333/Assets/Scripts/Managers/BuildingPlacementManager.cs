using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// This is the controller for placing buildings.
/// It receives input via the <see cref="BuildingPlacementUI"/>
/// </summary>
public class BuildingPlacementManager : MonoBehaviour
{
    [SerializeField] private AllBuildingData _allBuildingData;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private ParticleSystem _placementParticle;
    [SerializeField] private Material _canPlaceMaterial;
    [SerializeField] private Material _cannotPlaceMaterial;
    [SerializeField] private LayerMask _buildingMask;
    [SerializeField] private DismantleBuildingUI _dismantleUI;

    public AllBuildingData AllBuildings => _allBuildingData;
    
    private BuildingData _buildingToPlace = null;
    private GameObject _placementGhost = null;
    private PlayerBuildingManager _localPlayerBuildingManager = null;
    private bool _allowPlace = true;

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
    private Dictionary<string, GameObject> _ghostObjects = new();

    private void Update()
    {
        if (!_allowPlace)
        {
            return;
        }
        RaycastHit hitInfo;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (_buildingToPlace == null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(ray, out hitInfo, 20000, _buildingMask))
                {
                    Debug.Log(hitInfo.collider.name);
                    var buildingClicked = hitInfo.transform.GetComponentInParent<PlacedBuildingBase>();
                    _dismantleUI.Open(buildingClicked);
                }
            }
            return;
        }

        if (Physics.Raycast(ray, out hitInfo, 20000, _groundMask))
        {
            if (_placementGhost != null)
            {
                _placementGhost.SetActive(false);
            }

            if (_ghostObjects.TryGetValue(_buildingToPlace.BuildingGhostPrefab.name, out var existingGhost))
            {
                _placementGhost = existingGhost;
                _placementGhost.SetActive(true);
            }
            else
            {
                _placementGhost = Instantiate(_buildingToPlace.BuildingGhostPrefab, transform);
                _ghostObjects.Add(_buildingToPlace.BuildingGhostPrefab.name, _placementGhost);
                ValidPlacement();
            }

            _placementGhost.transform.position = hitInfo.point;

            if (Input.GetMouseButtonDown(0))
            {
                PlaceBuilding(hitInfo.point);
            }
        }
    }

    /// <summary>
    ///     Places a building at the designated location
    /// </summary>
    /// <param name="position"></param>
    private void PlaceBuilding(Vector3 position)
    {
        _placementParticle.transform.position = position;
        _placementParticle.Play();

        var placedBuilding = Instantiate<PlacedBuildingBase>(_buildingToPlace.BuildingPlacedPrefab, position, Quaternion.identity);


        _localPlayerBuildingManager.AddBuilding(placedBuilding);
        
        _placementGhost.SetActive(false);
        _placementGhost = null;
        _buildingToPlace = null;
    }

    private void InvalidPlacement()
    {
        if (_placementGhost != null)
        {
            var mrList = _placementGhost.GetComponentsInChildren<MeshRenderer>();
            foreach (var mr in mrList)
            {
                var mats = mr.materials;
                for (int i = 0; i < mats.Length; i++)
                {
                    mats[i] = _cannotPlaceMaterial;
                }
                mr.SetMaterials(mats.ToList());
            }
        }
    }

    private void ValidPlacement()
    {
        if (_placementGhost != null)
        {
            var mrList = _placementGhost.GetComponentsInChildren<MeshRenderer>();
            foreach(var mr in mrList)
            {
                var mats = mr.materials;
                for (int i = 0; i < mats.Length; i++)
                {
                    mats[i] = _canPlaceMaterial;
                }
                mr.SetMaterials(mats.ToList());
            }
        }
    }

    public void SetLocalBuildingManager(PlayerBuildingManager playerBuildingManager)
    {
        _localPlayerBuildingManager = playerBuildingManager;
    }

    public void TogglePlacement(bool canPlace)
    {
        _allowPlace = canPlace;
    }
}
