using UnityEngine;
using UnityEngine.InputSystem;

public class RobotDrive : MonoBehaviour
{
    public WheelCollider[] wheelColliders; // FL, FR, RL, RR
    public Transform[] wheelVisuals; // Assign in Inspector: FL, FR, RL, RR
    public float motorTorque = 1.13f; // Lynxmotion A4WD3 max torque per wheel (Nm)
    public float maxWheelRpm = 170f; // A4WD3 spec
    public CameraToggle cameraToggle; // Assign in inspector

    private Vector2 moveInput;

    // Called by Input System (bind to Move action)
    // Try this alternative signature for Send Messages
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        Debug.Log("Move input: " + moveInput);
    }

    void FixedUpdate()
    {
        float forward = moveInput.y;
        float turn = -moveInput.x; // Invert turning direction

        float left = Mathf.Clamp(forward - turn, -1f, 1f);
        float right = Mathf.Clamp(forward + turn, -1f, 1f);

        // If only turning (no forward/backward), spin in place
        if (Mathf.Abs(forward) < 0.01f && Mathf.Abs(turn) > 0.01f)
        {
            left = -turn;
            right = turn;
        }

        // Apply torque to each wheel
        wheelColliders[0].motorTorque = left * motorTorque;  // Front Left
        wheelColliders[2].motorTorque = left * motorTorque;  // Rear Left
        wheelColliders[1].motorTorque = right * motorTorque; // Front Right
        wheelColliders[3].motorTorque = right * motorTorque; // Rear Right

        // Clamp wheel speed to max RPM
        for (int i = 0; i < wheelColliders.Length; i++)
        {
            if (Mathf.Abs(wheelColliders[i].rpm) > maxWheelRpm)
            {
                wheelColliders[i].motorTorque = 0f;
            }
        }

        // Update visual wheels to match colliders (only apply x-axis rotation)
        for (int i = 0; i < wheelColliders.Length; i++)
        {
            Vector3 pos;
            Quaternion rot;
            wheelColliders[i].GetWorldPose(out pos, out rot);
            if (wheelVisuals != null && wheelVisuals.Length > i && wheelVisuals[i] != null)
            {
                // Only apply the x-axis rotation to the visual wheel
                Vector3 euler = wheelVisuals[i].localEulerAngles;
                float wheelRotX = rot.eulerAngles.x;
                wheelVisuals[i].localEulerAngles = new Vector3(wheelRotX, euler.y, euler.z);
            }
        }

        float speed = GetComponent<Rigidbody>().linearVelocity.magnitude;
        float avgRpm = 0f;
        foreach (var wheel in wheelColliders)
            avgRpm += Mathf.Abs(wheel.rpm);
        avgRpm /= wheelColliders.Length;

        //Debug.Log($"Robot speed (m/s): {speed:F2} | Avg wheel RPM: {avgRpm:F1}");
    }

    void Start()
    {
        // Set the radius of each wheel collider
        foreach (WheelCollider wheelCollider in wheelColliders)
        {
            wheelCollider.radius = 0.08f; // 60mm radius
        }
    }

    public void OnToggleCamera()
    {
        if (cameraToggle != null)
            cameraToggle.ToggleCamera();
    }
}
