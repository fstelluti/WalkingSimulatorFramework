using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WalkingSimFramework.Scriptable_Objects
{
    public enum TransformTarget
    {
        Position,
        Rotation,
        Both
    }

    /// <summary>
    /// Data for random noise applied to either the camera position, rotation, or both
    /// Credit to VeryHotShark's FPS controller: https://github.com/VeryHotShark/First-Person-Controller-VeryHotShark
    /// </summary>
    [CreateAssetMenu(fileName = "Perlin Camera Noise Data", menuName = "My Interaction System/PerlinCameraNoiseData")]
    public class PerlinCameraNoiseData : ScriptableObject
    {
        public TransformTarget transformTarget;

        [Space]
        public float amplitude;
        public float frequency;
    }
}
