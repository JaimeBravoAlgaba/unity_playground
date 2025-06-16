# Assets Guide

## - URDF robot:

You can find the URDF files of the Lynxmotion A4WD-3 under ```Assets/Robot/URDF```. Add this last file to your ```Assets``` folder in Unity then right click on you hierarchy and select ```3D Object/URDF Model (import)``` then select the URDF model you wish to import.

Otherwise you may create or add you own URDF model, for more information please refer to the [Unity-Robotics-Hub](https://github.com/Unity-Technologies/Unity-Robotics-Hub?tab=readme-ov-file "Unity-Robotics-Hub") repository.

Next you will need to set up ```Wheel Collider``` components for Unity to properly integrate wheel physics:

- In ```Hierarchy``` add empty objects with ```Ctrl+Maj+N``` under the base link of your URDF model.
- Add the component ```Wheel Collider``` and fill in with the following values (which I found to work well with the Lynxmotion A4WD-3):

![image](https://github.com/user-attachments/assets/8445c969-d844-4bd2-a6fd-b51de4fbe08a)
