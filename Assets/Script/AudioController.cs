using UnityEngine;
using UnityEngine.Serialization;

namespace Script
{
    /// <summary>
    /// This class is responsible for controlling all audio sources in the game.
    /// </summary>
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