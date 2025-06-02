# unity_playground
A Unity-ROS2 environment for testing mobile robots in different terrains.

## Setting up Unity - ROS2
Please refer to the Unity-Robotics-Hub repository for installation and setup: https://github.com/Unity-Technologies/Unity-Robotics-Hub?tab=readme-ov-file

Else you can follow my installation procedure: assuming you already have unity, ros2 humble, and docker installed and properly set up.
- Follow the initial setup guide: https://github.com/Unity-Technologies/Unity-Robotics-Hub/blob/main/tutorials/quick_setup.md
  - please note that the git links might not be up to date so it is strongly encouraged to click on the TCP and URDF links to retreive the correct github link
- Now follow the demosetup: https://github.com/Unity-Technologies/Unity-Robotics-Hub/blob/main/tutorials/ros_unity_integration/setup.md
  - note that if the docker commands do not work add sudo at the beginnig of both and launch them as a single command
  - make sure you have cloned the repository on your computer and also have set up a ROS2 workspace, all files will be available in this repo anyways.
  - Make sure the ROS message path is directed to the package from original cloned repository not your workspace
- And setup the demo publisher to test proper installation: https://github.com/Unity-Technologies/Unity-Robotics-Hub/blob/main/tutorials/ros_unity_integration/publisher.md
