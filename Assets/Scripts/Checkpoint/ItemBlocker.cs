using Interactables;
using Interaction.Items;
using UnityEngine;

namespace Checkpoints
{
    public class ItemBlocker : MonoBehaviour
    {
        [SerializeField] private BoxCollider blocker;

        private int itemBlockerLayer;

        private void Start()
        {
            itemBlockerLayer = gameObject.layer;
        }

        private void OnTriggerEnter(Collider other)
        {
            var interactable = other.GetComponentInParent<BasePickUpInteractable>();
            if (GudrunNestManager.Instance.gudrun.GetComponent<BasePickUpInteractable>() == interactable) {
                gameObject.layer = 0;
                blocker.enabled = false;
            }
        }
        
        private void OnTriggerStay(Collider other)
        {
            var interactable = other.GetComponentInParent<BasePickUpInteractable>();
            if (GudrunNestManager.Instance.gudrun.GetComponent<BasePickUpInteractable>() == interactable) {
                return;
            }
            if (interactable) {
                var player = interactable.GetComponentInParent<InteractionManager>();
                if (player) {
                    player.ReleaseObject(true);
                    var point = other.ClosestPoint(interactable.transform.position);
                    var dirToObject = interactable.transform.position - point;
                    interactable.GetComponent<Rigidbody>().velocity += dirToObject * 5f;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            gameObject.layer = itemBlockerLayer;
            blocker.enabled = true;
        }
    }
}