using System.Collections.Generic;
using UnityEngine;

[Tooltip("This class must be assign on TruckGroups parent (obi solver)")]
public class TruckGroupQueue : MonoBehaviour
{
    public static TruckGroupQueue Instance { get; private set; }
    public bool IsOnLevelDesign => _isOnLevelDesign;

    public bool IsReadyGroupEmpty => _readyGroups.Count == 0;

    [SerializeField] bool _isOnLevelDesign;

    private Queue<VehicleGroup> _readyGroups = new();
 
    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        QueueTrigger.OnRoadCheck += GetTruckGroup;
    }

    private void OnDisable()
    {
        QueueTrigger.OnRoadCheck -= GetTruckGroup;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (_isOnLevelDesign)
            {
                _isOnLevelDesign = false;
                Debug.LogWarning("Level Design Deactivated");
            }
            else if (!_isOnLevelDesign)
            {
                _isOnLevelDesign = true;
                Debug.LogWarning("Level Design Activated");
            }
        }
    }

    void GetTruckGroup()
    {
        if (_readyGroups.Count == 0) return;

        bool allAreTrue = true;

        RopeHandler[] ropes = _readyGroups.Peek().GetComponentsInChildren<RopeHandler>();

        foreach (RopeHandler rope in ropes)
        {
            if (rope.IsCollidingForaWhile)
            {
                allAreTrue = false;
                break;
            }
        }

        if (allAreTrue)
        {
            _readyGroups.Dequeue().CheckRoad(); 
        }
        else
        {
            _readyGroups.Dequeue();

            if (_readyGroups.Count == 0) return;

            if (_readyGroups.Peek() != null)
            {
                _readyGroups.Dequeue().CheckRoad();
            }
        }
    }

    public void EnqueueGroup(VehicleGroup group)
    {
        if (!_readyGroups.Contains(group) && !group.DidItMove)
        {
            _readyGroups.Enqueue(group);
        }
    }

    public void DequeueLastGroup()
    {
        if (_readyGroups.Count == 0) return;

        _readyGroups.Dequeue();
    }

    public bool CheckContainsThis(VehicleGroup group)
    {
        if (_readyGroups.Contains(group))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
