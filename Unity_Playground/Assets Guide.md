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
