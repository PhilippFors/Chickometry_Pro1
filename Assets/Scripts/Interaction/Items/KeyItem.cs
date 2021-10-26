using UnityEngine;

namespace Interaction.Items
{
    /// <summary>
    /// An item that is literally a key to a door or a similiar use case.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class KeyItem : BasePickUpInteractable
    {
        [SerializeField] private GameObject lockObject;
        public GameObject currentUsableLock;

        public override void OnInteract()
        {
            if (lockObject == currentUsableLock)
            {
                Debug.Log("Yo, that key do kinda fit");
            }
        }
    }
}
