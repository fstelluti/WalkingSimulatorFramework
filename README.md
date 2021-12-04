# Walking Simulator Framework for Unity

> Set of tools used for walking simulator or exploration based games.

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

Compatible with Unity 2020

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

Or download the Unity package and set it up in another project.

More detailed use cases will be outlined in the Wiki.

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
