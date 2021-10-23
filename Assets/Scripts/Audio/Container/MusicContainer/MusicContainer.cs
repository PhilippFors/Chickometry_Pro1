using Audio.Container.SFXContainer;
using Audio.SFX.SFXContainer;
using UnityEngine;

namespace Audio.Container.MusicContainer
{
    [CreateAssetMenu(fileName = "MusicContainer", menuName = "Audio/Music/Music Container")]
    public class MusicContainer : BaseSoundContainer
    {
        public MusicSoundBank track;
    }
}