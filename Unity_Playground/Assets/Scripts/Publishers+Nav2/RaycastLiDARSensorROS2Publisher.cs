using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;
using RosMessageTypes.Sensor;
using RosMessageTypes.Std;
using RosMessageTypes.BuiltinInterfaces;
using Unity.Collections;
using System;
using UnitySensors.Sensor.LiDAR;

[RequireComponent(typeof(RaycastLiDARSensor))]
public class RaycastLiDARSensorROS2Publisher : MonoBehaviour
{
    public string topicName = "/raycast_lidar";
    public string frameId = "lidar_link";

    private ROSConnection ros;
    private RaycastLiDARSensor lidar;

    void Start()
    {
        lidar = GetComponent<RaycastLiDARSensor>();
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<PointCloud2Msg>(topicName);
    }

    void Update()
    {
        var points = lidar.pointCloud.points;

        if (points.Length == 0) return;

        PointCloud2Msg msg = CreatePointCloud2Msg(points);
        ros.Publish(topicName, msg);
    }


    PointCloud2Msg CreatePointCloud2Msg(NativeArray<UnitySensors.Data.PointCloud.PointXYZI> points)
    {
        int pointCount = points.Length;
        int pointStep = 16; // x, y, z, intensity (4 floats)

        byte[] data = new byte[pointCount * pointStep];

        for (int i = 0; i < pointCount; i++)
        {
            var p = points[i];
            Buffer.BlockCopy(BitConverter.GetBytes(p.position.x), 0, data, i * pointStep + 0, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(p.position.y), 0, data, i * pointStep + 4, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(p.position.z), 0, data, i * pointStep + 8, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(p.intensity), 0, data, i * pointStep + 12, 4);
        }

        return new PointCloud2Msg
        {
            header = new HeaderMsg
            {
                frame_id = frameId,
                stamp = new TimeMsg
                {
                    sec = (int)(Time.timeSinceLevelLoad),
                    nanosec = (uint)((Time.timeSinceLevelLoad % 1) * 1e9)
                }

            },
            height = 1,
            width = (uint)pointCount,
            is_bigendian = false,
            point_step = (uint)pointStep,
            row_step = (uint)(pointCount * pointStep),
            is_dense = true,
            fields = new[]
            {
                new PointFieldMsg("x", 0, PointFieldMsg.FLOAT32, 1),
                new PointFieldMsg("y", 4, PointFieldMsg.FLOAT32, 1),
                new PointFieldMsg("z", 8, PointFieldMsg.FLOAT32, 1),
                new PointFieldMsg("intensity", 12, PointFieldMsg.FLOAT32, 1)
            },
            data = data
        };
    }
}
