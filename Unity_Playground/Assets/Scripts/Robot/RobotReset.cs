using UnityEngine;

public class RobotReset : MonoBehaviour
{
    public Transform robotTransform; // Assign your robot's main Transform in Inspector
    public float uprightThreshold = 0.5f; // Cosine of max allowed tilt angle (0.5 ≈ 60 degrees)
    public Vector3 resetPosition = Vector3.zero; // Where to reset the robot (optional)
    public bool useResetPosition = false; // Set true to also reset position

    // Update is called once per frame
    void Update()
    {
        // Check if robot is turtled (upside down or tipped over)
        if (Vector3.Dot(robotTransform.up, Vector3.up) < uprightThreshold)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                // Reset rotation to upright
                robotTransform.rotation = Quaternion.identity;
                // Optionally reset position
                if (useResetPosition)
                    robotTransform.position = resetPosition;
                // Also reset Rigidbody velocity if present
                Rigidbody rb = robotTransform.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }
            }
        }
    }
}
