using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WalkingSimFramework.Scriptable_Objects
{
    /// <summary>
    /// Data class that data for the head (camera) bobbing
    /// Credit to VeryHotShark's FPS controller: https://github.com/VeryHotShark/First-Person-Controller-VeryHotShark
    /// </summary>
    [CreateAssetMenu(fileName = "Head Bobbing Data", menuName = "My Interaction System/HeadBobbingData")]
    public class HeadBobbingData : ScriptableObject
    {
        [BoxGroup("Curves")] public AnimationCurve xCurve;
        [BoxGroup("Curves")] public AnimationCurve yCurve;

        [Space]
        [BoxGroup("Amplitude")] public float xAmplitude;
        [BoxGroup("Amplitude")] public float yAmplitude;

        [Space]
        [BoxGroup("Frequency")] public float xFrequency;
        [BoxGroup("Frequency")] public float yFrequency;

        [Space]
        [BoxGroup("Run Multipliers")] public float runAmplitudeMultiplier;
        [BoxGroup("Run Multipliers")] public float runFrequencyMultiplier;

        [Space]
        [BoxGroup("Crouch Multipliers")] public float crouchAmplitudeMultiplier;
        [BoxGroup("Crouch Multipliers")] public float crouchFrequencyMultiplier;

        public float MoveBackwardsFrequencyMultiplier { get; set; }
        public float MoveSideFrequencyMultiplier { get; set; }
    }
}
