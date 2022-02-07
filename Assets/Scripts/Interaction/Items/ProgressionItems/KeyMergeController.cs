using Interaction.Interactables;
using UnityEngine;

namespace Interaction.Items.ProgressionItems
{
    public class KeyMergeController : MonoBehaviour
    {
        [SerializeField] private RoomLoopKey abstractKey;
        [SerializeField] private RoomLoopKey normalKey;
        [SerializeField] private Transform mainKeyTransformAbstract;
        // [SerializeField] private Transform mainKeyTransformNormal;
        [SerializeField] private SocketInteractable[] abstractSockets;
        // [SerializeField] private SocketInteractable[] normalSockets;
        [SerializeField] private RoomSnapInteractable[] abstractKeyParts;
        // [SerializeField] private RoomSnapInteractable[] normalKeyParts;

        private bool[] filledKeys = new bool[3];

        private void Start()
        {
            foreach (var socket in abstractSockets) {
                socket.socketActivated += SocketEvent;
            }

            // foreach (var socket in normalSockets) {
            //     socket.socketActivated += SocketEvent;
            // }
        }

        private void OnDisable()
        {
            foreach (var socket in abstractSockets) {
                socket.socketActivated -= SocketEvent;
            }

            // foreach (var socket in normalSockets) {
            //     socket.socketActivated -= SocketEvent;
            // }
        }

        private bool CheckForAllKeys()
        {
            foreach (var b in filledKeys) {
                if (!b) {
                    return false;
                }
            }

            return true;
        }

        private void SocketEvent(SocketInteractable socket)
        {
            for (int i = 0; i < abstractSockets.Length; i++) {
                if (abstractSockets[i].gameObject.activeSelf && abstractSockets[i] == socket) {
                    filledKeys[i] = true;
                    AttachToSocket(abstractSockets[i], abstractKeyParts[i]);
                    DisableKey(abstractKeyParts[i].gameObject, mainKeyTransformAbstract);
                }
            }

            if (CheckForAllKeys()) {
                ReleaseKey();
            }
        }


        private void DisableKey(GameObject key, Transform newParent)
        {
            key.GetComponent<Rigidbody>().isKinematic = true;
            key.GetComponent<Collider>().enabled = false;
            key.transform.parent = newParent;
        }

        private void AttachToSocket(SocketInteractable socket, RoomSnapInteractable key)
        {
            socket.AttachObject(key);
            socket.gameObject.SetActive(false);
        }

        private void ReleaseKey()
        {
            //Todo: Soundeffects, etc.?
            normalKey.gameObject.SetActive(true);
            normalKey.canBePickedUp = true;
            normalKey.GetComponent<Rigidbody>().isKinematic = false;
            normalKey.GetComponent<Collider>().enabled = true;

            abstractKey.canBePickedUp = true;
            abstractKey.GetComponent<Rigidbody>().isKinematic = false;
            abstractKey.GetComponent<Collider>().enabled = true;
        }
    }
}