using Interaction.Items;
using RoomLoop.Portal;
using UnityEngine;

namespace RoomLoop
{
    public class RoomInteractable : BasePickUpInteractable, IPortalTraveller
    {
        public Vector3 PreviousPortalOffset { get; set; }
        public virtual bool CanTravel => canTeleport;
        public Transform currentParent;
        public bool isAbstract;
        public bool isInOriginalRoom = true;
        [HideInInspector] public bool thrown;

        [SerializeField] protected bool canTeleport;
        
        private RoomPuzzleController roomPuzzle;
        
        public void Init(RoomPuzzleController puzzle) => roomPuzzle = puzzle;

        public override void OnPickup()
        {
            base.OnPickup();

            if (isInOriginalRoom) {
                roomPuzzle.RemoveObject(this);
            }

            canTeleport = false;
        }

        public override void OnThrow()
        {
            base.OnThrow();

            thrown = true;
            canTeleport = true;
            transform.parent = currentParent;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.transform.CompareTag("Floor") && thrown) {
                ReturnObject();
            }

            thrown = false;
        }

        public void ReturnObject()
        {
            if (isInOriginalRoom) {
                roomPuzzle.ReturnObject(this);
            }

            if (thrown && isInOriginalRoom) {
                roomPuzzle.UpdatePosition(this);
            }

            thrown = false;
        }

        public void Teleport(Transform inPortal, Transform outPortal, Vector3 pos, Quaternion rot, Vector3 velocity)
        {
            transform.position = pos;
            transform.rotation = rot;
            rb.velocity = velocity;
        }

        public virtual void MakeInvisible()
        {
        }

        public virtual void MakeVisible()
        {
        }
    }
}