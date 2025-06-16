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
