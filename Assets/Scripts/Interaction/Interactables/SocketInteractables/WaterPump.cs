using Interactables;
using Interactables.Util;
using UnityEngine;

namespace Interaction.Interactables
{
    public class WaterPump : SocketInteractable
    {
        [SerializeField] private ParticleSystem waterParticlesPlay;
        [SerializeField] private TimedPlantActivator activator;

        public override void OnInteract()
        {
            if (!isInteractable) {
                return;
            }
            
            waterParticlesPlay.Play();
            
            if (!activator) {
                return;
            }
            activator.ActivateTimer();
        }

        public override bool Activate(RoomSnapInteractable interactable, InteractionManager manager)
        {
            base.Activate(interactable, manager);
            isInteractable = true;
            return true;
        }
    }
}