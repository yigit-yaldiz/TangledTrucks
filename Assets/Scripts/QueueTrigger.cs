using System;
using System.Collections.Generic;
using UnityEngine;

public class QueueTrigger : MonoBehaviour
{
    public static Action OnRoadCheck;

    public static bool CanAnyVehicleStart = true;

    private int _count;

    private void Start()
    {
        CanAnyVehicleStart = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out SecondVehicle vehicle))
        {
            _count++;

            if (VehicleGroup.SecondVehicleCount == _count)
            {
                _count = 0;
                CanAnyVehicleStart = true;
                OnRoadCheck?.Invoke();
                GameManager.Instance.CompletedGroupCount++;
            }
        }
    }
}
