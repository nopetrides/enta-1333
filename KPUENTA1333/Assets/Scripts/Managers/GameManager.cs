using System;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _playerCount = 1;
    [SerializeField] private BuildingPlacementManager _placementManager;
    [SerializeField] private LocalPlayerUI _playerUI;
    [SerializeField] private UnityEvent _myEvent;
    [SerializeField] private Canvas[] _UIElements;

    // Determine the grid size
    [SerializeField] private int _gridWidth = 10, _gridHeight = 10, _cellSize = 10;

    private List<Player> playerController = new List<Player>();
    
    private delegate void DisableUI();
    private DisableUI toggleDelegate;

    private GameGrid _gameGrid;
    public GameGrid GameGrid => _gameGrid;
    
    void Awake()
    {
        toggleDelegate += ToggleAllUI;
        toggleDelegate.Invoke();

        _placementManager.SetGameManager(this);

        for (int i = 0; i < _playerCount; i++)
        {
            var player = new Player(i);
            playerController.Add(player);
            if (i == 0)
            {
                _placementManager.SetLocalBuildingManager(player.BuildingManager);
                _playerUI.SubscribeToPlayerUpdates(player);
            }
        }
        _gameGrid = new GameGrid(_gridWidth, _gridHeight, _cellSize);
    }

    private void Update()
    {
        if (Input.GetButtonDown("ToggleUI"))
        {
            toggleDelegate.Invoke();
        }

        foreach (var p in playerController)
        {
            p.BuildingManager.OnUpdate();
        }
    }

    private void ToggleAllUI()
    {
        foreach (var ui in _UIElements)
        {
            ui.enabled = !ui.enabled;
        }
    }
}
