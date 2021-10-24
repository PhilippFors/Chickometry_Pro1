using Audio.Container;
using Audio.Container.MusicContainer;
using Sirenix.OdinInspector;
using UnityEngine;
using Utlities.Locators;

namespace Audio.Music
{
    public class MusicController : MonoBehaviourService
    {
        [SerializeField] private AudioSource source;
        [SerializeField] private bool playOnAwake;

        [Header("Audio Stats")] [SerializeField]
        private MusicContainer currentContainer;

        [SerializeField, ReadOnly] private float currentPlaybackTime;
        [SerializeField, ReadOnly] private float clipLength;

        [Header("Playback Settings")] [SerializeField]
        private bool overrideStartPoint;

        [SerializeField] private bool useCustomTime;

        [SerializeField, DisableIf("useCustomTime")]
        private MusicStartPoints startPoint;

        [SerializeField, EnableIf("useCustomTime")]
        private float startPointTime;

        [Header("Loop Settings")]
        [SerializeField, Tooltip("Checking this loops the entire track from beginning to end.")]
        private bool loopAll;

        [SerializeField, DisableIf("loopAll")] private bool doCustomLoop;
        [SerializeField] private bool useCustomLoopTime;

        [SerializeField, DisableIf("useCustomLoopTime")]
        private MusicStartPoints endPoint;

        [SerializeField, EnableIf("useCustomLoopTime")]
        private float endPointTime;

        private MusicSoundBank CurrentClip => currentContainer.track;

        private void Awake() {
            if (playOnAwake) {
                PlayMusicClip();
            }
        }

        private void Update() {
            source.loop = loopAll;
            currentPlaybackTime = Mathf.Round(source.time);
            
            if (loopAll) {
                doCustomLoop = false;
            }

            if (doCustomLoop) {
                var start = CurrentClip.GetStartPoint(startPoint);
                var end = CurrentClip.GetStartPoint(endPoint);

                if (useCustomLoopTime) {
                    if (useCustomTime) {
                        if (endPointTime < startPointTime) {
                            Debug.LogError("Loop end can't be smaller than loop start", this);
                            return;
                        }
                    }
                    else {
                        if (endPointTime < start) {
                            Debug.LogError("Loop end can't be smaller than loop start", this);
                            return;
                        }
                    }

                    if (currentPlaybackTime >= endPointTime) {
                        SetPlaybackTime();
                    }
                }
                else {
                    if (useCustomTime) {
                        if (end < startPointTime) {
                            Debug.LogError("Loop end can't be smaller than loop start", this);
                            return;
                        }
                    }
                    else {
                        if (end < start) {
                            Debug.LogError("Loop end can't be smaller than loop start", this);
                            return;
                        }
                    }

                    if (currentPlaybackTime >= end) {
                        SetPlaybackTime();
                    }
                }
            }
        }

        [Button("Play music clip")]
        public void PlayMusicClip() {
            source.clip = CurrentClip.GetAudioClip;
            clipLength = CurrentClip.GetAudioClip.length;
            source.Play();

            SetPlaybackTime();
        }

        [Button("Stop music clip")]
        public void StopMusic() {
            source.Stop();
        }

        private void SetPlaybackTime() {
            if (overrideStartPoint) {
                if (useCustomTime) {
                    if (startPointTime > CurrentClip.GetAudioClip.length) {
                        Debug.LogError("Start time is longer than the audio clip length.", this);
                        return;
                    }

                    source.time = startPointTime;
                }
                else {
                    var t = CurrentClip.GetStartPoint(startPoint);
                    if (t > CurrentClip.GetAudioClip.length) {
                        Debug.LogError("Start time is longer than the audio clip length.", this);
                        return;
                    }

                    source.time = t;
                }
            }
            else {
                source.time = 0;
            }
        }
    }
}