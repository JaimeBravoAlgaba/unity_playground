using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowRobot : MonoBehaviour
{
    public GameObject robot; // The robot to follow
    public Vector3 localOffset = new Vector3(0, 2, -4); // Offset in robot's local space

    [Header("Camera Angle (degrees)")]
    [Range(-90, 90)]
    public float pitch = 20f; // Up/down angle
    [Range(-180, 180)]
    public float yaw = 0f;    // Left/right angle

    void LateUpdate()
    {
        // Calculate rotation from pitch and yaw
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        // Offset in robot's local space, rotated by pitch/yaw
        Vector3 rotatedOffset = rotation * localOffset;

        // Position the camera relative to the robot's local space
        transform.position = robot.transform.TransformPoint(rotatedOffset);
        transform.LookAt(robot.transform.position + Vector3.up * 0.7f); // Look at robot, slightly above its origin
    }
}
