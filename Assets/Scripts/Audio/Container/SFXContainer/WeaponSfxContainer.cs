using UnityEngine;

namespace Audio.Container.SFXContainer
{
    /// <summary>
    /// Container for weapon sfx.
    /// </summary>
    [CreateAssetMenu(fileName = "WeaponSfxContainer.asset", menuName = "Audio/WeaponSfxContainer")]
    public sealed class WeaponSfxContainer : BaseSoundContainer
    {
        public SingleSoundBank assault_1;
    }
}
