# Assets Guide

## - Import the scene

- Import the [.unitypackage](https://github.com/JaimeBravoAlgaba/unity_playground/blob/main/Unity_Playground/Unity_Playground_CAR.unitypackage) file into your Unity assets with "right click"/Import Package/Custom Package... from the project window.

- Navigate to /Scenes and open Unity_Playground_CAr, then feel free to delete the sample scene as you won't need it.

You should be set with the basics for this project as the file contains the Robot object as well as RGBD camera and LiDAR with their respective ROS publisher scripts operational.

- Now you will need to remove all the console errors:
  - In the ```Package Manager``` select ```Install package from git URL``` and paste the following links:
```bash
https://github.com/Field-Robotics-Japan/UnitySensors.git?path=/Assets/UnitySensors#v2.0.5
```
```bash
https://github.com/Field-Robotics-Japan/UnitySensors.git?path=/Assets/UnitySensorsROS#v2.0.5
```

All console errors should disappear.

## - ROS2 files

- Unity's clock and ROS2's clock are not sychronized, a ROS2 republisher is therefore necessary in order to be able tu use ```Nav2``` and ```SLAM```.
    - Source and run the ROS2 node by opening a terminal in ```unity_Nav2/unity_nav2``` sourcing and running
```bash
ros2 run unity_nav2 unity_nav2_republish
```
- In the ```ros2_cmd``` text file you will find all the commands that I use and plan to use (in the case of nav2 and slam, which are not yet operational).
  
