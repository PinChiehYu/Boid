using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Task
{
    private Vector3 targetPosition;
    Func<FishState, Vector3, Vector3> behaviorDelegate;

    /*
    private Vector3 ApplySteeringForce(Vector3 desiredvelocity, float speedratio = 1f)
    {
        desiredvelocity.Normalize();
        desiredvelocity *= fishstate.MaxSpeed * speedratio;
        Vector3 steering = desiredvelocity - fishstate.Velocity;
        return steering;
    }
    */
}

    