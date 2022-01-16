using Interaction.Interactables;
using UnityEngine;

namespace Interaction.Items.ProgressionItems
{
    public class KeyMergeController : MonoBehaviour
    {
        [SerializeField] private RoomLoopKey normalKey;
        [SerializeField] private RoomLoopKey abstractKey;
        [SerializeField] private Transform mainKeyTransformAbstract;
        [SerializeField] private Transform mainKeyTransformNormal;
        [SerializeField] private SocketInteractable[] abstractSockets;
        [SerializeField] private SocketInteractable[] normalSockets;
        [SerializeField] private RoomSnapInteractable[] abstractKeyParts;
        [SerializeField] private RoomSnapInteractable[] normalKeyParts;

        private bool[] keys = new bool[3];

        private void Start()
        {
            foreach (var socket in abstractSockets) {
                socket.socketActivated += SocketEvent;
            }

            foreach (var socket in normalSockets) {
                socket.socketActivated += SocketEvent;
            }
        }

        private void OnDisable()
        {
            foreach (var socket in abstractSockets) {
                socket.socketActivated -= SocketEvent;
            }

            foreach (var socket in normalSockets) {
                socket.socketActivated -= SocketEvent;
            }
        }

        private bool CheckForAllKeys()
        {
            foreach (var b in keys) {
                if (!b) {
                    return false;
                }
            }

            return true;
        }

        private void SocketEvent(SocketInteractable socket)
        {
            for (int i = 0; i < abstractSockets.Length; i++) {
                if (abstractSockets[i].gameObject.activeSelf && abstractSockets[i] == socket ||
                    normalSockets[i].gameObject.activeSelf && normalSockets[i] == socket) {
                    keys[i] = true;
                    AttachToSocket(normalSockets[i], normalKeyParts[i]);
                    AttachToSocket(abstractSockets[i], abstractKeyParts[i]);

                    DisableKeys(normalKeyParts[i].gameObject, mainKeyTransformNormal);
                    DisableKeys(abstractKeyParts[i].gameObject, mainKeyTransformAbstract);
                }

                // if (abstractSockets[i] == socket) {
                //     abstractKeyParts[i].thrown = true;
                //     abstractKeyParts[i].ReturnObject();
                // }
                // else if (normalSockets[i] == socket) {
                //     normalKeyParts[i].thrown = true;
                //     normalKeyParts[i].ReturnObject();
                // }
            }

            if (CheckForAllKeys()) {
                ReleaseKey();
            }
        }


        private void DisableKeys(GameObject key, Transform newParent)
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
            normalKey.canBePickedUp = true;
            normalKey.GetComponent<Rigidbody>().isKinematic = false;
            normalKey.GetComponent<Collider>().enabled = true;

            abstractKey.canBePickedUp = true;
            abstractKey.GetComponent<Rigidbody>().isKinematic = false;
            abstractKey.GetComponent<Collider>().enabled = true;
        }
    }
}