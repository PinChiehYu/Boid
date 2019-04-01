using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishState {

    public float Mass { get; private set; }
    public Vector3 Position { get; private set; }
    public Vector3 Velocity { get; private set; }
    public float MaxForce { get; private set; }
    public float MaxVelocity { get; private set; }
    public Quaternion Orientation { get; private set; }
    public float Starvation;

    List<BehaviorType> BehaviorList;
    Dictionary<BehaviorType, Task> TaskList;

    private GameObject fishPrefab;
    public Vector3 currentforce;

    public void Initialize(GameObject prefab)
    {
        float tanksize = TankManager.Instance.GetTankSize();
        Mass = Random.Range(1f, 3f);
        Position = new Vector3(Random.Range(-1 * tanksize, tanksize), Random.Range(-1 * tanksize, tanksize), Random.Range(-1 * tanksize, tanksize));
        Velocity = new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), Random.Range(-3f, 3f));
        MaxForce = Random.Range(50f, 100f);
        MaxVelocity = Random.Range(10f, 15f);
        Orientation = prefab.transform.rotation;

        fishPrefab = prefab;
        currentforce = Vector3.zero;
        Starvation = Random.Range(0f, 1f);

        UpdatePhysicsState();
    }

    public void UpdatePhysicsState()
    {
        Velocity += Vector3.ClampMagnitude(currentforce, MaxForce) / Mass * Time.deltaTime;
        Velocity = Vector3.ClampMagnitude(Velocity, MaxVelocity);
        Position += Velocity * Time.deltaTime;
        fishPrefab.transform.position = Position;
        fishPrefab.transform.rotation = Quaternion.Slerp(fishPrefab.transform.rotation, Quaternion.LookRotation(Velocity) * Orientation, MaxVelocity * Time.deltaTime);

        currentforce = Vector3.zero;

        Starvation -= Time.deltaTime * 0.01f;
        if (Starvation < 0) Starvation = 1f;

        Debug.DrawLine(Position, Position + Velocity);
        float tanksize = TankManager.Instance.GetTankSize();
    }

    public void AddForce(Vector3 force)
    {
        currentforce += force;
    }
}
