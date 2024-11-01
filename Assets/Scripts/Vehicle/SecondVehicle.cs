using Obi;
using UnityEngine;
using Dreamteck.Splines;
using System.Collections;
using System;

public class SecondVehicle : MonoBehaviour
{
    public Transform FrontVehicle => _frontVehicle;

    const float _distanceBetweenFrontVehicle = 4.5f;

    SplineComputer _mainRoad;
    
    SplineComputer _secondVehicleRoad;

    SplineFollower _splineFollower;
    SplineProjector _splineProjector;

    Transform _frontVehicle;

    ObiRopeCursor _cursor;

    private void Awake()
    {
        _splineFollower = GetComponent<SplineFollower>();
        _splineProjector = GetComponent<SplineProjector>();

        _secondVehicleRoad = GetComponentInParent<SplineComputer>();

        //_mainRoad = _trailerRoad.GetComponentInParent<SplineComputer>();
        _mainRoad = transform.parent.parent.GetComponent<SplineComputer>();

        SetFrontVehicle();

        _splineFollower.followSpeed = GameManager.VehicleSpeed;
        _cursor = GetComponentInChildren<ObiRopeCursor>();
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

    public void VehicleStart()
    {
        if (_splineFollower.spline == _mainRoad && _splineProjector.spline == _mainRoad) return;

        SetTheRoad(_secondVehicleRoad);

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

    public void ChangeToMainRoad()
    {
        SetTheRoad(_mainRoad);
    }

    void SetFrontVehicle()
    {
        int mySiblingIndex = transform.GetSiblingIndex();
        VehicleGroup truckGroup = GetComponentInParent<VehicleGroup>();
        
        if (mySiblingIndex != 0)
        {
            _frontVehicle = transform.parent.GetChild(mySiblingIndex - 1);

            truckGroup.VehicleTransforms.Add(_frontVehicle);

            if (mySiblingIndex == transform.parent.childCount - 1)
            {
                truckGroup.VehicleTransforms.Add(transform);
            }
        }
    }

    public Transform GetFrontVehicle()
    {
        return _frontVehicle;
    }

    public void GetCloser(float duration)
    {
        StartCoroutine(SetDistances(duration));
    }

    IEnumerator SetDistances(float duration)
    {
        float waitTime = duration;
        float elapsedTime = 0;
        float percent = 0;

        Vector3 startPosition = transform.position;
        Vector3 targetPosition;
        Vector3 direction;
        float distance;

        float ropeLength = _cursor.GetComponent<ObiRope>().CalculateLength();

        _cursor.GetComponent<ObiRope>().plasticYield = 0;

        ObiParticleAttachment[] attachments = GetComponentsInChildren<ObiParticleAttachment>();

        //bool isFrontVehicleTruck = _frontVehicle.TryGetComponent(out FirstVehicle truck);

        while (elapsedTime < waitTime)
        {
            direction = (transform.position - _frontVehicle.position).normalized;

            targetPosition = direction * _distanceBetweenFrontVehicle + _frontVehicle.position; //distance between front

            //targetPosition = direction * (isFrontVehicleTruck ? 4.5f : 4f) + _frontVehicle.position;

            transform.position = Vector3.Lerp(startPosition, targetPosition, percent);
            
            distance = Vector3.Distance(attachments[0].target.position, attachments[1].target.position);

            _cursor.ChangeLength(Mathf.Lerp(ropeLength, distance / 5f, percent)); //rope length

            yield return null;

            percent = elapsedTime / waitTime;
            elapsedTime += Time.deltaTime;
        }
    }
}
