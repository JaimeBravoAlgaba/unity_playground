using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using RosMessageTypes.Geometry;
using RosMessageTypes.Nav;
using RosMessageTypes.Tf2;
using RosMessageTypes.BuiltinInterfaces;
using System;

public class Nav2Publisher : MonoBehaviour
{
    ROSConnection ros;

    [Header("Frame Transforms")]
    public Transform baseLink;
    public Transform lidarTransform;
    public Transform cameraTransform;

    [Header("ROS Settings")]
    public string odomTopic = "/odom_unity";
    public string tfTopic = "/tf_unity";
    public float publishRateHz = 20f;

    private Vector3 lastPosition;
    private Quaternion lastRotation;
    private Vector3 smoothedLinearVelocity = Vector3.zero;
    private Vector3 smoothedAngularVelocity = Vector3.zero;
    private float smoothingFactor = 0.1f;

    private float timeElapsed;

    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<OdometryMsg>(odomTopic);
        ros.RegisterPublisher<TFMessageMsg>(tfTopic);

        lastPosition = baseLink.position;
        lastRotation = baseLink.rotation;
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= 1f / publishRateHz)
        {
            Vector3 currentPosition = baseLink.position;
            Quaternion currentRotation = baseLink.rotation;

            Vector3 rawLinearVelocity = (currentPosition - lastPosition) / timeElapsed;
            Vector3 deltaEuler = (currentRotation.eulerAngles - lastRotation.eulerAngles);
            Vector3 rawAngularVelocity = deltaEuler / timeElapsed;

            smoothedLinearVelocity = Vector3.Lerp(smoothedLinearVelocity, rawLinearVelocity, smoothingFactor);
            smoothedAngularVelocity = Vector3.Lerp(smoothedAngularVelocity, rawAngularVelocity, smoothingFactor);

            PublishOdom();
            PublishTFs();

            lastPosition = currentPosition;
            lastRotation = currentRotation;
            timeElapsed = 0;
        }
    }

    void PublishOdom()
    {
        Vector3 unityPos = baseLink.position;
        Quaternion unityRot = baseLink.rotation;

        Vector3 rosPos = new Vector3(unityPos.z, -unityPos.x, unityPos.y);
        Quaternion rosRot = new Quaternion(unityRot.z, unityRot.x, unityRot.y, -unityRot.w);

        OdometryMsg odom = new OdometryMsg
        {
            header = new RosMessageTypes.Std.HeaderMsg
            {
                frame_id = "odom",
                stamp = RosUtil.GetTimeNow()
            },
            child_frame_id = "base_link",
            pose = new PoseWithCovarianceMsg
            {
                pose = new PoseMsg
                {
                    position = new PointMsg(rosPos.x, rosPos.y, rosPos.z),
                    orientation = new QuaternionMsg(rosRot.x, rosRot.y, rosRot.z, rosRot.w)
                }
            },
            twist = new TwistWithCovarianceMsg
            {
                twist = new TwistMsg
                {
                    linear = new Vector3Msg(smoothedLinearVelocity.x, smoothedLinearVelocity.y, smoothedLinearVelocity.z),
                    angular = new Vector3Msg(smoothedAngularVelocity.x, smoothedAngularVelocity.y, smoothedAngularVelocity.z)
                }
            }
        };

        ros.Publish(odomTopic, odom);
    }

    void PublishTFs()
    {
        TFMessageMsg tfMessage = new TFMessageMsg
        {
            transforms = new TransformStampedMsg[]
            {
                CreateTransform("odom", "base_link", baseLink),
                CreateTransform("base_link", "lidar_link", lidarTransform),
                CreateTransform("base_link", "camera", cameraTransform)
            }
        };

        ros.Publish(tfTopic, tfMessage);
    }

    TransformStampedMsg CreateTransform(string parent, string child, Transform t)
    {
        Vector3 unityPos = t.position;
        Quaternion unityRot = t.rotation;

        Vector3 rosPos = new Vector3(unityPos.z, -unityPos.x, unityPos.y);
        Quaternion rosRot = new Quaternion(unityRot.z, unityRot.x, unityRot.y, -unityRot.w);

        return new TransformStampedMsg
        {
            header = new RosMessageTypes.Std.HeaderMsg
            {
                frame_id = parent,
                stamp = RosUtil.GetTimeNow()
            },
            child_frame_id = child,
            transform = new TransformMsg
            {
                translation = new Vector3Msg(rosPos.x, rosPos.y, rosPos.z),
                rotation = new QuaternionMsg(rosRot.x, rosRot.y, rosRot.z, rosRot.w)
            }
        };
    }
}

public static class RosUtil
{
    private static double rosUnityTimeOffset = 0;

    public static void SyncTimeWithRos(double rosTimeAtStart)
    {
        rosUnityTimeOffset = rosTimeAtStart - Time.realtimeSinceStartupAsDouble;
    }

    public static TimeMsg GetTimeNow()
    {
        double rosTime = Time.realtimeSinceStartupAsDouble + rosUnityTimeOffset;
        int secs = (int)Math.Floor(rosTime);
        uint nsecs = (uint)((rosTime - secs) * 1e9);
        return new TimeMsg(secs, nsecs);
    }
}

