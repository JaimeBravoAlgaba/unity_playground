using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class OrientationHUD : MonoBehaviour
{
    public Transform player; // Assign your player or aircraft transform
    public TextMeshProUGUI hudText;     // Assign the UI Text element
    private Vector3 lastPosition;
    private float speed;

    void Start()
    {
        lastPosition = player.position;
    }

    void FixedUpdate()
    {
        Vector3 displacement = player.position - lastPosition;
        speed = displacement.magnitude / Time.deltaTime;
        lastPosition = player.position;
    }

    void Update()
    {
        Vector3 euler = player.rotation.eulerAngles;

        // Normalize angles to -180 to 180
        float pitch = -NormalizeAngle(euler.x);
        float roll = NormalizeAngle(euler.z);

        hudText.text = $"Pitch: {pitch:F1}°\nRoll: {roll:F1}°\nSpeed: {speed:F1} m/s";
    }

    float NormalizeAngle(float angle)
    {
        if (angle > 180) angle -= 360;
        return angle;
    }
}
