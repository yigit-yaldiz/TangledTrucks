using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

[DefaultExecutionOrder(1)]
public class RoadManager : MonoBehaviour
{
    VehicleGroup _truckGroup;

    SplineComputer _secondVehicleRoad;
    SplineComputer _mainRoad;
    
    Trigger _trigger;

    List<SplinePoint> _secondVehicleRoadSplinePoints = new();
    List<SplinePoint> _mainRoadSplinePoints = new();

    private void Awake()
    {
        _mainRoad = GetComponent<SplineComputer>();
        _truckGroup = GetComponent<VehicleGroup>();
        _trigger = GetComponentInChildren<Trigger>();

        _secondVehicleRoad = transform.GetChild(0).GetComponent<SplineComputer>();
    }

    public void DrawVehicleRoad()
    {
        _truckGroup.VehicleTransforms = _truckGroup.VehicleTransforms.OrderBy(t => t.GetSiblingIndex()).ToList();
        _truckGroup.VehicleTransforms.Reverse();

        foreach (Transform vehiclePoint in _truckGroup.VehicleTransforms)
        {
            SplinePoint splinePoint = new SplinePoint();
            splinePoint.position = vehiclePoint.position;
            _secondVehicleRoadSplinePoints.Add(splinePoint);
        }

        _secondVehicleRoad.SetPoints(_secondVehicleRoadSplinePoints.ToArray(), SplineComputer.Space.World);
        
        SplinePoint triggerFirstPoint = _trigger.SplineComputer.GetPoint(0);
        _secondVehicleRoad.SetPoint(_secondVehicleRoad.pointCount, triggerFirstPoint);
        
        _secondVehicleRoad.RebuildImmediate();
    }

    public void DrawMainRoad()
    {
        SplinePoint[] triggerPoints = _trigger.SplineComputer.GetPoints();
        _mainRoadSplinePoints.AddRange(triggerPoints);

        float mainRoadPercent = _trigger.GetMainRoadConnectPercent();
        SplineComputer mainRoadSplineComputer = MainRoad.Instance.SplineComputer;
        int intersectionIndex = mainRoadSplineComputer.PercentToPointIndex(mainRoadPercent) - 1;

        for (int i = intersectionIndex; i >= 0; i--)
        {
            SplinePoint splinePoint = new SplinePoint();
            splinePoint.position = mainRoadSplineComputer.GetPoint(i).position;
            _mainRoadSplinePoints.Add(splinePoint);
        }

        _mainRoad.SetPoints(_mainRoadSplinePoints.ToArray(), SplineComputer.Space.World);
        _mainRoad.RebuildImmediate();
    }
}
