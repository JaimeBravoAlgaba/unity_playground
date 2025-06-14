# unity_playground
A Unity-ROS2 environment for testing mobile robots in different terrains.

## Setting up Unity - ROS2

- Please refer to the Unity-Robotics-Hub repository for installation and setup: https://github.com/Unity-Technologies/Unity-Robotics-Hub?tab=readme-ov-file

- Else you can follow my own streamlined complete guide right [here](https://github.com/JaimeBravoAlgaba/unity_playground/blob/main/Unity_Playground/Setting%20up%20Unity-ROS2%20environment.md "Streamlined guide") 

## Unity assets

Please refer to the [Assets guide]() for more details and implementation directives.

- Camera scripts:
  - `CameraToggle` allows to toggle between two cameras by pressing `c`. For example first and third person.
  - `FollowRobot` defines the third person view of the robot.
  - `RobotVision` defines the first person view of the robot.
  - `RosImagePublisher` is the script used to publish both the raw and compressed image to a ROS2 topic.
 
- Robot scripts:
  - `RobotDrive` allows the user to control the Lynxmotion mobile base with the `WASD` keys.
  - `RobotReset` supposedly unblocks the robot if ever needed (not yet working).
 
- URDF:
  - contains the URDF of the Lynxmotion robot base.
