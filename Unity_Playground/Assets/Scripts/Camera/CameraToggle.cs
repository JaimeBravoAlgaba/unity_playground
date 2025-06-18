using UnityEngine;

public class CameraToggle : MonoBehaviour
{
    public Camera camera1;
    public Camera camera2;

    // Method name must match the action name exactly, no parameters
    public void ToggleCamera()
    {
        Debug.Log("Toggle Camera called");
        bool cam1Active = camera1.enabled;
        camera1.enabled = !cam1Active;
        camera2.enabled = cam1Active;
    }

    void Start()
    {
        camera1.enabled = true;
        camera2.enabled = false;
    }
}
