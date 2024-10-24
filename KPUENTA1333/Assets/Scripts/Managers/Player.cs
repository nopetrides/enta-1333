
using System;

public class Player
{
    private int _playerIndex;
    private int _playerFaction;
    public int PlayerFaction => _playerFaction;

    private float _storedPower;
    private PlayerBuildingManager _buildingManager;

    public Action<float> OnPowerChanged;

    public PlayerBuildingManager BuildingManager => _buildingManager;

    public Player(int playerIndex, int playerFaction, GameManager gameManager)
    {
        _playerIndex = playerIndex;
        _playerFaction = playerFaction;
        _storedPower = 0;

        _buildingManager = new PlayerBuildingManager(this, gameManager);
    }

    public void ResourceGain(float gain)
    {
        _storedPower += gain;
        OnPowerChanged?.Invoke(_storedPower);
    }

}
