using Interaction.Items;
using ObjectAbstraction;
using ObjectAbstraction.ModelChanger;
using UnityEngine;
using Utilities.AI;

namespace Entities.Companion
{
    /// <summary>
    /// Lovely hen that follows the player when it's not abstracted.
    /// </summary>
    public class Gudrun : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private float stopDistance = 2f;
        [SerializeField] private float speed = 4f;
        [SerializeField] private Animator animator;
        
        private bool isGrounded;
        private BasePickUpInteractable pickUpInteractable;
        private Rigidbody rb;
        private IModelChanger switcher;
        private NavData navData;
        private Vector3[] currentPoints = new Vector3[0];
        private int pointIndex;

        private void Awake() {
            if (!player) {
                var obj = GameObject.FindWithTag("Player");
                player = obj.transform;
            }
            switcher = GetComponent<IModelChanger>();
            rb = GetComponent<Rigidbody>();
            pickUpInteractable = GetComponent<BasePickUpInteractable>();
            navData = new NavData();
        }

        private void Update()
        {
            var cols = Physics.OverlapSphere(transform.position + new Vector3(0, 0.09f, 0), 0.2f,
                LayerMask.GetMask("Default", "Interactable"));
            if (cols.Length > 1) {
                isGrounded = true;
            }
            else {
                isGrounded = false;
            }
            // isGrounded = Physics.CheckSphere(transform.position + new Vector3(0, 0.09f, 0), 0.2f, LayerMask.GetMask("Default", "Interactable"));

            if (!isGrounded || switcher.IsAbstract || pickUpInteractable.IsPickedUp || Vector3.Distance(transform.position, player.position) < stopDistance) {
                animator.SetBool("isRunning", false);
                return;
            }

            animator.SetBool("isRunning", true);
            navData.UpdatePath(transform.position, player.position, ref currentPoints);
            
            if (navData.pathStatus == PathStatus.PathReady) {
                pointIndex = 0;
            }
            
            if (currentPoints.Length == 0) {
                return;
            }

            var transf = transform.position;
            if (HasNextPoint() && Vector3.Distance(transf, currentPoints[pointIndex + 1]) < 0.4f) {
                pointIndex++;
            }

            var lookAt = GetNextPoint();

            transform.LookAt(lookAt);
            rb.MovePosition(transform.position + (transform.forward * (speed * Time.deltaTime)));
        }

        private Vector3 GetNextPoint() {
            Vector3 pos;

            if (HasNextPoint()) {
                pos = currentPoints[pointIndex + 1];
            }
            else {
                pos = player.position;
            }

            pos.y = transform.position.y;
            return pos;
        }

        private bool HasNextPoint() => pointIndex + 1 < currentPoints.Length;
    }
}