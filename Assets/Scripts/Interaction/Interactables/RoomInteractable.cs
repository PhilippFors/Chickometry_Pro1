using Interaction.Items;
using RoomLoop;
using UnityEngine;

namespace Interaction.Interactables
{
    public class RoomInteractable : BasePickUpInteractable
    {
        public Vector3 PreviousPortalOffset { get; set; }
        public Transform currentParent;
        public bool isAbstract;
        public bool isInOriginalRoom = true;
        [HideInInspector] public bool thrown;

        protected RoomPuzzleController roomPuzzle;
        
        public void Init(RoomPuzzleController puzzle) => roomPuzzle = puzzle;

        public override void OnPickup()
        {
            base.OnPickup();

            if (roomPuzzle && isInOriginalRoom) {
                roomPuzzle.RemoveObject(this);
            }

        }

        public override void OnThrow()
        {
            base.OnThrow();

            thrown = true;
            transform.parent = currentParent;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (thrown) {
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
        
        public virtual void Sync(RoomInteractable interactable){}
    }
}