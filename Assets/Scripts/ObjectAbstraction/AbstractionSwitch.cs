using System.Collections.Generic;
using Interaction.Interactables;
using UnityEngine;

namespace ObjectAbstraction
{
    /// <summary>
    /// Toggles the abstraction level of every object in a defined area.
    /// </summary>
    public class AbstractionSwitch : BaseInteractable
    {
        [SerializeField] private bool isSwitchedOn;
        [SerializeField] private List<ModelSwitcher> objectsInArea = new List<ModelSwitcher>();

        private void Awake() {
            objectsInArea = new List<ModelSwitcher>();
        }

        private void ToggleSwitch()
        {
            foreach (var s in objectsInArea)
            {
                s.ToggleModels();
            }

            isSwitchedOn = !isSwitchedOn;
        }

        public override void OnUse()
        {
            ToggleSwitch();
        }

        public void AddModelSwitcher(ModelSwitcher obj)
        {
            if (objectsInArea.Contains(obj))
            {
                return;
            }
            objectsInArea.Add(obj);
        }
    }
}