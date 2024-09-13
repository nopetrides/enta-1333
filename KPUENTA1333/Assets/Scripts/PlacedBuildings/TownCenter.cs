
using UnityEngine;

public class TownCenter : PlacedBuildingBase
{
    [SerializeField] private GameObject UnitToSpawn;
    [SerializeField] private Transform PointToSpawnAt;

    public void SpawnNewUnit()
    {
        Instantiate(UnitToSpawn, PointToSpawnAt);
    }
}