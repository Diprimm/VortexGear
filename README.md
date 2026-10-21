# VortexGear - 6DOF Third-Person Spaceship Prototype

A technical Unity prototype focused on implementing a **6DOF (Six Degrees of Freedom)** third-person spaceship controller. The primary objective of this project was to master Unity's **New Input System** and deliver a high-fidelity **Game Feel** through advanced camera mechanics, post-processing, and physics-driven systems.

This project serves as a showcase of core **Gameplay Programming** skills, architecture modularity, and creative problem-solving under physics constraints.


<p align="center">
  <video width="700" muted controls loop autoplay>
    <source src="https://github.com/user-attachments/assets/2013b4a3-88cf-416c-a30e-9ef004d46e39" type="video/mp4">
    Your browser does not support the video tag.
  </video>
</p>

---

## 🛠️ Core Technical Features

### 1. Physics-Driven 6DOF Controller
*   **Rigidbody Architecture:** Spaceship movement is entirely physics-driven, utilizing `Rigidbody.AddForce` and `Rigidbody.AddTorque` to achieve authentic 6DOF control (pitch, yaw, roll, and 3-axis translation).
*   **New Input System Integration:** Leverages Unity's New Input System with action maps specifically tailored for complex, multi-axis flight controls, ensuring seamless rebindability and responsiveness.

### 2. High-Fidelity Game Feel & Juice
*   **Speed Shader:** A custom full-screen shader designed to introduce screen-space procedural lines and distortion to dynamically convey extreme velocity.
*   **Camera Polish:** Integrated dynamic camera shakes (tailored via code/Cinemachine) coupled with Post-Processing effects like motion to amplify the sensation of acceleration.

---

## 🧠 Technical Challenges & Solutions

### 🛸 The Evade Mechanic (Dodge/Barrel Roll)
The greatest engineering challenge during development was implementing the spaceship's **Evade / Dodge** ability. This mechanic allows the ship to perform a rapid axial rotation to dodge incoming projectiles and obstacles.

*   **The Problem:** Initial iterations felt extremely rigid and "clunky." The first approach utilized a traditional Unity Keyframe Animation component. However, this was quickly discarded because it lacked **modularity and scalability**: it was impossible to tweak individual values (like speed or rotation arcs) for different spaceship models without creating entirely new animation clips.
*   **The Solution:** The mechanic was completely refactored to be **code-driven**. By utilizing mathematical interpolation (Lerp/Slerp) and physics torque overrides inside a modular state machine, the roll behavior became fully procedural. This allows any future spaceship variant to use the exact same script while customizing dodge speed, distance, and angular velocity straight from the Inspector.

---

## 📝 License

This project is licensed under a custom Software and Source Code License Agreement. Commercial use, external project distribution, and asset exploitation are strictly prohibited without prior authorization. Please refer to the full [LICENSE](LICENSE) file for details.

---

## ⚠️ Important Note Regarding 3D Assets

To comply with copyright guidelines and intellectual property restrictions, **the original 3D spaceship model used in development is NOT included in this repository**. 

If you clone or download this project to test or build upon it:
*   You will need to import your own 3D model or use a Unity primitive (like a capsule or cube) as a placeholder.
*   Simply assign your custom model to the spaceship's Prefab/GameObject structure, and the physics-driven movement scripts will work seamlessly.

---
