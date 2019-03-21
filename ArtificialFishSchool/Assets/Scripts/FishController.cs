using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishController : MonoBehaviour
{
    private FishState fishState;
    private int groupID;
    private Dictionary<BehaviorType, bool> enableBehaviorList;
    private List<int> preyIDList;
    private List<int> predatorIDList;

    private List<Task> taskList;

    public void Initialize(int id, Dictionary<BehaviorType, bool> enablebehavior, List<int> preyid, List<int> predatorid)
    {
        groupID = id;
        enableBehaviorList = enablebehavior;
        preyIDList = preyid;
        predatorIDList = predatorid;
        fishState = new FishState();
        fishState.Initialize(gameObject);
        taskList = new List<Task>();
    }

	public void Steering ()
    {
        List<GameObject> fishlist = TankManager.Instance.GetFishList();
        Vector3 leaveforce = Vector3.zero , averageposition = Vector3.zero, averagevelocity = Vector3.zero;
        float separationcounter = 0, cohesioncounter = 0, alignmentcounter = 0;
        foreach (GameObject fish in fishlist)
        {
            FishController fishcontroller = fish.GetComponent<FishController>();
            if (enableBehaviorList[BehaviorType.Separation])
            {
                if (fish != gameObject)
                {
                    float distance = Vector3.Distance(fishcontroller.GetPosition(), fishState.Position);
                    if (distance < TankManager.Instance.GetCrowdDistance())
                    {
                        Vector3 repulsiveforce = GetPosition() - fishcontroller.GetPosition();
                        repulsiveforce = repulsiveforce.normalized / distance;
                        leaveforce += repulsiveforce;
                        separationcounter++;
                    }
                }
            }
            if (enableBehaviorList[BehaviorType.Cohesion])
            {
                float distance = Vector3.Distance(fishcontroller.GetPosition(), GetPosition());
                if (fish != gameObject && fishcontroller.GetGroupID() == groupID && distance < TankManager.Instance.GetNeighborDistance())
                {
                    averageposition += fish.transform.position;
                    cohesioncounter++;
                }
            }
            if (enableBehaviorList[BehaviorType.Alignment])
            {
                float distance = Vector3.Distance(fishcontroller.GetPosition(), GetPosition());
                if (fish != gameObject && fishcontroller.GetGroupID() == groupID && distance < TankManager.Instance.GetNeighborDistance())
                {
                    averagevelocity += fishcontroller.GetVelocity();
                    alignmentcounter++;
                }
            }
            if (enableBehaviorList[BehaviorType.Evasion])
            {
                float distance = Vector3.Distance(fishcontroller.GetPosition(), GetPosition());
                if (fish != gameObject && predatorIDList.Contains(fishcontroller.GetGroupID()) && distance < TankManager.Instance.GetNeighborDistance())
                {
                    Vector3 predictposition = fishcontroller.GetPosition() + fishcontroller.GetVelocity() * distance / fishState.MaxVelocity;
                    Flee(predictposition);
                }
            }
        }
        if (separationcounter > 0)
        {
            ApplySteeringForce(leaveforce);
        }
        if (cohesioncounter > 0)
        {
            averageposition /= cohesioncounter;
            Seek(averageposition);
        }
        if (alignmentcounter > 0)
        {
            averagevelocity /= alignmentcounter;
            ApplySteeringForce(averagevelocity);
        }

        Containment();
        ObstacleAvoidance();
        Pursuit();
    }

    public void Locomotion()
    {
        fishState.UpdatePhysicsState();
    }

    private void Containment()
    {
        if (enableBehaviorList[BehaviorType.Containment])
        {
            Collider[] hitColliders = Physics.OverlapSphere(GetPosition(), TankManager.Instance.GetCrowdDistance(), LayerMask.GetMask("Wall"));
            foreach (Collider c in hitColliders)
            {
                Vector3 normalvector = c.gameObject.transform.rotation * Vector3.up;
                ApplySteeringForce(normalvector);
            }
        }
    }

    private void Pursuit()
    {
        if (enableBehaviorList[BehaviorType.Pursuit])
        {
            Collider[] hitColliders = Physics.OverlapSphere(GetPosition(), TankManager.Instance.GetNeighborDistance(), LayerMask.GetMask("Fish"));
            foreach (Collider coll in hitColliders)
            {
                FishController fishcontroller = coll.gameObject.GetComponent<FishController>();
                if (coll.gameObject != gameObject && preyIDList.Contains(fishcontroller.GetGroupID()))
                {
                    float distance = Vector3.Distance(fishcontroller.GetPosition(), GetPosition());
                    Vector3 predictposition = fishcontroller.GetPosition() + fishcontroller.GetVelocity() * distance / fishState.MaxVelocity;
                    Seek(predictposition);
                    break;
                }
            }
        }
    }

    private void Seek(Vector3 target, float speedration = 1f)
    {
        ApplySteeringForce(target - GetPosition(), speedration);
    }

    private void Flee(Vector3 target, float speedration = 1f)
    {
        ApplySteeringForce(fishState.Position - target, speedration);
    }

    private void Wander()
    {
        ApplySteeringForce(fishState.Velocity);
    }

    private void ObstacleAvoidance()
    {
        if (enableBehaviorList[BehaviorType.ObstacleAvoidance])
        {
            RaycastHit hit;
            if (Physics.SphereCast(GetPosition(), 1.5f, GetVelocity(), out hit, 10f, LayerMask.GetMask("Obstacle")))
            {
                Debug.DrawLine(GetPosition(), hit.point, Color.red);
                ApplySteeringForce(hit.normal);
            }
        }
    }

    private void ApplySteeringForce(Vector3 desiredvelocity, float speedratio = 1f)
    {
        desiredvelocity.Normalize();
        desiredvelocity *= fishState.MaxVelocity * speedratio;
        Vector3 steering = desiredvelocity - GetVelocity();
        fishState.AddForce(steering);
    }

    public Vector3 GetPosition()
    {
        return fishState.Position;
    }

    public Vector3 GetVelocity()
    {
        return fishState.Velocity;
    }

    public int GetGroupID()
    {
        return groupID;
    }
}
