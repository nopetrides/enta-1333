
using System;
using UnityEngine;

public class SolarGenerator : PlacedBuildingBase
{
    [SerializeField] private float PowerCycleTime = 1f;
    [SerializeField] private float PowerPerCycle = 10f;
    private float generatorTimer = 0f;
    
    
    private void Update()
    {
        generatorTimer += Time.deltaTime;

        if (generatorTimer >= PowerCycleTime)
        {
            generatorTimer = 0f;
            AddPowerToPowerStorage();
        }
    }

    private void AddPowerToPowerStorage()
    {
        // todo
        // Add PowerPerCycle to player storage manager
    }
}
