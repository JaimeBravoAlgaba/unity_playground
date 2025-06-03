using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Sensor;
using RosMessageTypes.Std;
using RosMessageTypes.BuiltinInterfaces;
using System;

public class RosDualImagePublisher : MonoBehaviour
{
    public string rawImageTopic = "/camera/image_raw";
    public string compressedImageTopic = "/camera/image_compressed";
    public Camera targetCamera;
    public int imageWidth = 640;
    public int imageHeight = 480;
    public float publishRate = 0.5f;

    private ROSConnection ros;
    private float timeElapsed;

    private RenderTexture renderTexture;
    private Texture2D texture2D;

    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<ImageMsg>(rawImageTopic);
        ros.RegisterPublisher<CompressedImageMsg>(compressedImageTopic);

        renderTexture = new RenderTexture(imageWidth, imageHeight, 24);
        texture2D = new Texture2D(imageWidth, imageHeight, TextureFormat.RGB24, false);
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
        targetCamera.targetTexture = renderTexture;
        targetCamera.Render();

        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(new Rect(0, 0, imageWidth, imageHeight), 0, 0);
        texture2D.Apply();
        targetCamera.targetTexture = null;
        RenderTexture.active = null;

        FlipTextureVertically(texture2D); // Flip the texture vertically

        byte[] rawData = texture2D.GetRawTextureData();
        byte[] jpegData = texture2D.EncodeToJPG();

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

        CompressedImageMsg compressedImageMsg = new CompressedImageMsg
        {
            header = header,
            format = "jpeg",
            data = jpegData
        };

        ros.Publish(rawImageTopic, rawImageMsg);
        ros.Publish(compressedImageTopic, compressedImageMsg);
    }

    // Add this method to your class
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
