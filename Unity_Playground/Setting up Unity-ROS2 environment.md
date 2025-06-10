# Setting up Unity-ROS2 environment

# Installation of Unity packages

- Access the package manager in Unity and click on the + sign in the upper left corner of the package manager window.
  
- Select install package form git URL
  
- Paste the following links for the ROS-TCP-Connector, visualisations and the URDF-Importer packages necessary for ROS-Unity communications:
  - `https://github.com/Unity-Technologies/ROS-TCP-Connector.git?path=/com.unity.robotics.ros-tcp-connector`
  - `https://github.com/Unity-Technologies/ROS-TCP-Connector.git?path=/com.unity.robotics.visualizations`
  - `https://github.com/Unity-Technologies/URDF-Importer.git?path=/com.unity.robotics.urdf-importer#v0.5.2`

Please refer to the individual repos for the latest versions, all accessible through the Unity-Robotics-Hub repo `https://github.com/Unity-Technologies/Unity-Robotics-Hub/tree/main`

- Clone the Unity-Robotics-Hub repository in your workspace by typing:
  ```bash
  git clone https://github.com/Unity-Technologies/Unity-Robotics-Hub.git
  ```
  
- Open a terminal in `tutorials/ros_unity_integration` of the cloned repo, paste and run the following lines:
  ```bash
  sudo docker build -t foxy -f ros2_docker/Dockerfile .
  sudo docker run -it --rm -p 10000:10000 foxy /bin/bash
  ```

If you get the error `failed: port is already allocated` after a misstype of the command enter `sudo lsof -i :10000` which will show the processes on port 10000 and then enter `sudo kill -9 <PID>` by replacing `<PID>` with the actual PID found in the list of processes.
  
- Then in the docker terminal run:
  ```bash
  ros2 run ros_tcp_endpoint default_server_endpoint --ros-args -p ROS_IP:=0.0.0.0
  ```
  
- In the Unity menu bar open the `Robotics` tab and select `ROS Settiings`.
  
- Make sure you have the same fields, pay close attention to the `Protocol` field being on `ROS1` by default

![image](https://github.com/user-attachments/assets/18c899a0-e67f-4612-92e4-df0765c7b9e7)

- In the Unity menu bar, the `Robotics` tab and select `Generate ROS Messages`. In the Message Browser window, click the Browse button at the top right to set the ROS message path to tutorials/ros_unity_integration/ros2_packages/unity_robotics_demo_msgs in the cloned repo.


- In the message browser, expand unity_robotics_demo_msgs and click `Build 2 msgs` and `Build 2 srvs` to generate C# scripts from the ROS .msg and .srv files.

![image](https://github.com/user-attachments/assets/9d3a4f0c-d1d7-473b-83c9-d35c60d10fd7)

- Set up the ROSPublisher by creating an empty game object in the Unity hierarchy and add the `RosImagePublisher` script as a component naming the topics and selecting the target camera.

- Run `rviz2` and add visualization by topic. Select the `Image` topic.
