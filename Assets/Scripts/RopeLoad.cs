using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeLoad
{
    public static RopeData Load(RopeHandler rope)
    {
        RopeData data = Resources.Load(RopeSettings.Instance.GetPath(rope.Id)) as RopeData;

        if (data == null)
        {
            Debug.LogWarning(RopeSettings.Instance.GetPath(rope.Id) + " not found.");
            return null;
        }

        for (int j = 0; j < data.AttachmentPositions.Count; j++)
        {
            rope.Attachments[j].target.position = data.AttachmentPositions[j];
        }

        for (int j = 0; j < data.Particles.Length; j++)
        {
            int solverIndex = rope.Actor.solverIndices[j];
            rope.Actor.solver.positions[solverIndex] = data.Positions[j];
            rope.Actor.solver.velocities[solverIndex] = Vector4.zero;
        }

        return data;
    }
}
