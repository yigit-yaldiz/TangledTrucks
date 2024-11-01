using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

[DefaultExecutionOrder(1)]
public class VehicleGroup : MonoBehaviour
{
    public List<Transform> VehicleTransforms { get => _vehicleTransforms; set => _vehicleTransforms = value; }
    public bool DidItMove => _didItMove;

    public static int VehicleGroupCount;
    public static int SecondVehicleCount;

    [SerializeField] List<Transform> _vehicleTransforms = new();
    
    [SerializeField] AnimationCurve _jumpCurve;
    [SerializeField] AnimationCurve _scaleCurve;
    [SerializeField] AnimationCurve _rotationCurve;

    const float _gettingCloserDuration = 0.2f;
    
    bool _didItMove;

    FirstVehicle _truck;
    RoadManager _roadManager;
    
    SecondVehicle[] _trailers;
    DraggableVehicle[] _draggables;

    Coroutine _coroutine;

    ParticleSystem[] _smokeParticles;

    private void Awake()
    {
        _draggables = GetComponentsInChildren<DraggableVehicle>();

        _truck = GetComponentInChildren<FirstVehicle>();
        _trailers = GetComponentsInChildren<SecondVehicle>();

        _roadManager = GetComponent<RoadManager>();

        SecondVehicleCount = _trailers.Length;

        _smokeParticles = GetComponentsInChildren<ParticleSystem>();
    }

    private void OnEnable()
    {
        foreach (DraggableVehicle draggable in _draggables)
        {
            draggable.OnCheck += StartCheck;
        }

        RopeHandler.OnCollisionEnd += StartCollisionCheck;

        GameManager.OnPanelActivated += StopVehicle;
    }

    private void OnDisable()
    {
        foreach (DraggableVehicle draggable in _draggables)
        {
            draggable.OnCheck -= StartCheck;
        }

        RopeHandler.OnCollisionEnd -= StartCollisionCheck;

        GameManager.OnPanelActivated -= StopVehicle;
    }

    private void Start()
    {
        GameManager.Instance.IncreaseTruckGroupCount();
    }

    void StartCollisionCheck(RopeHandler rope)
    {
        if (GetComponentInParent<TruckGroupQueue>().IsOnLevelDesign) return;

        StartCheck(rope);
    }

    void StartCheck(RopeHandler rope)
    {
        if (DraggableVehicle.GetIsDragging)
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }

            return;
        }

        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        _coroutine = StartCoroutine(DelayCoroutine(0.5f));

        IEnumerator DelayCoroutine(float delay)
        {
            yield return new WaitForSeconds(delay);

            bool allRopesAreTrue = true;

            RopeHandler[] ropes = GetComponentsInChildren<RopeHandler>();

            foreach (RopeHandler ropeHandler in ropes)
            {
                if (ropeHandler.IsCollidingForaWhile)
                {
                    allRopesAreTrue = false;
                    break;
                }
            }

            if (!allRopesAreTrue || _didItMove)
            {
                yield break;
            }

            if (QueueTrigger.CanAnyVehicleStart)
            {
                GameManager.Instance.IncreaseReleasedGroupCount();

                StartTheGroup();
            }
            else
            {
                if (!TruckGroupQueue.Instance.CheckContainsThis(this))
                {
                    GameManager.Instance.IncreaseReleasedGroupCount();
                    TruckGroupQueue.Instance.EnqueueGroup(this);
                }
            }
        }
    }

    public void CheckRoad()
    {
        if (!_didItMove && QueueTrigger.CanAnyVehicleStart)
        {
            StartTheGroup();
        }
    }

    void StartTheGroup()
    {
        float startAnimDuration = 0.3f;

        SetTheDraggablesDisable();
        ClearTheSnapPoints();

        _truck.transform.DOJump(_truck.transform.position, 1, 1, startAnimDuration).SetEase(_jumpCurve);
        _truck.transform.DOScale(_truck.transform.localScale.x * 1.2f, startAnimDuration).SetEase(_scaleCurve);

        for (int i = 0; i < _trailers.Length; i++)
        {
            SecondVehicle trailer = _trailers[i];

            trailer.transform.DOJump(trailer.transform.position, 1, 1, startAnimDuration).SetEase(_jumpCurve);
            trailer.transform.DOScale(trailer.transform.localScale.x * 1.2f, startAnimDuration).SetEase(_scaleCurve);
            trailer.transform.DOLookAt(i == 0 ? _truck.transform.position : _trailers[i - 1].transform.position, startAnimDuration).SetEase(_rotationCurve);
        }

        StartCoroutine(Delay());

        IEnumerator Delay()
        {
            QueueTrigger.CanAnyVehicleStart = false;
            _didItMove = true;

            yield return new WaitForSeconds(startAnimDuration);
            _truck.transform.rotation = Quaternion.Euler(Vector3.up * _truck.transform.rotation.eulerAngles.y);

            for (int i = 0; i < _trailers.Length; i++)
            {
                SecondVehicle trailer = _trailers[i];
                trailer.transform.rotation = Quaternion.Euler(Vector3.up * trailer.transform.rotation.eulerAngles.y);
            }
            yield return new WaitForSeconds(startAnimDuration * 0.3f);

            #region Get Second Vehicles Closer
            foreach (SecondVehicle trailer in _trailers)
            {
                trailer.GetCloser(_gettingCloserDuration);
            }

            yield return new WaitForSeconds(_gettingCloserDuration);
            #endregion

            _roadManager.DrawVehicleRoad();
            _roadManager.DrawMainRoad();

            _truck.VehicleStart();

            foreach (SecondVehicle trailer in _trailers)
            {
                trailer.VehicleStart();
            }

            VirtualActionsOnStart();
        }
    }

    void SetTheDraggablesDisable()
    {
        foreach (DraggableVehicle draggable in _draggables)
        {
            draggable.SetDraggableUnusable();
        }
    }

    public void DisableTruckGroup()
    {
        StartCoroutine(Delay(2));

        IEnumerator Delay(float time)
        {
            yield return new WaitForSeconds(time);
            gameObject.SetActive(false);
        }
    }

    void ClearTheSnapPoints()
    {
        foreach (DraggableVehicle draggable in _draggables)
        {
            draggable.MakeUnoccupiedThePoint();
        }
    }

    void VirtualActionsOnStart()
    {
        foreach (ParticleSystem smoke in _smokeParticles)
        {
            smoke.Play();
        }
    }

    void StopVehicle()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
    }
}
