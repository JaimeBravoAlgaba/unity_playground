# Assets Guide

## - Create URDF for Lynxmotion A4WD-3:

##### Feel free to follow this guide for any other URDF you wish to create, simply replace the meshes and link type.

- Please add the assets from this repo to your Unity project.
- In your Unity Hierarchy add ```3D Object/URDF Model(new)```
- Expand ```base_link``` and select ```Visuals``` the select ```Mesh``` in the dropdown menu and ```Add visual```.
- Drag and drop the ```A4WD3-NWheeled``` mesh from ```Assets/Robot/Meshes/A4WD3-NWheeled``` onto the ```unnamed``` object which is the visual you've just added. The body of the robot should now appear on screen.

Make sure you reset all the position parameters to 0.

- Now you can add a child link from the inspector of ```base_link```. Select the ```Fixed``` joint type and ```Add child link (with joint)```.

I recommend properly naming every child joint you create.

- Same as before add a visual but this time it will be ```A4WD3-Wheel``` under the ```Meshes/A4WD3-Wheel``` directory.
- Positioning the child link object at x = &plusmn; 0.138 ; y = 0.034 ; z = &plusmn; 0.11. 

Next you will need to set up ```Wheel Collider``` components for Unity to properly integrate wheel physics:

- In ```Hierarchy``` add empty objects with ```Ctrl+Maj+N``` under the base link of your URDF model.
- Add the component ```Wheel Collider``` and fill in with the following values (which I found to work well with the Lynxmotion A4WD-3):

![image](https://github.com/user-attachments/assets/8445c969-d844-4bd2-a6fd-b51de4fbe08a)

- Do not forget to properly name and place each wheel collider in correspondence to the wheel it will act as.

## - Animate your robot

Now we need to gain control of this robot, for this we will use Unity's integrated ```Input System Package```.

- Navigate to ```Edit/Project Settings/Input System Package``` and delete all actions and the action maps other than ```Player``` for good measure.
- Create a new action and name it ```Move```, set ```Action Type``` as ```Value``` and ```Control Type``` as ```Vector 2```.
- Now click on the ```+``` to the right of move and select ```Add Up/Down/Left/Right Composite``` and bind the new actions inuitively to ```WASD``` keys.
- Close the window and access the Inspector of ```Robot``` in your URDF object.
- Add a ```Rigidbody``` component and set the mass of the robot, 3kg in our case.
- Add a ```Player Input``` component and make sure it is set as follows:

![image](https://github.com/user-attachments/assets/fd96df09-51a3-4a1b-b43b-43a74bac0a60)

- Next add the ```RobotDrive``` script as a component and attach the wheel colliders in the order front left, front right, back left, back right.
- Do the same for the ```Wheel Visuals``` by drag and dropping the ```A4WD3-Wheel``` visual objects found under ```Visuals/unnamed``` in the same order as the wheel colliders.
- Ensure that you have the same settings in Robot's inspector:

![image](https://github.com/user-attachments/assets/e68e65b5-3807-4f9a-9a3b-6cc4251e1c8a)

- Make sure that for every link Urdf inertial scripts are as follows:

![image](https://github.com/user-attachments/assets/5d24b8c6-4183-49da-b0e3-9c8c4390a4be)

- Launch play mode and test if the robot is indeed moving.

## - Camera manager

- Navigate to ```Edit/Project Settings/Input System Package``` and add a new ```ToggleCamera``` action, its ```Action Type``` is set to ```Button``` by default. Bind it to ```C```.
- Create an ```Empty Game Object``` and name it ```Camera Manager```
- Create a new camera object and name it ```FPV```, rename the main camera to ```TPV``` (third person view), and place them both under the ```Camera Manager``` object for organising purposes.
- Add the ```CameraToggle``` script as a component of ```Camera Manager``` and attach the two cameras to the script. This will enable you to switch cameras by tapping ```c```
- Respectively add ```RobotVision``` and ```FollowRobot``` scripts to ```FPV``` and ```TPV``` cameras and attach the ```base_link``` object to the cameras.
- Disable one of the two cameras for the camera toggle script to work.
- Drag the ```Camera Manager``` object in the ```Camera Toggle``` field.
- If needed adjust the offset, pitch and yaw to your liking.
