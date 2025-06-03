using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotVision : MonoBehaviour
{
    public GameObject robot; // The robot to follow
    public Vector3 localOffset = new Vector3(0, 0.15f, 0.2f); // Slightly above and forward from robot's origin

    [Header("Camera Angle (degrees)")]
    [Range(-90, 90)]
    public float pitch = 0f; // Up/down angle
    [Range(-180, 180)]
    public float yaw = 0f;   // Left/right angle

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Calculate rotation from pitch and yaw
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        // Offset in robot's local space, rotated by pitch/yaw
        Vector3 rotatedOffset = rotation * localOffset;

        // Position the camera relative to the robot's local space
        transform.position = robot.transform.TransformPoint(rotatedOffset);
        // Look in the robot's forward direction, adjusted by pitch/yaw
        transform.rotation = robot.transform.rotation * rotation;
    }
}
