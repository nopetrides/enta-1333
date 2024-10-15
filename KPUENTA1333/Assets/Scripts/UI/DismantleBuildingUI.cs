using UnityEngine;

public class DismantleBuildingUI : MonoBehaviour
{
    [SerializeField] private GameObject Parent;

    private PlacedBuildingBase _placedBuilding = null;

    public void Open(PlacedBuildingBase buildingToDestroy)
    {
        Parent.SetActive(true);
        _placedBuilding = buildingToDestroy;
    }

    private void Close()
    {
        _placedBuilding = null;
        Parent.SetActive(false);
    }

    public void ButtonCancel()
    {
        Close();
    }

    public void ButtonConfirm()
    {
        _placedBuilding.OnRemoved();
        // todo replace with returning to pool
        Destroy(_placedBuilding?.gameObject);

        Close();
    }
}
