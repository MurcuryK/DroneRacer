using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {

    // movement/location of drone
    private Transform Drone;

    // Speed of camera
    private Vector3 VelocityCam;

    // Location befind the Dronw
    public Vector3 BehindPosition = new Vector3(0, 8, -10);
    public float Angle = 14;

    void Awake()
    {
        // Finding the Drones location
        Drone = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        // Change camera position slowly from current position to the drones position + the distances over time of 0.1 
        transform.position = Vector3.SmoothDamp(transform.position, Drone.transform.TransformPoint(BehindPosition) + Vector3.up * Input.GetAxis("Vertical"), ref VelocityCam, 0.1f);

        // Change the rotation of the camera based on the drones current Y rotation
        transform.rotation = Quaternion.Euler(new Vector3(Angle, Drone.GetComponent<DroneMovement>().CurrentYRo, 0));
    }
}
