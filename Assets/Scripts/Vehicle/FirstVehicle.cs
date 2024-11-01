using System.Collections;
using UnityEngine;
using Dreamteck.Splines;

public class FirstVehicle : MonoBehaviour
{
    SplineComputer _mainRoad;

    SplineFollower _splineFollower;
    SplineProjector _splineProjector;

    VehicleGroup _vehicleGroup;

    private void Awake()
    {
        _splineFollower = GetComponent<SplineFollower>();
        _splineProjector = GetComponent<SplineProjector>();

        _mainRoad = transform.parent.parent.GetComponent<SplineComputer>();

        _vehicleGroup = GetComponentInParent<VehicleGroup>();

        _splineFollower.followSpeed = GameManager.VehicleSpeed;
    }

    public void VehicleStart()
    {
        ChangeToMainRoad();

        StartCoroutine(Accelarate());

        IEnumerator Accelarate()
        {
            while (gameObject.activeInHierarchy)
            {
                yield return new WaitForSeconds(0.3f);
                _splineFollower.followSpeed += GameManager.VehicleSpeed * 0.05f;
            }
        }
    }

    void SetTheRoad(SplineComputer road)
    {
        _splineProjector.spline = road;
        _splineProjector.RebuildImmediate();

        _splineFollower.spline = road;
        _splineFollower.RebuildImmediate();

        float startPercent = (float)_splineProjector.GetPercent();

        _splineFollower.SetPercent(startPercent);
    }

    public void ChangeToMainRoad()
    {
        SetTheRoad(_mainRoad);
        //_splineFollower.direction = Spline.Direction.Backward;
    }

    public void DisableVehicleGroup()
    {
        _vehicleGroup.DisableTruckGroup();
    }
}
