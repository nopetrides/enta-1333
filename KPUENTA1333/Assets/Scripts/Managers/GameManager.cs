using System;
using System.Collections.Generic;
using Lecture_6;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    [FormerlySerializedAs("_playerCount")] [SerializeField] private int PlayerCount = 1;
    [FormerlySerializedAs("_placementManager")] [SerializeField] private BuildingPlacementManager PlacementManager;
    [FormerlySerializedAs("_playerUI")] [SerializeField] private LocalPlayerUI PlayerUI;
    [FormerlySerializedAs("_myEvent")] [SerializeField] private UnityEvent MyEvent;
    [FormerlySerializedAs("_UIElements")] [SerializeField] private Canvas[] UIElements;

    // Determine the grid size
    [FormerlySerializedAs("_gridWidth")] [SerializeField] private int GridWidth = 10;
    [FormerlySerializedAs("_gridHeight")] [SerializeField] private int GridHeight = 10;
    [FormerlySerializedAs("_cellSize")] [SerializeField] private int CellSize = 10;
    [FormerlySerializedAs("_cellTickRate")] [SerializeField] private float CellTickRate;
    [FormerlySerializedAs("_damageSystem")] [SerializeField] private DamageSystem DamageSystem;

    private List<Player> _playerController = new List<Player>();
    
    private delegate void DisableUI();
    private DisableUI _toggleDelegate;

    private GameGrid _gameGrid;
    public GameGrid GameGrid => _gameGrid;
    
    public DamageSystem CombatSystem => DamageSystem;
    
    void Awake()
    {
        _toggleDelegate += ToggleAllUI;
        _toggleDelegate.Invoke();

        PlacementManager.SetGameManager(this);

        for (int i = 0; i < PlayerCount; i++)
        {
            var player = new Player(i, this);
            _playerController.Add(player);
            if (i == 0)
            {
                PlacementManager.SetLocalBuildingManager(player.BuildingManager);
                PlayerUI.SubscribeToPlayerUpdates(player);
            }
        }
        _gameGrid = new GameGrid(GridWidth, GridHeight, CellSize, CellTickRate, this);
    }

    private void Update()
    {
        if (Input.GetButtonDown("ToggleUI"))
        {
            _toggleDelegate.Invoke();
        }

        foreach (var p in _playerController)
        {
            p.BuildingManager.OnUpdate();
        }

        _gameGrid.OnUpdate();
    }

    private void ToggleAllUI()
    {
        foreach (var ui in UIElements)
        {
            ui.enabled = !ui.enabled;
        }
    }
}
