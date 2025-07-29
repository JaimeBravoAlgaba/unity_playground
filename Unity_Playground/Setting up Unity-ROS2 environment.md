# Setting up Unity-ROS2 environment

# Installation of Unity packages

- Access the package manager in Unity and click on the + sign in the upper left corner of the package manager window.
  
- Select install package form git URL
  
- Paste the following links for the ROS-TCP-Connector, visualisations and the URDF-Importer packages necessary for ROS-Unity communications:
  - `https://github.com/Unity-Technologies/ROS-TCP-Connector.git?path=/com.unity.robotics.ros-tcp-connector`
  - `https://github.com/Unity-Technologies/ROS-TCP-Connector.git?path=/com.unity.robotics.visualizations`

Please refer to the individual repos for the latest versions, all accessible through the Unity-Robotics-Hub repo `https://github.com/Unity-Technologies/Unity-Robotics-Hub/tree/main`

- Clone the Unity-Robotics-Hub repository in your workspace by typing:
  ```bash
  git clone https://github.com/Unity-Technologies/Unity-Robotics-Hub.git
  ```
  
- Clone the [ROS2 branch of the ROS-TCP-Endpoint](https://github.com/Unity-Technologies/ROS-TCP-Endpoint/tree/main-ros2) repository into the `src` folder in your Colcon workspace.
- Use the following command line to clone the right branch.

  ```bash
  git clone -b main-ros2 https://github.com/Unity-Technologies/ROS-TCP-Endpoint.git
  ```

-   Then navigate to your Colcon workspace and run the following commands:

    ```bash
	source install/setup.bash
    colcon build
	source install/setup.bash
	```

	Note: yes, you need to run the source command twice. The first sets up the environment for the build to use, the second time adds the newly built packages to the environent.

- In your Colcon workspace, run the following command, replacing `<your IP address>` with your ROS machine's IP or hostname.

	```bash
	ros2 run ros_tcp_endpoint default_server_endpoint --ros-args -p ROS_IP:=<your IP address>
    ```
- Run ```ifconfig``` in a terminal to find out your IP address.
  
- In the Unity menu bar open the `Robotics` tab and select `ROS Settiings`.
  
- Make sure you have the same fields, pay close attention to the `Protocol` field being on `ROS1` by default, set it to ```ROS2```. Replace the ```ROS IP Address``` with your IP address.

![image](https://github.com/user-attachments/assets/a30afe80-e161-4d0d-990b-ad0a66358b1c)

- In the Unity menu bar, the `Robotics` tab and select `Generate ROS Messages`. In the Message Browser window, click the Browse button at the top right to set the ROS message path to tutorials/ros_unity_integration/ros2_packages/unity_robotics_demo_msgs in the cloned repo.


- In the message browser, expand unity_robotics_demo_msgs and click `Build 2 msgs` and `Build 2 srvs` to generate C# scripts from the ROS .msg and .srv files.

![image](https://github.com/user-attachments/assets/9d3a4f0c-d1d7-473b-83c9-d35c60d10fd7)

- Set up the ROSPublisher by creating an empty game object in the Unity hierarchy and add the `RosImagePublisher` script as a component naming the topics and selecting the target camera, make sure to dissble that camera for the sake of the [Assets guide](https://github.com/JaimeBravoAlgaba/unity_playground/blob/main/Unity_Playground/Assets/Assets%20Guide.md "Assets guide") which I recommed you follow next.

- Run `rviz2` and add visualization by topic. Select the `Image` topic.
