using UnityEngine;
using DG.Tweening;

namespace Interaction.Interactables
{
    public class Door : MonoBehaviour
    {
        private Vector3 startingPos;
        private Vector3 endPos;
        public Vector3 movingVector;
        private Rigidbody rb;

        void Start()
        {
            startingPos = transform.position;
            endPos = startingPos + movingVector;
            rb = gameObject.GetComponent<Rigidbody>();
        }
        
        public void OpenDoor()
        {
            rb.useGravity = false;
            transform.DOMove(endPos, 1);
        }

        public void CloseDoor()
        {
            // transform.DOMove(startingPos, 1);
            rb.useGravity = true;
        }
    }
}