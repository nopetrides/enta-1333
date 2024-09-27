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
    
    private List<Player> playerController = new List<Player>();
    
    private delegate void DisableUI();
    private DisableUI toggleDelegate;
    
    void Start()
    {
        toggleDelegate += ToggleAllUI;
        toggleDelegate.Invoke();
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
