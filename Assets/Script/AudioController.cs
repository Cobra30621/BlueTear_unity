using UnityEngine;
using UnityEngine.Serialization;

namespace Script
{
    public class AudioController : MonoBehaviour
    {
        public AudioSource[] audioSources;

        public void FadeVolume(float volume)
        {
            foreach (var audioSource in audioSources)
            {
                if (audioSource.volume > volume)
                {
                    audioSource.volume = volume;
                }
            }
        }
    }
}