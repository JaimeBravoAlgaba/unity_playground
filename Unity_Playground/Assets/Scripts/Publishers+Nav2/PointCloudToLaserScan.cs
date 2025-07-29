using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using RosMessageTypes.Sensor;
using RosMessageTypes.Std;
using RosMessageTypes.BuiltinInterfaces;
using System;
using System.Collections.Generic;

public class PointCloudToLaserScan : MonoBehaviour
{
    public string pointCloudTopic = "/raycast_lidar";
    public string laserScanTopic = "/scan_unity";
    public string frameId = "lidar_link";

    public int numLaserRays = 360;
    public float minRange = 0.1f;
    public float maxRange = 10f;
    public float minHeight = 1f;
    private float minVerticalAngleDeg;
    private float maxVerticalAngleDeg;

    public float lidarHeight = 0.15f; // Example: 15 cm above ground

    private ROSConnection ros;
    private static double rosUnityTimeOffset = 0.0;

    private List<Vector3> pointBuffer = new List<Vector3>();
    private float minSeenAngle = float.MaxValue;
    private float maxSeenAngle = float.MinValue;

    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.Subscribe<PointCloud2Msg>(pointCloudTopic, OnPointCloudReceived);
        ros.RegisterPublisher<LaserScanMsg>(laserScanTopic);

        // Compute vertical angle limits
        minVerticalAngleDeg = Mathf.Atan2(-lidarHeight, maxRange) * Mathf.Rad2Deg;
        maxVerticalAngleDeg = Mathf.Atan2(1.2f - lidarHeight, 1.5f) * Mathf.Rad2Deg;
    }

    void OnPointCloudReceived(PointCloud2Msg msg)
    {
        int pointStep = (int)msg.point_step;
        int pointCount = msg.data.Length / pointStep;

        for (int i = 0; i < pointCount; i++)
        {
            int offset = i * pointStep;
            float x = BitConverter.ToSingle(msg.data, offset + 0);
            float y = BitConverter.ToSingle(msg.data, offset + 4);
            float z = BitConverter.ToSingle(msg.data, offset + 8);

            float verticalAngle = Mathf.Atan2(z, Mathf.Sqrt(x * x + y * y)) * Mathf.Rad2Deg;

            minSeenAngle = Mathf.Min(minSeenAngle, verticalAngle);
            maxSeenAngle = Mathf.Max(maxSeenAngle, verticalAngle);

            pointBuffer.Add(new Vector3(x, y, z));
        }

        if (minSeenAngle <= minVerticalAngleDeg && maxSeenAngle >= maxVerticalAngleDeg)
        {
            ProcessBufferedPoints();
            pointBuffer.Clear();
            minSeenAngle = float.MaxValue;
            maxSeenAngle = float.MinValue;
        }
    }

    void ProcessBufferedPoints()
    {
        float[] ranges = new float[numLaserRays];
        for (int i = 0; i < numLaserRays; i++) ranges[i] = maxRange + 1f;

        foreach (Vector3 point in pointBuffer)
        {
            float x = point.x;
            float y = point.y;
            float z = point.z;
            
		// Ignore points higher than 1.3 meters
	    if (z > minHeight)
		continue;

            float verticalAngle = Mathf.Atan2(z, Mathf.Sqrt(x * x + y * y)) * Mathf.Rad2Deg;
            if (verticalAngle < minVerticalAngleDeg || verticalAngle > maxVerticalAngleDeg)
                continue;

            float range = Mathf.Sqrt(x * x + y * y);
            if (range < minRange || range > maxRange)
                continue;

            float horizontalAngle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
            if (horizontalAngle < 0) horizontalAngle += 360f;

            int bin = Mathf.FloorToInt(horizontalAngle / 360f * numLaserRays);
            if (bin < 0 || bin >= numLaserRays)
                continue;

            if (range < ranges[bin])
                ranges[bin] = range;
        }

        for (int i = 0; i < numLaserRays; i++)
        {
            if (ranges[i] > maxRange)
                ranges[i] = float.PositiveInfinity;
        }

        LaserScanMsg scan = new LaserScanMsg
        {
            header = new HeaderMsg
            {
                frame_id = frameId,
                stamp = GetTimeNow()
            },
            angle_min = 0f,
            angle_max = 2f * Mathf.PI,
            angle_increment = 2f * Mathf.PI / numLaserRays,
            time_increment = 0f,
            scan_time = 1f / 20f,
            range_min = minRange,
            range_max = maxRange,
            ranges = ranges,
            intensities = new float[numLaserRays]
        };

        ros.Publish(laserScanTopic, scan);
    }

    public static TimeMsg GetTimeNow()
    {
        double rosTime = Time.realtimeSinceStartupAsDouble + rosUnityTimeOffset;
        int secs = (int)Math.Floor(rosTime);
        uint nsecs = (uint)((rosTime - secs) * 1e9);
        return new TimeMsg(secs, nsecs);
    }
}

