using System.Collections;
using System.Linq;
using Interaction.Interactables;
using RoomLoop;
using UnityEngine;

namespace Interactables.Util
{
    public class TimedPlantActivator : MonoBehaviour
    {
        [SerializeField] private Collider[] activationTrigger;
        [SerializeField] private float time;
        
        private bool canActivate;

        public void ActivateTimer()
        {
            StartCoroutine(Timer());
        }

        private IEnumerator Timer()
        {
            canActivate = true;
            yield return new WaitForSeconds(time);
            canActivate = false;
        }

        private void OnTriggerStay(Collider other)
        {
            if (activationTrigger.Contains(other) && canActivate) {
                var roomInteractable = other.GetComponentInParent<RoomInteractable>();
                var plant = (PlantRoomInteractable) roomInteractable;
                plant.EnablePlant();
                enabled = false;
            }
        }
    }
}