﻿using ECM2.Characters;
using UnityEngine;

namespace Checkpoints
{
    /// <summary>
    /// Triggers a reset when player gets to undesired areas
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    public class OutOfBoundsTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<FirstPersonCharacter>()) {
                CheckpointManager.Instance.ResetToCheckpoint();
            }
        }
    }
}