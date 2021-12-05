using Interaction.Items;
using UnityEngine;

namespace RoomLoop
{
    public class RoomInteractable : BasePickUpInteractable
    {
        public Transform currentParent;
        public bool isAbstract;
        public bool isInOriginalRoom = true;
        
        private RoomPuzzle roomPuzzle;
        private bool thrown;
        
        public void Init(RoomPuzzle puzzle) => roomPuzzle = puzzle;

        public override void OnThrow()
        {
            base.OnThrow();
            thrown = true;
            transform.parent = currentParent;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (thrown && isInOriginalRoom) {
                roomPuzzle.UpdatePosition(this);
            }
            thrown = false;
        }
    }
}