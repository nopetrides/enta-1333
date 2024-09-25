using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _playerCount = 1;
    [SerializeField] private BuildingPlacementManager _placementManager;

    private List<Player> playerController = new List<Player>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < _playerCount; i++)
        {
            var player = new Player(i);
            playerController.Add(player);
            if (i == 0)
            {
                _placementManager.SetLocalBuildingManager(player.BuildingManager);
            }
        }
    }
}
