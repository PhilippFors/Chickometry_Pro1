using Sirenix.OdinInspector;
using UnityEngine;

namespace Audio.Container
{
    /// <summary>
    /// Base class for audioclip wrapper
    /// </summary>
    [System.Serializable]
    public abstract class SoundBank
    {
        public abstract AudioClip GetAudioClip { get; }
    }

    public enum MusicStartPoints
    {
        beginning,
        startPoint1,
        startPoint2,
        startPoint3
    }

    [System.Serializable]
    public class MusicSoundBank : SoundBank
    {
        public override AudioClip GetAudioClip => musicClip;

        [SerializeField] private AudioClip musicClip;

        [SerializeField] private float startPoint1;
        [SerializeField] private float startPoint2;
        [SerializeField] private float startPoint3;

        public float GetStartPoint(MusicStartPoints startPoints)
        {
            switch (startPoints)
            {
                case MusicStartPoints.beginning:
                    return 0;
                case MusicStartPoints.startPoint1:
                    return startPoint1;
                case MusicStartPoints.startPoint2:
                    return startPoint2;
                case MusicStartPoints.startPoint3:
                    return startPoint3;
                default:
                    return 0;
            }
        }
    }

    public abstract class SfxSoundBank : SoundBank
    {
        public virtual float GetPitchOffset
        {
            get
            {
                var pitchOffset = 0f;

                if (usePitchOffset)
                {
                    pitchOffset = Random.Range(-minPitchOffset, maxPitchOffset);
                }

                return pitchOffset;
            }
        }

        [SerializeField] private bool usePitchOffset;
        [SerializeField, EnableIf("usePitchOffset")]
        private float minPitchOffset = 0.05f;
        [SerializeField, EnableIf("usePitchOffset")]
        private float maxPitchOffset = 0.05f;
    }

    /// <summary>
    /// Can hold an array of sounds and return a random clip when needed;
    /// </summary>
    [System.Serializable]
    public sealed class RandomSoundBank : SfxSoundBank
    {
        public override AudioClip GetAudioClip => GetRandomClip();

        [SerializeField] private bool excludeLastUsedClipFromChoice;
        [SerializeField] private AudioClip[] audioClips;

        private int prevIndex = 0;

        private AudioClip GetRandomClip()
        {
            var i = 0;
            if (audioClips.Length > 1)
            {
                i = GetRandomIndex();

                if (excludeLastUsedClipFromChoice)
                {
                    while (i == prevIndex)
                    {
                        i = GetRandomIndex();
                    }
                }

                prevIndex = i;
            }

            return audioClips[i];
        }

        private int GetRandomIndex() => Random.Range(0, audioClips.Length);
    }

    /// <summary>
    /// Holds a single clip and can return it.
    /// </summary>
    [System.Serializable]
    public sealed class SingleSoundBank : SfxSoundBank
    {
        public override AudioClip GetAudioClip => audioClip;

        [SerializeField] private AudioClip audioClip;
    }
}
