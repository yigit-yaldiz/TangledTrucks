using Obi;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObiSolver))]
public class RopeCollision : MonoBehaviour
{
    public static RopeCollision Instance { get; private set; }

    public List<ObiActor> CollidingActors => collidingActors;

    [SerializeField] List<ObiActor> collidingActors = new List<ObiActor>();
    ObiSolver solver;

    float lastCheckTime = 0;

    void Awake()
    {
        Instance = this;

        solver = GetComponent<ObiSolver>();
    }

    void OnEnable()
    {
        solver.OnParticleCollision += OnRopeParticleCollision;
    }

    void OnDisable()
    {
        solver.OnParticleCollision -= OnRopeParticleCollision;
    }

    public bool IsColliding(ObiActor actor)
    {
        return collidingActors.Contains(actor);
    }

    private void OnRopeParticleCollision(object sender, ObiSolver.ObiCollisionEventArgs frame)
    {
        if (lastCheckTime > Time.time)
            return;

        lastCheckTime = Time.time + RopeSettings.Instance.CollisionCheckThreshold;

        CollidingActors.Clear();
        foreach (Oni.Contact contact in frame.contacts)
        {
            // if this one is an actual collision:
            if (contact.distance < RopeSettings.Instance.CollisionDistance)
            {
                int simplexStartA = solver.simplexCounts.GetSimplexStartAndSize(contact.bodyA, out int simplexSizeA);
                int simplexStartB = solver.simplexCounts.GetSimplexStartAndSize(contact.bodyB, out int simplexSizeB);

                int particleIndexA = solver.simplices[simplexStartA];
                int particleIndexB = solver.simplices[simplexStartB];

                ObiSolver.ParticleInActor pa1 = solver.particleToActor[particleIndexA];
                ObiSolver.ParticleInActor pa2 = solver.particleToActor[particleIndexB];

                if (!CollidingActors.Contains(pa1.actor))
                    CollidingActors.Add(pa1.actor);
                if (!CollidingActors.Contains(pa2.actor))
                    CollidingActors.Add(pa2.actor);
            }
        }
    }
}