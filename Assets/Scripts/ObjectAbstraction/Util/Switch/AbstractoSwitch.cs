using System.Collections;
using System.Collections.Generic;
using Interaction.Interactables;
using ObjectAbstraction.ModelChanger;
using UnityEngine;

namespace ObjectAbstraction
{
    /// <summary>
    /// Toggles the abstraction level of every object in a defined area.
    /// </summary>
    public class AbstractoSwitch : BaseInteractable
    {
        [SerializeField] private float timer;
        [SerializeField] private bool useTimer;
        [SerializeField] private bool isSwitchedOn;
        
        private List<IModelChanger> objectsInArea = new List<IModelChanger>();

        public override void OnInteract()
        {
            ToggleSwitch();
        }

        private void ToggleSwitch()
        {
            foreach (var s in objectsInArea) {
                s.ToggleModels();
            }

            isSwitchedOn = !isSwitchedOn;

            if (useTimer && isSwitchedOn) {
                StartCoroutine(SwitchTimer());
            }
        }

        private IEnumerator SwitchTimer()
        {
            yield return new WaitForSeconds(timer);
            ToggleSwitch();
        }

        public void AddModelSwitcher(IModelChanger obj)
        {
            if (objectsInArea.Contains(obj)) {
                return;
            }

            objectsInArea.Add(obj);
        }
    }
}