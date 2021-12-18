# Walking Simulator Framework for Unity

> Set of tools used for walking simulators or exploration based games.

#### Features

* Ability to examine, use, and equip items in various customizable ways
* Includes a First Person Controller that uses [Unity's new Input system](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.2/manual/index.html)
* Uses the Universal Render Pipeline (URP) to highlight items selectively
* Setup to add animated doors, drawers, cabinets that can open/close
* Event driven items (ex: light switch toggle)
* Equipment items that can be used via an inventory UI and used on other items in the scene (ex: keys for doors)
* Support for sound effects when using items 
* Test scene with several prefab based examples
* Basic Inventory UI (included for demonstration purposes)

Compatible with Unity 2020.3.12f, 2021.1.0f1 and above

---

![BookAnim_02](https://user-images.githubusercontent.com/10926088/146653353-dd44baf3-ccf6-4ba0-8537-57b53a6192c9.gif)

![Overalnteractions_03](https://user-images.githubusercontent.com/10926088/146653363-0c31f404-aeea-47c7-ab82-9048f64fbbe0.gif)


---

## Setup

Unity Packages needed:

* Input System
* Universal Render Pipeline (URP)
* Naughty Attributes
* SmartData

---

## How to use

There are two options. Either download the whole project, open it with Unity, and open the scene included in the project (SampleScene).
This wiil include a basic Inventory/HUD system.

Or download the Unity package from the [Releases](https://github.com/fstelluti/WalkingSimulatorFramework/releases) section and set it up in another project.

More details for the package setup and general information is outlined in the [Wiki](https://github.com/fstelluti/WalkingSimulatorFramework/wiki).

---

## Troubleshooting

Check the current [Issues](https://github.com/fstelluti/WalkingSimulatorFramework/issues) to make sure that the problem doesn't already exist.

Otherwise, comments and pull requests are welcome!

---

### Credits

Uses

* A modified version of [VeryHotShark's First Person Controller](https://github.com/VeryHotShark/First-Person-Controller-VeryHotShark) - Fixed some minor bugs and upgraded it to be able to use Unity's new Input system.
* [SmartData](https://github.com/sigtrapgames/SmartData) - Global data/states and events
* [Naughty Attributes](https://github.com/dbrizov/NaughtyAttributes) - Inspector improvements 
* [Free 3D models](https://free3d.com/) - Scene demonstration assets
* [Free Sounds](https://freesound.org/) - Item Sounds
