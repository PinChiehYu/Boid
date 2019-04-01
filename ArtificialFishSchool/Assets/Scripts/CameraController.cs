using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float xSpeed, ySpeed, scrollSpeed;
    private float x, y;

    private float originDistance;
    private float distance;

    private bool isFollowing;
    private Transform followingFish;

    void Start()
    {
        x = transform.rotation.eulerAngles.y;
        y = transform.rotation.eulerAngles.x;
        distance = Vector3.Distance(Vector3.zero, transform.position);
        originDistance = distance;
    }

    void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (hit.transform.tag == "Fish")
                {
                    isFollowing = true;
                    followingFish = hit.transform;
                    distance = originDistance / 5;
                }
                else
                {
                    isFollowing = false;
                    distance = originDistance;
                }
            }
        }

        if (Input.GetMouseButton(1))
        {
            x += Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;
            y -= Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;

            x %= 360;
        }

        distance -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed * Time.deltaTime;
        distance = Mathf.Clamp(distance, originDistance / 3, originDistance);

        Quaternion rotation = Quaternion.Euler(y, x, 0);
        Vector3 target = isFollowing ? followingFish.position : Vector3.zero;


        transform.rotation = rotation;
        transform.position = Vector3.Lerp(transform.position, rotation * new Vector3(0, 0, -distance) + target, 0.3f);
    }
}
