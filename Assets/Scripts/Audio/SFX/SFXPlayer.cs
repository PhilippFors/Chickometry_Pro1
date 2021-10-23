using UnityEngine;
using Utlities;

namespace Audio.SFX
{
    /// <summary>
    /// Simple SFX player which plays any audio clip you tell it to play.
    /// Clips can have random pitch modulation if desired.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class SFXPlayer : MonoBehaviour
    {
        [SerializeField] private float minRandomPitch = 0.95f;
        [SerializeField] private float maxRandomPtich = 1.05f;
        
        private AudioSource audioSource;
        private GlobalSFXController _globalSfx;
        
        private void Start()
        {
            _globalSfx = ServiceLocator.Get<GlobalSFXController>();
            audioSource = GetComponent<AudioSource>();
        }

        public void PlayAudioClip(AudioClip clip, bool randomPitch = false)
        {
            if (randomPitch)
            {
                RandomizePitch();
            }
            audioSource.PlayOneShot(clip);
        }

        private void RandomizePitch()
        {
            audioSource.pitch = Random.Range(minRandomPitch, maxRandomPtich);
        }
    }
}
