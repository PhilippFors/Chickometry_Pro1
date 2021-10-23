using System.Collections.Generic;
using Audio.Container;
using Audio.Container.SFXContainer;
using Audio.SFX.SFXContainer;
using UnityEngine;
using UnityEngine.Audio;
using Utlities.Locators;

namespace Audio.SFX
{
    /// <summary>
    /// Has some global SFX settings. Has a reference to all SFX containers.
    /// </summary>
    public class GlobalSFXController : MonoBehaviourService
    {
        public PlayerSfxContainer PlayerSfx => playerSfx;
        public CommonSfxContainer CommonSfx => commonSfx;
        public WeaponSfxContainer WeaponSfx => weaponSfx;

        [SerializeField] private PlayerSfxContainer playerSfx;
        [SerializeField] private CommonSfxContainer commonSfx;
        [SerializeField] private WeaponSfxContainer weaponSfx;

        [SerializeField] private AudioMixerGroup otherGroup;
        
        private Dictionary<GameObject, AudioSource> audioInstances = new Dictionary<GameObject, AudioSource>();

        public void AddAudioSource(GameObject obj, AudioSource audioInstance)
        {
            audioInstances.Add(obj, audioInstance);
        }

        public void RemoveAudioSource(GameObject obj)
        {
            audioInstances.Remove(obj);
        }

        public void PlaySFX(GameObject source, SfxSoundBank bank, bool usePtichoffset)
        {
            var pitchoffset = usePtichoffset ? bank.GetPitchOffset : 0;
            PlaySFX(source, bank.GetAudioClip, pitchoffset);
        }

        public void PlaySFX(GameObject source, AudioClip clip, float pitchOffset = 0, float volumeScale = 1.0f)
        {
            AudioSource audioSource = GetOrAddAudioSource(source);
            if (audioSource != null)
            {
                audioSource.pitch = 1.0f + pitchOffset;
                audioSource.PlayOneShot(clip, volumeScale);
            }
            else
            {
                Debug.LogWarning(string.Format("Couldn't find audio instance for {0}", source.name));
            }
        }

        private AudioSource GetAudioSource(GameObject obj)
        {
            AudioSource audioSource = null;
            audioInstances.TryGetValue(obj, out audioSource);
            return audioSource;
        }

        // I didn't make much about the audio souces configurable, this is where you'd change the defaults
        private AudioSource GetOrAddAudioSource(GameObject obj)
        {
            AudioSource audioSource = GetAudioSource(obj);

            if (audioSource == null)
            {
                audioSource = obj.GetComponent<AudioSource>();
                if (!audioSource)
                {
                    audioSource = obj.AddComponent<AudioSource>();
                }

                audioSource.playOnAwake = false;
                audioSource.spatialize = true;
                audioSource.spatialBlend = 0.8f;
                audioSource.volume = 1f;
                audioSource.loop = false;
                audioSource.minDistance = 4f;
                audioSource.maxDistance = 100f;
                audioSource.rolloffMode = AudioRolloffMode.Linear;
                audioSource.outputAudioMixerGroup = otherGroup;
                audioSource.dopplerLevel = 1f;
                AddAudioSource(obj, audioSource);
            }

            return audioSource;
        }
    }
}