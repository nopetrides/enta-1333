
using System;
using UnityEngine;

public class SolarGenerator : PlacedBuildingBase
{
    [SerializeField] private float PowerCycleTime = 1f;
    [SerializeField] private float PowerPerCycle = 10f;
    private float generatorTimer = 0f;
    
    protected override void Tick()
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
        _owner.ResourceGain(PowerPerCycle);
    }
}
