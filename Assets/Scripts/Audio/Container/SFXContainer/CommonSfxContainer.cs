using Audio.Container;
using UnityEngine;

namespace Audio.SFX.SFXContainer
{
    /// <summary>
    /// SFX Container for common sfx.
    /// </summary>
    [CreateAssetMenu(fileName = "CommonSfxContainer.asset", menuName = "Audio/CommonSfxContainer")]
    public sealed class CommonSfxContainer : BaseSoundContainer
    {
        public RandomSoundBank impactSounds;
    }
}