using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSnapPoint : MonoBehaviour
{
    public bool IsOccupied => _isOccupied;

    [SerializeField] bool _isOccupied;

    private void Start()
    {
        VehicleSnapManager.Instance.AddSnapPoint(this);
    }

    public void MakeOccupied()
    {
        _isOccupied = true;
    }

    public void MakeUnoccupied()
    {
        _isOccupied = false;
    }
}

