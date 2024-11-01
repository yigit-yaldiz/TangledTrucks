using Obi;
using System;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ObiRope))]
public class RopeHandler : MonoBehaviour
{
    [SerializeField] private string uniqueId;
    public string Id => uniqueId;
    public static Action<RopeHandler> OnCollisionEnd;
    public bool IsColliding => RopeCollision.Instance.IsColliding(actor);

    public bool IsCollidingForaWhile { get; private set; } 

    public ObiActor Actor => actor;
    public ObiRope Rope => rope;
    public RopeData Data => data;
    public ObiParticleAttachment[] Attachments => attachments;

    private ObiActor actor;
    private ObiRope rope;
    private RopeData data;
    private ObiParticleAttachment[] attachments;

    float collisionTimer;
    bool waitForCollide = true;

    [ContextMenu("GenerateID")]
    private void GenerateID()
    {
        uniqueId = Guid.NewGuid().ToString();
    }

    private void Reset()
    {
        GenerateID();
    }

    private void Awake()
    {
        actor = GetComponent<ObiActor>();
        rope = GetComponent<ObiRope>();
        attachments = GetComponents<ObiParticleAttachment>();
    }

    private void Start()
    {
        data = RopeLoad.Load(this);
    }

    private void Update()
    {
        if (RopeCollision.Instance.IsColliding(actor))
        {
            collisionTimer = 0;
            waitForCollide = false;
            IsCollidingForaWhile = true;
        }
        else if (!waitForCollide)
        {
            collisionTimer += Time.deltaTime;
            if (collisionTimer > RopeSettings.Instance.CollisionEndDuration)
            {
                waitForCollide = true;
                IsCollidingForaWhile = false;
                StartCoroutine(Delay());
            }
        }

        IEnumerator Delay()
        {
            while (DraggableVehicle.GetIsDragging)
            {
                yield return null;
            }
            OnCollisionEnd?.Invoke(this);
        }
    }
}
