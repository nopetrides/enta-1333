
public class Player
{
    private int _playerIndex;
    private int _storedPower;
    private PlayerBuildingManager _buildingManager;

    public PlayerBuildingManager BuildingManager => _buildingManager;

    public Player(int playerIndex)
    {
        _playerIndex = playerIndex;
        _storedPower = 0;

        _buildingManager = new PlayerBuildingManager(this);
    }

}
