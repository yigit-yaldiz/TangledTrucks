using System;
using System.Collections;
using UnityEngine;
using Dreamteck.Splines;

public class Trigger : MonoBehaviour
{
    public SplineComputer SplineComputer => _splineComputer;

    SplineComputer _splineComputer;

    private void Awake()
    {
        _splineComputer = GetComponent<SplineComputer>();
    }

    public float GetMainRoadConnectPercent()
    {
        SplineProjector projector = GetComponentInChildren<SplineProjector>();
        return (float) projector.GetPercent();
    }
}
