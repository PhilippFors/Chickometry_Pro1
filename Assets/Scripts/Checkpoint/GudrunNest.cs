using System;
using Interaction.Interactables;
using ObjectAbstraction.ModelChanger;
using UnityEngine;

namespace Checkpoints
{
    [RequireComponent(typeof(BoxCollider))]
    public class GudrunNest : BaseInteractable
    {
        [SerializeField] private Transform resetPosition;
        [SerializeField] private ParticleSystem nestParticles;
        private bool isActive;

        private void Start()
        {
            SetNestParticles();
        }

        public override void OnInteract()
        {
            if (!isActive) {
                return;
            }
            
            var gudrun = GudrunNestManager.Instance.gudrun;
            var modelChanger = gudrun.GetComponent<AdvModelChanger>();
            if (modelChanger) {
                if (!modelChanger.IsAbstract) {
                    modelChanger.ToggleModels();
                }
            }

            gudrun.position = resetPosition.position;
        }

        public void SetActive(bool active)
        {
            isActive = active;
            SetNestParticles();
        }

        private void SetNestParticles()
        {
            if (isActive) {
                nestParticles.Play();
            }
            else {
                nestParticles.Stop();
            }
        }
    }
}