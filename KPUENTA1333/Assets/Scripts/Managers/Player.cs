
using System;

public class Player
{
    private int _playerIndex;
    private float _storedPower;
    private PlayerBuildingManager _buildingManager;

    public Action<float> OnPowerChanged;

    public PlayerBuildingManager BuildingManager => _buildingManager;

    public Player(int playerIndex)
    {
        _playerIndex = playerIndex;
        _storedPower = 0;

        _buildingManager = new PlayerBuildingManager(this);
    }

    public void ResourceGain(float gain)
    {
        _storedPower += gain;
        OnPowerChanged?.Invoke(_storedPower);
    }

}
