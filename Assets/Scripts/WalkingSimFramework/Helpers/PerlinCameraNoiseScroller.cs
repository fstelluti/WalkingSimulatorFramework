using WalkingSimFramework.Scriptable_Objects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WalkingSimFramework.Helpers
{
    /// <summary>
    /// Class used to update the perlin noise
    /// Credit to VeryHotShark's FPS controller: https://github.com/VeryHotShark/First-Person-Controller-VeryHotShark
    /// </summary>
    public class PerlinCameraNoiseScroller
    {
        PerlinCameraNoiseData m_data;

        Vector3 m_noiseOffset;
        Vector3 m_noise;

        public Vector3 Noise => m_noise;

        public PerlinCameraNoiseScroller(PerlinCameraNoiseData _data)
        {
            m_data = _data;

            float randMaxRange = 32f;

            m_noiseOffset.x = Random.Range(0f, randMaxRange);
            m_noiseOffset.y = Random.Range(0f, randMaxRange);
            m_noiseOffset.z = Random.Range(0f, randMaxRange);
        }

        public void UpdateNoise()
        {
            float scrollOffset = Time.deltaTime * m_data.frequency;

            m_noiseOffset.x += scrollOffset;
            m_noiseOffset.y += scrollOffset;
            m_noiseOffset.z += scrollOffset;

            m_noise.x = Mathf.PerlinNoise(m_noiseOffset.x, 0f);
            m_noise.y = Mathf.PerlinNoise(m_noiseOffset.x, 1f);
            m_noise.z = Mathf.PerlinNoise(m_noiseOffset.x, 2f);

            m_noise -= Vector3.one * 0.5f;
            m_noise *= m_data.amplitude;
        }
    }
}
