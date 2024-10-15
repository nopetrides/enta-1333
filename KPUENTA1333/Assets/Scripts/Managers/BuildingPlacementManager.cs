using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using static UnityEditor.FilePathAttribute;

/// <summary>
/// This is the controller for placing buildings.
/// It receives input via the <see cref="BuildingPlacementUI"/>
/// </summary>
public class BuildingPlacementManager : MonoBehaviour
{
    [FormerlySerializedAs("_allBuildingData")] [SerializeField] private AllBuildingData AllBuildingData;
    [FormerlySerializedAs("_groundMask")] [SerializeField] private LayerMask GroundMask;
    [FormerlySerializedAs("_placementParticle")] [SerializeField] private ParticleSystem PlacementParticle;
    [FormerlySerializedAs("_canPlaceMaterial")] [SerializeField] private Material CanPlaceMaterial;
    [FormerlySerializedAs("_cannotPlaceMaterial")] [SerializeField] private Material CannotPlaceMaterial;
    [FormerlySerializedAs("_buildingMask")] [SerializeField] private LayerMask BuildingMask;
    [FormerlySerializedAs("_dismantleUI")] [SerializeField] private DismantleBuildingUI DismantleUI;

    [FormerlySerializedAs("_gameManager")] [SerializeField] private GameManager GameManager;

    public AllBuildingData AllBuildings => AllBuildingData;
    
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
                if (Physics.Raycast(ray, out hitInfo, 20000, BuildingMask))
                {
                    Debug.Log(hitInfo.collider.name);
                    var buildingClicked = hitInfo.transform.GetComponentInParent<PlacedBuildingBase>();
                    DismantleUI.Open(buildingClicked);
                }
            }
            return;
        }

        if (Physics.Raycast(ray, out hitInfo, 20000, GroundMask))
        {
            if (_placementGhost != null)
            {
                _placementGhost.SetActive(false);
            }

            if (_ghostObjects.TryGetValue(_buildingToPlace.BuildingGhost.name, out var existingGhost))
            {
                _placementGhost = existingGhost;
                _placementGhost.SetActive(true);
            }
            else
            {
                _placementGhost = Instantiate(_buildingToPlace.BuildingGhost, transform);
                _ghostObjects.Add(_buildingToPlace.BuildingGhost.name, _placementGhost);
                ValidPlacement();
            }

            var pos = GameManager.GameGrid.GetCellWorldCenter(hitInfo.point);

            _placementGhost.transform.position = pos;

            if (Input.GetMouseButtonDown(0))
            {
                PlaceBuilding(pos);
            }
        }
    }

    /// <summary>
    ///     Places a building at the designated location
    /// </summary>
    /// <param name="position"></param>
    private void PlaceBuilding(Vector3 position)
    {
        PlacementParticle.transform.position = position;
        PlacementParticle.Play();

        var placedBuilding = Instantiate<PlacedBuildingBase>(_buildingToPlace.BuildingPlacedBase, position, Quaternion.identity);


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
                    mats[i] = CannotPlaceMaterial;
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
                    mats[i] = CanPlaceMaterial;
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

    internal void SetGameManager(GameManager gameManager)
    {
        GameManager = gameManager;
    }
}
