using Interaction.Items;
using ObjectAbstraction;
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
        private bool isGrounded;
        private BasePickUpInteractable pickUpInteractable;
        private Rigidbody rb;
        private IModelChanger switcher;
        private NavData navData;
        private Vector3[] currentPoints = new Vector3[0];
        private int pointIndex = 0;

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
            isGrounded = Physics.CheckSphere(transform.position + new Vector3(0, 0.09f, 0), 0.2f, LayerMask.GetMask("Default", "Interactable"));

            if (!isGrounded || switcher.IsAbstract || pickUpInteractable.IsPickedUp || Vector3.Distance(transform.position, player.position) < stopDistance) {
                return;
            }

            UpdatePath();
            
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

        private void UpdatePath() {
            navData.source = transform.position;
            navData.target = player.position;

            if (navData.pathStatus != PathStatus.PathPending) {
                AiPathManager.RequestPath(navData);
            }

            if (navData.pathStatus == PathStatus.PathReady) {
                currentPoints = navData.Path;
                pointIndex = 0;
                navData.pathStatus = PathStatus.NoPath;
            }
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