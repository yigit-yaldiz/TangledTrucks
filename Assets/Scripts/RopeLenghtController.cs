using System.Collections;
using UnityEngine;
using Obi;

[Tooltip("This class must be assigned to the rope")]
[DefaultExecutionOrder(1)]
public class RopeLenghtController : MonoBehaviour
{
    [SerializeField] float _maxDistance;
    
    RopeHandler _ropeHandler;
    ObiRope _rope;
    ObiParticleAttachment _attachment;
    
    DraggableVehicle _draggableVehicle;
    SecondVehicle _thisVehicle;

    Transform _frontVehicle;

    ObiRopeCursor _cursor;

    private void Awake()
    { 
        _ropeHandler = GetComponent<RopeHandler>();
        _rope = GetComponent<ObiRope>();
        _attachment = GetComponent<ObiParticleAttachment>();
        
        _draggableVehicle = GetComponentInParent<DraggableVehicle>();
        _thisVehicle = GetComponentInParent<SecondVehicle>();

        _cursor = GetComponent<ObiRopeCursor>();

        SetFrontAttachment();
    }

    private void Start()
    {
        _frontVehicle = _thisVehicle.FrontVehicle;
    }

    private void OnEnable()
    {
        _draggableVehicle.OnDrag += ClampDistanceBetweenVehicles;
    }

    private void OnDisable()
    {
        _draggableVehicle.OnDrag -= ClampDistanceBetweenVehicles;
    }

    private void ClampDistanceBetweenVehicles(DraggableVehicle vehicle)
    {
        Transform targetVehicle = vehicle.transform;
        Transform refVehicle = _frontVehicle;

        float distance = Vector3.Distance(targetVehicle.position, refVehicle.position);
        float smoothness = 9.25f;

        if (distance > _maxDistance)
        {
            Vector3 direction = (refVehicle.position - targetVehicle.position).normalized;

            Vector3 newPosition = targetVehicle.position;
            newPosition.x = Mathf.Lerp(targetVehicle.position.x, refVehicle.position.x - direction.x * _maxDistance, Time.deltaTime * smoothness);

            targetVehicle.position = newPosition;
        }

        vehicle.transform.position = targetVehicle.position;
    }

    public void SetRopeLenght(float lenght)
    {
        float distance = Vector3.Distance(_frontVehicle.position, _thisVehicle.transform.position);

        _rope.stretchingScale = lenght;
        _cursor.ChangeLength(_rope.restLength * 0.9f);
    }

    void SetFrontAttachment()
    {
        _attachment.target = _thisVehicle.GetFrontVehicle();
    }

    Vector3 GetParticlePositionAtPercent(float percent)
    {
        int[] indeces = _ropeHandler.Rope.solverIndices;
        int index = indeces[Mathf.RoundToInt((_ropeHandler.Rope.activeParticleCount - 1) * percent)];

        return _ropeHandler.Rope.GetParticlePosition(index);
    }
}
