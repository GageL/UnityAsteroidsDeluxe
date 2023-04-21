<div align="center">

![readme splash](https://i.imgur.com/UP2Qe56.png)

<br>

[**Installation**](#installation) &middot;
[**Gameplay**](#gameplay) &middot;
[**User Interface**](#user-interface) &middot;
[**Customization**](#customization) &middot;
[**TODO**](#todo) &middot;
[**License**](./LICENSE.md)

<br>

> Source: This is a full Unity Project with associated packages, assets, settings and game logic.
  
> Assets: This project utilizes packs for **concept** purposes from the [Kenney](https://kenney.nl/) packs, primarily the [Simple Space](https://kenney.nl/assets/simple-space) pack.

> Licensing: Any and all licensed packages or assets included in this project and repository have either CC0 or MIT release.

</div>

# Installation

### Dependencies

The external libraries that are used in this project are as follows. Note, to add these packages to your project, simply navigate to your project `manifest.json` file located within your project's Packages directory and add the following lines.

#### Unity Tweens

```json
"nl.elraccoone.tweens": "git+https://github.com/jeffreylanters/unity-tweens"
```

#### NaughtyAttributes

```json
"com.dbrizov.naughtyattributes": "https://github.com/dbrizov/NaughtyAttributes.git#upm"
```

#### TextMeshPro

> For this package, be sure to import the default package included with TMP (Window =-> TextMeshPro -> Import TMP Essential Resources)

> Additionally, the current implemented version `3.0.6` includes specific settings for the UI components. Any previous version may contain bugs, graphic artifacts or redactions in functionality

```json
"com.unity.textmeshpro": "3.0.6"
```

### Usage

To utilize specifically the project source, navigate to the `{DRIVE}:\{PATH_TO_CLONE}\AsteroidsDeluxe\Assets\_AsteroidsDeluxe` directory and implement that folder into your own project.

#### Editor

The `[Foldout("Runtime Debug")]` attribute that is utilized through the project is meant as a way to visualize values set at runtime and as such, members that fall under this attribute are to be left at their default values and not be edited before playmode.

# Gameplay

## Demo Link

https://drive.google.com/file/d/195PF2jJuDt5IkVJY4X_lgedlmdmyGyqB/view?usp=sharing

### Player

#### Controls

| Keybind         | Description     |
|-----------------|-----------------|
| W / Up Arrow | Holding this key will move the ship in a "forward" motion in relation to the rotation |
| S / Down Arrow | Holding this key will move the ship in a "backward" motion in relation to the rotation |
| A / Left Arrow | Holding this key will rotate the ship in a "counter clockwise" motion |
| D / Right Arrow | Holding this key will rotate the ship in a "clockwise" motion |
| Space / Right Control | Pressing this key will shoot projectiles from the player ship |

#### Lives

The player has a configurable amount of lives that are set at the start of each game session. When the player ship collides with an obstacle or is hit by an enemy projectile, the ship is destroyed and a life is subtracted. When there are no more lives and the last ship is destroyed, the game is over.

#### Score

When the player destroys an obstacle or enemy, they are granted points relative to a configurable amount on the destroyed object.

#### Combat

The player has the ability to fire projectiles from the *nose* of the ship. These configurable projectiles has a specific speed and lifetime associated with each *bullet*. When the projectile collides with an enemy/obstacle, the obstacle is destroyed and thus score points are gained.

### AI

#### Types

**Obstacles**

* Small Asteroid
* Medium Asteroid
* Large Asteroid
* Satellite

**Enemies**

* Alien Ship
* Mothership
* Mothership Shield Segment
* Mothership Segment

#### Spawning

Each enemy/obstacle has a random chance to spawn at the extents of the players view with a random Y value. Additionally, a random rotation is appended to give a varied distribution of movement.

#### Trajectory

The trajectory of an enemy/obstacle is based off of its spawn rotation and speed as described in Spawning above. Additionally, some obstacles and enemies may fracture which will create new trajectories for the fractures that are different than the originator.

#### Fracturing

Fracturing can take place on an enemy/obstacle if configured. This essentially means that upon destruction of itself, it can spawn new objects at its destruction origin point and provide said objects with new random trajectories.

# User Interface

#### PreGame Canvas

This UI is meant to showcase previous players scores and to act as a starting point for a new game session.

#### InGame Canvas

This UI is meant to house the players Score, Lives, Score Milestone, Wave Popup and Session End/Completed screens.

#### Global Canvas

This UI is meant to house specific controls that are universal across the experience such as Audio control.

# Customization

### Managers

#### Application Manager

The logic here is meant for data storage and exchange as well as initializing the game window and app setup. Customization features such as App Start Clip can be set here.

#### Audio Manager

The logic here drives the playback of audo through he default unity engine audio system.

#### Game State Manager

The logic here hosts the players state as well as that of the game session. Customization features such as Milestone Data, Game Start Delay, Starting Lives, Respawn Delay and audio queues can be set here.

#### Actor Wave Manager

The logic here runs the entire spawn logic loop and tracking for the game session as well as configuration for Finite Waves and Infinite Wave modes. Customization features such as Finite Waves, Infinite Wave Base, Infinite Wave Divisor, Start Spawn Threshold, Start Spawn Rate Override Min/Max, Next Wave Delay and Wave Cleared Clip can be set here.

# TODO

These tasks are the current planned features that are on the roadmap to be either completed *(in progress)* or started *(backlog)*

- [ ] Additional documentation on Managers
- [ ] Additional documentation on User Interface
- [ ] Additional documentation on Customization
- [ ] Additional class XML documentation
- [ ] Additional in-editor tooltips
- [ ] Name & score entering
- [ ] AI trajectory out of bounds case **(bug)**
- [ ] Enemies logic
- [ ] Enemies projectiles / homing
- [ ] Wave cleared popup **(bug)**
- [ ] Right side spawning
