using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

public class MainRoad : MonoBehaviour
{
    public static MainRoad Instance { get; private set; }
    public SplineComputer SplineComputer => _splineComputer;

    SplineComputer _splineComputer;

    private void Awake()
    {
        Instance = this;
        _splineComputer = GetComponent<SplineComputer>();
    }
}
