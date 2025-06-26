using UnityEngine;
using UnityEngine.InputSystem;

public class RobotControls : MonoBehaviour
{
    [Header("Camera Controls")]
    public CameraToggle cameraToggle; // Assign in inspector

    [Header("Wheel Joints")]
    public ArticulationBody frontLeftWheel;
    public ArticulationBody frontRightWheel;
    public ArticulationBody rearLeftWheel;
    public ArticulationBody rearRightWheel;

    [Header("Drive Settings")]
    public float driveForce = 57.2f;
    public float throttleMutliplier = 16f;
    public float turnFactor = 5f;
    public float wheelRadius = 0.079f; // Adjust to match your wheel size

    private ArticulationBody rootBody;
    private float throttle;
    private float turn;

    void Start()
    {
        rootBody = GetComponent<ArticulationBody>();
    }

    public void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        throttle = input.y;
        turn = input.x;
    }

void Update()
{
    float leftSpeed;
    float rightSpeed;

    if (Mathf.Abs(throttle) > 0.01f)
    {
        // Moving forward/backward with turning
        leftSpeed = (throttle * throttleMutliplier + turn * turnFactor) * driveForce;
        rightSpeed = (throttle * throttleMutliplier - turn * turnFactor) * driveForce;
    }
    else
    {
        // In-place rotation
        leftSpeed = +turn * turnFactor * driveForce;
        rightSpeed = -turn * turnFactor * driveForce;
    }

    ApplyWheelVelocity(frontLeftWheel, leftSpeed);
    ApplyWheelVelocity(rearLeftWheel, leftSpeed);
    ApplyWheelVelocity(frontRightWheel, rightSpeed);
    ApplyWheelVelocity(rearRightWheel, rightSpeed);
}


    void ApplyWheelVelocity(ArticulationBody wheel, float velocity)
    {
        var drive = wheel.xDrive;
        drive.stiffness = 0f;
        drive.damping = 10f;
        drive.forceLimit = 10000;
        drive.targetVelocity = velocity;
        wheel.xDrive = drive;
    }

    public void OnToggleCamera()
    {
        if (cameraToggle != null)
            cameraToggle.ToggleCamera();
    }

    public void OnResetOrientation()
        {
            Vector3 currentPos = rootBody.transform.position;
            float yaw = rootBody.transform.rotation.eulerAngles.y;
            Vector3 newPos = new Vector3(currentPos.x, currentPos.y + 1.0f, currentPos.z);
            Quaternion newRot = Quaternion.Euler(0, yaw, 0);

            rootBody.TeleportRoot(newPos, newRot);
        }
}
