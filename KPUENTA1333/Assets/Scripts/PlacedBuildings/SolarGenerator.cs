
using UnityEngine;

public class SolarGenerator : PlacedBuildingBase
{
    [SerializeField] private float PowerCycleTime = 1f;
    [SerializeField] private float PowerPerCycle = 10f;
    [SerializeField] private int AreaDamagePerTick = 1;
    // todo range value, default to 1
    private float _generatorTimer = 0f;

    public override void OnPlaced()
    {
        // Set the AOE damage to surrounding cells
        Manager.AddBuildingCellEffect(transform.position, 1, AreaDamagePerTick);
    }

    public override void OnRemoved()
    {
        // Remove the AOE damage from surrounding cells
        Manager.RemoveBuildingCellEffect(transform.position, 1, AreaDamagePerTick);
    }

    protected override void Tick()
    {
        _generatorTimer += Time.deltaTime;

        if (_generatorTimer >= PowerCycleTime)
        {
            _generatorTimer = 0f;
            AddPowerToPowerStorage();
        }
    }

    private void AddPowerToPowerStorage()
    {
        Owner.ResourceGain(PowerPerCycle);
    }
}
