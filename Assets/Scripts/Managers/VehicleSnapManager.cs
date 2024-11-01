using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSnapManager : MonoBehaviour
{
    [SerializeField] List<VehicleSnapPoint> _snapPoints = new();
    [SerializeField] float _snapRange = 0.25f;

    #region Singleton Pattern
    public static VehicleSnapManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    private void OnEnable()
    {
        DraggableVehicle.OnSnap += OnDragEnded;
    }

    private void OnDisable()
    {
        DraggableVehicle.OnSnap -= OnDragEnded;
    }

    void OnDragEnded(DraggableVehicle vehicle)
    {
        if (vehicle is not ISnapable || !vehicle.GetAvailable) return;

        float closestDistance = -1;
        Transform closestSnapPoint = null;
        VehicleSnapPoint currentPoint;

        foreach (VehicleSnapPoint snapPoint in _snapPoints)
        {
            float currentDistance = Vector3.Distance(vehicle.transform.position, snapPoint.transform.position);

            if (closestSnapPoint == null || currentDistance < closestDistance)
            {
                closestDistance = currentDistance;
                closestSnapPoint = snapPoint.transform;
            }
        }

        currentPoint = closestSnapPoint.GetComponent<VehicleSnapPoint>();

        if (currentPoint != null && closestDistance <= _snapRange && !currentPoint.IsOccupied)
        {
            if (vehicle.transform.position != currentPoint.transform.position)
            {
                GameManager.Instance.DecreaseMoveCount(decreaseCount: 1);
            }

            vehicle.transform.position = closestSnapPoint.position;

            vehicle.SetSnappedPoint(currentPoint);

            currentPoint.MakeOccupied();

            if (vehicle.GetPreviousPoint != null && vehicle.GetPreviousPoint.IsOccupied)
            {
                vehicle.GetPreviousPoint.MakeUnoccupied();
            }

            vehicle.SetPreviousPoint(currentPoint);
        }
        else
        {
            vehicle.transform.position = vehicle.GetPreviousPoint.transform.position;
        }
    }

    public void AddSnapPoint(VehicleSnapPoint snapPoint)
    {
        _snapPoints.Add(snapPoint);
    }
}
