using Interactables;
using UnityEngine;

namespace Interaction.Interactables
{
    public class WaterPump : SocketInteractable
    {
        [SerializeField] private ParticleSystem waterParticlesPlay;

        private void Start()
        {
            isInteractable = false;
        }

        public override void OnInteract()
        {
            if (!isInteractable) {
                return;
            }
            
            waterParticlesPlay.Play();
        }

        public override bool Activate(RoomSnapInteractable interactable, InteractionManager manager)
        {
            base.Activate(interactable, manager);
            isInteractable = true;
            return true;
        }
    }
}