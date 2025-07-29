using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Sensor;
using RosMessageTypes.Std;
using RosMessageTypes.BuiltinInterfaces;
using System;

public class RosDualImagePublisher : MonoBehaviour
{
    public string rawImageTopic = "/camera/image_raw";
    public string depthImageTopic = "/camera/depth_raw";
    public Camera targetCamera;
    public Camera depthCamera; // Assign a camera set to render depth
    public int imageWidth = 1280;
    public int imageHeight = 720;
    public float publishRate = 0.1f;

    private ROSConnection ros;
    private float timeElapsed;

    private RenderTexture renderTexture;
    private Texture2D texture2D;

    private RenderTexture depthRenderTexture;
    private Texture2D depthTexture2D;

    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<ImageMsg>(rawImageTopic);
        ros.RegisterPublisher<ImageMsg>(depthImageTopic);

        renderTexture = new RenderTexture(imageWidth, imageHeight, 24);
        texture2D = new Texture2D(imageWidth, imageHeight, TextureFormat.RGB24, false);

        depthRenderTexture = new RenderTexture(imageWidth, imageHeight, 24, RenderTextureFormat.Default);
        depthTexture2D = new Texture2D(imageWidth, imageHeight, TextureFormat.R8, false);
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed >= publishRate)
        {
            PublishBothImages();
            timeElapsed = 0f;
        }
    }

    void PublishBothImages()
    {
        // RGB image
        targetCamera.targetTexture = renderTexture;
        targetCamera.Render();

        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(new Rect(0, 0, imageWidth, imageHeight), 0, 0);
        texture2D.Apply();
        targetCamera.targetTexture = null;
        RenderTexture.active = null;

        FlipTextureVertically(texture2D);

        byte[] rawData = texture2D.GetRawTextureData();

        // Depth image
        depthCamera.targetTexture = depthRenderTexture;
        depthCamera.Render();

        RenderTexture.active = depthRenderTexture;
        depthTexture2D.ReadPixels(new Rect(0, 0, imageWidth, imageHeight), 0, 0);
        depthTexture2D.Apply();
        depthCamera.targetTexture = null;
        RenderTexture.active = null;

        FlipTextureVertically(depthTexture2D);

        byte[] depthData = depthTexture2D.GetRawTextureData();

        TimeMsg timestamp = new TimeMsg
        {
            sec = (int)Time.time,
            nanosec = (uint)((Time.time - (int)Time.time) * 1e9)
        };

        HeaderMsg header = new HeaderMsg
        {
            stamp = timestamp,
            frame_id = "camera_frame"
        };

        ImageMsg rawImageMsg = new ImageMsg
        {
            header = header,
            height = (uint)imageHeight,
            width = (uint)imageWidth,
            encoding = "rgb8",
            is_bigendian = 0,
            step = (uint)(imageWidth * 3),
            data = rawData
        };

        ImageMsg depthImageMsg = new ImageMsg
        {
            header = header,
            height = (uint)imageHeight,
            width = (uint)imageWidth,
            encoding = "mono8",
            is_bigendian = 0,
            step = (uint)(imageWidth),
            data = depthData
        };

        ros.Publish(rawImageTopic, rawImageMsg);
        ros.Publish(depthImageTopic, depthImageMsg);
    }

    void FlipTextureVertically(Texture2D texture)
    {
        int width = texture.width;
        int height = texture.height;
        Color[] pixels = texture.GetPixels();

        for (int y = 0; y < height / 2; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int top = y * width + x;
                int bottom = (height - y - 1) * width + x;
                Color temp = pixels[top];
                pixels[top] = pixels[bottom];
                pixels[bottom] = temp;
            }
        }
        texture.SetPixels(pixels);
        texture.Apply();
    }
}
