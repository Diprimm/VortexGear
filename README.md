# VortexGear - 6DOF Third-Person Spaceship Prototype

A technical Unity prototype focused on implementing a **6DOF (Six Degrees of Freedom)** third-person spaceship controller. The primary objective of this project was to master Unity's **New Input System** and deliver a high-fidelity **Game Feel** through advanced camera mechanics, post-processing, and physics-driven systems.

This project serves as a showcase of core **Gameplay Programming** skills, architecture modularity, and creative problem-solving.

[Demo Gameplay](https://github.com/user-attachments/assets/2013b4a3-88cf-416c-a30e-9ef004d46e39)

---

## Core Technical Features

### 1. Physics-Driven 6DOF Controller
*   **Rigidbody Architecture:** Spaceship movement is entirely physics-driven, utilizing `Rigidbody.AddForce` and `Rigidbody.AddTorque` to achieve authentic 6DOF control (pitch, yaw, roll, and 3-axis translation).
*   **New Input System Integration:** Leverages Unity's New Input System with action maps specifically tailored for complex, multi-axis flight controls, ensuring seamless rebindability and responsiveness.

### 2. High-Fidelity Game Feel & Juice
*   **Speed Shader:** A custom full-screen shader designed to introduce screen-space procedural lines and distortion to dynamically convey extreme velocity.
*   **Exhaust Shader:** custom afterburner shader that reacts to the ship's speed (not fully polished).
*   **Camera Polish:** Integrated dynamic camera shakes (tailored via code/Cinemachine) coupled with Post-Processing effects like motion blur to amplify the sensation of acceleration.
  

---

## Creation Process

### 1. Integration with New Input System 
The controller was created using the Unity Events behavior within the Player Input component. An action map was created that contains the controller's actions.

| PlayerInputActions | PlayerInputComponent |
| :---: | :---: |
| <img width="546" height="282" alt="PlayerInputActions" src="https://github.com/user-attachments/assets/631f601a-465f-4276-9225-d4e028ca4afd" /> | <img width="351" height="562" alt="PlayerInputComponent" src="https://github.com/user-attachments/assets/dd50dec0-3df8-4205-8029-60af9def4c0b" /> |

| Code |
| :---: |
| <img width="501" height="472" alt="CodeInputs" src="https://github.com/user-attachments/assets/dc16ec21-15d1-44b7-8154-1851fa25aec6" /> |


### 2. Basic Rigidbody Physics movement
The movement of the ship is based on Unity's physics system. By default, the ship is always subject to a small forward force; for this reason, we assign `transform.forward` multiplied by `baseForwardForce` to the `forward` Vector3.

<img width="1903" height="635" alt="TraslationCode" src="https://github.com/user-attachments/assets/f44f4e27-693e-4539-88ac-ded7788a28b9" />

Ultimately, we combine the strafe and elevation components to determine the total translational force. Additionally, in a separate method, the ship's rotations are calculated using the look input and the roll input.

### 3. Acrobatic Movement implementation
Now for the most complex part, the acrobatic. This is handled by a Corrutine divided in 3 phases.

First, we define the total duration of the maneuver; phase 1 (the primary spinning action) will account for 40% of this total time.

<img width="907" height="301" alt="p1" src="https://github.com/user-attachments/assets/079a2256-f3dc-48c0-9e59-1e617336e506" />


<div align="center"> 
  
###  Phase 1

</div>

<img width="1136" height="557" alt="p1 1" src="https://github.com/user-attachments/assets/30b9a8b1-8b22-4cf4-8a52-48de1c115909" />

A `while` loop runs for the previously established duration.
Based on the elapsed time, we calculate a float value, `easedT`, derived from the `rollEasingCurve` at that specific point in time.
Using a curve allows for a custom motion, for instance, to incorporate easing (smoothing).
This normalized value is then multiplied by `rollAngle`.
`rollAngle` defines the final rotation angle; for example, a value of 360 results in a full rotation. This can be any value (ideally between 300 and a multiple of 360 plus an additional offset).
Finally, the rotation is applied to the model based on the incremental change required for the current frame.

<div align="center"> 
  
###  Phase 2 (optional, depending on the angle) 

</div>

<img width="1142" height="584" alt="p2" src="https://github.com/user-attachments/assets/bb6eecc9-39d4-4a58-841e-21039512fe3a" />

If the angle is not a multiple of 360, this phase ensures a smooth transition between the maneuver's final rotation and the ship's original rotation.
The logic is similar to the previous phase, but here, we interpolate between the current rotation and the ship's original rotation.

<div align="center"> 
  
### Phase 3 

</div>

Clean up the rotations.

<img width="962" height="196" alt="p3" src="https://github.com/user-attachments/assets/3e6b7ac0-34df-43d8-ad01-8230fe499474" />


---

## Technical Challenges & Solutions

### The Evade Mechanic (Dodge/Barrel Roll)
The greatest engineering challenge during development was implementing the spaceship's **Evade / Dodge** ability. This mechanic allows the ship to perform a rapid axial rotation to dodge incoming projectiles and obstacles.

*   **The Problem:** Initial iterations felt extremely rigid and "clunky." The first approach utilized a traditional Unity Keyframe Animation component. However, this was quickly discarded because it lacked **modularity and scalability**: it was impossible to tweak individual values (like speed or rotation arcs) for different spaceship models without creating entirely new animation clips.
*   **The Solution:** The mechanic was completely refactored to be **code-driven**. By utilizing mathematical interpolation (Lerp/Slerp) and physics torque overrides inside a modular state machine, the roll behavior became fully procedural. This allows any future spaceship variant to use the exact same script while customizing dodge speed, distance, and angular velocity straight from the Inspector.

---

## Requirements for use

### 1. Version
* This project is developed in Unity version 6.3.

### 2. Controls
* W: accelerate
* S: decelerate/move back
* A: move left
* D: move right
* Q: roll left
* E: roll right
* Space: move up
* Left Control: move down
* Left Shift: boost

* Mouse: look direction

## License

This project is licensed under a MIT License Agreement. Please refer to the full [LICENSE](LICENSE) file for details.

---

## ⚠️ Important Note Regarding 3D Assets

To comply with copyright guidelines and intellectual property restrictions, **the original 3D spaceship model used in development is NOT included in this repository**. 

If you clone or download this project to test or build upon it:
*   You will need to import your own 3D model or use a Unity primitive (like a capsule or cube) as a placeholder.
*   Simply assign your custom model to the spaceship's Prefab/GameObject structure, and the physics-driven movement scripts will work seamlessly.

---
