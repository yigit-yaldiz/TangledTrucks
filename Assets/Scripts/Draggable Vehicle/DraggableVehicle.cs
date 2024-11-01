using System.Collections;
using UnityEngine;
using System;

[DefaultExecutionOrder(1)]
public class DraggableVehicle : MonoBehaviour, ISnapable
{
    #region Actions
    public static Action<DraggableVehicle> OnSnap;
    public Action<DraggableVehicle> OnDrag;
    public Action<RopeHandler> OnCheck;
    #endregion

    #region Properties
    public static bool GetIsDragging => _isDragging;
    public bool GetAvailable => _available;
    public VehicleSnapPoint GetPreviousPoint => _previousPoint;
    #endregion

    VehicleSnapPoint _previousPoint;

    RopeHandler _myRope;
    VehicleSnapPoint _snapPoint;

    Vector3 _mousePosition;

    bool _available = true;
    
    static bool _isDragging;
    
    static float _maxDragSpeed = 20;
    
    float _firstZPosition;

    private void Awake()
    {
        _myRope = GetComponentInChildren<RopeHandler>();
    }

    private void OnEnable()
    {
        GameManager.OnPanelActivated += SetDraggableUnusable;
    }

    private void OnDisable()
    {
        GameManager.OnPanelActivated -= SetDraggableUnusable;
    }

    private void Start()
    {
        if (TryGetComponent<FirstVehicle>(out FirstVehicle truck))
        {
            _available = false;
        }

        OnSnap?.Invoke(this);
        
        _firstZPosition = transform.position.z;
    }

    private void OnMouseDown()
    {
        if (!_available) return;

        _mousePosition = Input.mousePosition - GetMousePosition();
    }

    private void OnMouseDrag()
    {
        if (!_available) return;

        Drag();
    }

    private void OnMouseUp()
    {
        if (GameManager.NumberOfMoves > 0 && _available)
        {
            Snap();
        }
        else
        {
            if (!_available) return;

            Debug.LogWarning("No more moves left");
            transform.position = _previousPoint.transform.position;
        }
    }

    Vector3 GetMousePosition()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }

    public void Snap()
    {
        _isDragging = false;
        
        OnSnap?.Invoke(this);

        if (GetComponentInParent<TruckGroupQueue>().IsOnLevelDesign) return;

        OnCheck?.Invoke(_myRope);
    }

    public void Drag()
    {
        _isDragging = true;

        Vector3 position = Input.mousePosition - _mousePosition;
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(position);

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, _maxDragSpeed * Time.deltaTime);

        OnDrag?.Invoke(this);

        ClampVehicleDistance(height: 1.25f);
    }

    public void SetPreviousPoint(VehicleSnapPoint snapPoint)
    {
        _previousPoint = snapPoint;
    }

    public void SetDraggableUnusable()
    {
        _available = false;
    }

    public void SetSnappedPoint(VehicleSnapPoint snapPoint)
    {
        _snapPoint = snapPoint;
    }

    public void MakeUnoccupiedThePoint()
    {
        if (_snapPoint != null)
        {
            _snapPoint.MakeUnoccupied();
        }
    }

    void ClampVehicleDistance(float height)
    {
        Vector3 position = transform.position;

        float maxLeft = GameArea.Instance.GameAreaBox.bounds.min.x;
        float maxRight = GameArea.Instance.GameAreaBox.bounds.max.x;

        position.x = Mathf.Clamp(position.x, maxLeft, maxRight);
        position.y = height;
        position.z = _firstZPosition;
        
        transform.position = position;
    }
}
