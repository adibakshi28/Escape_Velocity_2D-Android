# Escape Velocity 2D

Escape Velocity 2D is an exhilarating endless scroller game set in outer space, featuring realistic gravitational physics and unique game dynamics. Developed using the Unity engine, this repository includes all the necessary files and resources to build, run, and modify the game. Players navigate a spaceship through space, collecting items and avoiding obstacles to achieve high scores based on distance traveled and collectables gathered.

## Table of Contents
- [Introduction](#introduction)
- [Features](#features)
- [Installation](#installation)
- [Usage](#usage)
- [Project Structure](#project-structure)
- [Gameplay](#gameplay)
- [Controls](#controls)
- [Scripts Overview](#scripts-overview)
  - [GameController.cs](#gamecontrollercs)
  - [PlayerController.cs](#playercontrollercs)
  - [EnemyController.cs](#enemycontrollercs)
  - [ObstacleController.cs](#obstaclecontrollercs)
  - [UIController.cs](#uicontrollercs)
- [Screenshots](#screenshots)
- [Contributing](#contributing)
- [License](#license)
- [Contact](#contact)

## Introduction

Escape Velocity 2D is a captivating 2D endless scroller game where players control a spaceship, navigating through space while avoiding obstacles and collecting items. The game is designed with Unity, providing a seamless and engaging experience for Android users.

<div style="display: flex; justify-content: space-between;">
  <img src="Game%20Screenshot/EV%20(11).jpeg" alt="Game Screenshot 11" style="width: 32%">
  <img src="Game%20Screenshot/EV%20(10).jpeg" alt="Game Screenshot 10" style="width: 32%">
  <img src="Game%20Screenshot/EV%20(13).jpeg" alt="Game Screenshot 13" style="width: 32%">
</div>

## Features

- **Realistic Physics:** Experience realistic gravitational physics in a 2D space environment.
- **Unique Game Dynamics:** Utilize gravitational pulls and other unique dynamics to navigate.
- **High-Quality Graphics:** Stunning 2D graphics and animations.
- **Intuitive Controls:** Smooth touch controls for an enhanced gaming experience.
- **Endless Scrolling:** Infinite gameplay with increasing difficulty.
- **Immersive Sound Effects:** High-quality sound effects to enhance the gaming experience.
- **Real-time Feedback:** Instant feedback on player performance.

## Installation

To set up and run the project locally, follow these steps:

### Prerequisites

- Unity Hub installed.
- Unity Editor version 2017 or later.
- Android SDK configured.

### Steps

1. Clone the repository:

    ```sh
    git clone https://github.com/adibakshi28/Escape_Velocity_2D-Android.git
    ```

2. Open the project in Unity:
    - Open Unity Hub.
    - Click on "Add" and select the cloned project directory.
    - Open the project.

3. Configure Build Settings for Android:
    - Navigate to `File > Build Settings`.
    - Select Android and click on `Switch Platform`.
    - Adjust player settings, including package name and version.

4. Build the Project:
    - Connect your Android device or set up an emulator.
    - Click on `Build and Run` to generate the APK and install it on the device.

## Usage

After building the project, install the APK on your Android device. Launch the game and follow the on-screen instructions to start playing. Use touch controls to navigate through space, avoid obstacles, and collect items to achieve high scores.

## Project Structure

- **Assets:** Contains all game assets, including:
    - **Scenes:** Different menus and UI elements.
    - **Scripts:** C# scripts for game logic.
    - **Prefabs:** Pre-configured game objects.
    - **Animations:** Animation controllers and clips.
    - **Audio:** Sound effects and music files.
    - **UI:** User interface elements.
- **Packages:** Unity packages used in the project.
- **ProjectSettings:** Project settings including input, tags, layers, and build settings.
- **.gitignore:** Specifies files and directories to be ignored by Git.
- **LICENSE:** The license under which the project is distributed.
- **README.md:** This readme file.

## Gameplay

Players navigate a spaceship through space, avoiding obstacles and collecting items. The game includes:

- **Endless Scrolling:** Infinite gameplay with increasing difficulty.
- **Objectives:** Navigate through space, avoid obstacles, and collect items to achieve high scores.
- **Scoring:** Points are awarded based on the distance traveled and items collected.
- **Feedback:** Real-time feedback to help players improve.

## Controls

- **Touch Controls:** Use touch inputs to navigate the spaceship and interact with the game environment.

## Scripts Overview

The `Assets/Scripts` directory contains essential C# scripts that drive the game's functionality. Here's a detailed overview:

### GameController.cs

Manages the overall game state, including game flow, starting and ending sessions, tracking player progress, and updating the UI with scores and other information.

### PlayerController.cs

Defines the behavior of the player spaceship, including movement and interactions with the environment and enemies.

### EnemyController.cs

Manages the behavior and interactions of enemy ships within the game, including movement, attacks, and collisions.

### ObstacleController.cs

Controls the behavior of obstacles within the game, including movement and collision detection.

### UIController.cs

Manages the user interface, handling interactions with menus, buttons, and other UI elements.

## Screenshots

Here are some screenshots of the game:

<div style="display: flex; flex-wrap: wrap; justify-content: space-between;">
  <img src="Game%20Screenshot/EV%20(1).jpeg" alt="Game Screenshot 1" style="width: 32%; margin-bottom: 10px;">
  <img src="Game%20Screenshot/EV%20(2).jpeg" alt="Game Screenshot 2" style="width: 32%; margin-bottom: 10px;">
  <img src="Game%20Screenshot/EV%20(3).jpeg" alt="Game Screenshot 3" style="width: 32%; margin-bottom: 10px;">
  <img src="Game%20Screenshot/EV%20(4).jpeg" alt="Game Screenshot 4" style="width: 32%; margin-bottom: 10px;">
  <img src="Game%20Screenshot/EV%20(5).jpeg" alt="Game Screenshot 5" style="width: 32%; margin-bottom: 10px;">
  <img src="Game%20Screenshot/EV%20(6).jpeg" alt="Game Screenshot 6" style="width: 32%; margin-bottom: 10px;">
  <img src="Game%20Screenshot/EV%20(7).jpeg" alt="Game Screenshot 7" style="width: 32%; margin-bottom: 10px;">
  <img src="Game%20Screenshot/EV%20(8).jpeg" alt="Game Screenshot 8" style="width: 32%; margin-bottom: 10px;">
  <img src="Game%20Screenshot/EV%20(9).jpeg" alt="Game Screenshot 9" style="width: 32%; margin-bottom: 10px;">
  <img src="Game%20Screenshot/EV%20(10).jpeg" alt="Game Screenshot 10" style="width: 32%; margin-bottom: 10px;">
  <img src="Game%20Screenshot/EV%20(11).jpeg" alt="Game Screenshot 11" style="width: 32%; margin-bottom: 10px;">
  <img src="Game%20Screenshot/EV%20(12).jpeg" alt="Game Screenshot 12" style="width: 32%; margin-bottom: 10px;">
  <img src="Game%20Screenshot/EV%20(13).jpeg" alt="Game Screenshot 13" style="width: 32%; margin-bottom: 10px;">
  <img src="Game%20Screenshot/EV%20(14).jpeg" alt="Game Screenshot 14" style="width: 32%; margin-bottom: 10px;">
  <img src="Game%20Screenshot/EV%20(15).jpeg" alt="Game Screenshot 15" style="width: 32%; margin-bottom: 10px;">
</div>

## Contributing

Contributions are welcome and greatly appreciated. To contribute:

1. Fork the repository:
    - Click the "Fork" button at the top right of the repository page.

2. Create a feature branch:

    ```sh
    git checkout -b feature/AmazingFeature
    ```

3. Commit your changes:

    ```sh
    git commit -m 'Add some AmazingFeature'
    ```

4. Push to the branch:

    ```sh
    git push origin feature/AmazingFeature
    ```

5. Open a pull request:
    - Navigate to your forked repository.
    - Click on the "Pull Request" button and submit your changes for review.

## License

This project is licensed under the MIT License. See the LICENSE file for more information.

## Contact

For any inquiries or support, feel free to contact:

[Adibakshi28 - GitHub Profile](https://github.com/adibakshi28)

Project Link: [Escape Velocity 2D-Android](https://github.com/adibakshi28/Escape_Velocity_2D-Android)
