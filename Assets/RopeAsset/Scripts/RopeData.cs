using Obi;
using System.Collections.Generic;
using UnityEngine;

public class RopeData : ScriptableObject
{
    public int[] Particles => particles;
    public List<Vector3> Positions => positions;
    public List<Vector3> AttachmentPositions => attachmentPositions;

    [SerializeField] private int[] particles;
    [SerializeField] private List<Vector3> positions;
    [SerializeField] private List<Vector3> attachmentPositions;

    public void Init(RopeHandler rope)
    {
        particles = rope.Actor.solverIndices;

        positions = new List<Vector3>();
        attachmentPositions = new List<Vector3>();

        for(int i = 0; i < rope.Attachments.Length; i++)
        {
            attachmentPositions.Add(rope.Attachments[i].target.position);
        }

        for (int i = 0; i < rope.Actor.solverIndices.Length; ++i)
        {
            int solverIndex = rope.Actor.solverIndices[i];
            var pos = rope.Actor.solver.positions[solverIndex];
            positions.Add(pos);
        }
    }
}
