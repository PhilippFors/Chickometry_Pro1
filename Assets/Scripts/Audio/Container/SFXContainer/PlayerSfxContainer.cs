using UnityEngine;

namespace Audio.Container.SFXContainer
{
    /// <summary>
    /// Container for player sfx.
    /// </summary>
    [CreateAssetMenu(fileName = "PlayerSfxContainern.asset", menuName = "Audio/PlayerSfxContainer")]
    public sealed class PlayerSfxContainer : BaseSoundContainer
    {
        public RandomSoundBank footsteps; //example
    }
}
