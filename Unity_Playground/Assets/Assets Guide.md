# Assets Guide

## - Create model for Lynxmotion A4WD-3:

- Please add the assets from this repo to your Unity project.
- In your Unity Hierarchy create an empty object with ```CTRL+SHIFT+N```. Rename it A4WD3 for good measure, this will be the root object of your model.
- Add the ```Articulation Body``` component and a ```Box Collider```. Set them as follows:

![image](https://github.com/user-attachments/assets/c0c6c4af-9ec8-4791-a8f2-b23ff9996acd)


- Drag and drop the ```A4WD3-Base``` mesh from ```Assets/Robot/Meshes``` onto the ```root object```, this is the visual you've just added. The body of the robot should now appear on screen.

Make sure you reset all the position parameters to 0 and reorient the visual accordingly.

- Now you can add the wheels by creating a child object of the root object, rename the wheel to distinguish between front and rear as well as left and right.

- Same as before add a visual but this time it will be ```A4WD3-Wheel``` in the same directory as before.

- Add an articulation body component to the wheel object as set it as follows, pay attention to the ```Revolute``` joint type:

![image](https://github.com/user-attachments/assets/438476a5-d1b7-4b72-95ae-51891e29e41b)

- Copy paste this object and rename to obtain all 4 wheel objects without having to add the component each time

- Position the child objects at x = &plusmn; 0.183 ; y = 0.034 ; z = &plusmn; 0.11. 

Next you will need to set up the colliders for the wheels.

- In ```Hierarchy``` add cylinders with right click ```3D Object/Cylinder``` and drag it on top of the wheel child objects in the hierarchy and scale them down to x = z = .158 and y = .042

- Delete the ```Capsule Collider``` component and replace it with a ```Mesh Collider``` in which you must check the ```Convex``` box.

- For now leave mesh renderer checked but once you've finished the tutorial you might uncheck it to remove the parasitic visual or even delete it.

- Do not forget to properly name and place each wheel collider in correspondence to the wheel it will act as.

- Now add the Camera in the same manner and place it correctly. Donot forget to add an ```Articulation body``` and leave the joint as ```Fixed```

- Your ```Hierarchy``` should look like this:

![image](https://github.com/user-attachments/assets/6afdb537-66cf-43a3-9b67-32061aa8c95d)

## - Animate your robot

Now we need to gain control of this robot, for this we will use Unity's integrated ```Input System Package```.

- Navigate to ```Edit/Project Settings/Input System Package``` and delete all actions and the action maps other than ```Player``` for good measure.
- You may remove all actions other than ```Move``` to tidy things up.
- You may also add a `ResetOrientation` (case sensitive) action and bind it to your preferred key (typically ```R```)
- Close the window and access the Inspector of your root object.
- Add a ```Player Input``` component and make sure it is set as follows:

![image](https://github.com/user-attachments/assets/fd96df09-51a3-4a1b-b43b-43a74bac0a60)

- Next add the ```Robot Controls``` script as a component and attach the wheel colliders in the order front left, front right, back left, back right.
- Values should automatically be correct but feel free to edit them to your liking in the Inspector:

![image](https://github.com/user-attachments/assets/9cc06b04-4752-4883-8ba1-05288d1f6524)

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
