using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    Animator _animator;
    
    private int _count;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out FirstVehicle vehicle))
        {
            _animator.SetTrigger("Up");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out SecondVehicle vehicle))
        {
            _count++;

            if (VehicleGroup.SecondVehicleCount == _count)
            {
                _animator.SetTrigger("Down");
                _count = 0;
            }
        }
    }
}
