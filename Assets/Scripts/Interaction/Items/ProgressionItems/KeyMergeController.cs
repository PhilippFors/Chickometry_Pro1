using Interaction.Interactables;
using UnityEngine;

namespace Interaction.Items.ProgressionItems
{
    public class KeyMergeController : MonoBehaviour
    {
        [SerializeField] private SocketInteractable[] abstractSockets;
        [SerializeField] private SocketInteractable[] normalSockets;

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
                if (abstractSockets[i].gameObject.activeSelf && abstractSockets[i] == socket) {
                    keys[i] = true;
                    normalSockets[i].gameObject.SetActive(false);

                }
            }
        
            for (int i = 0; i < normalSockets.Length; i++) {
                if (normalSockets[i].gameObject.activeSelf && normalSockets[i] == socket) {
                    keys[i] = true;
                    abstractSockets[i].gameObject.SetActive(false);
                }
            }
        
            if (CheckForAllKeys()) {
                ReleaseKey();
            }
        }

        private void ReleaseKey()
        {
            //Todo: release the peen
        }
    }
}