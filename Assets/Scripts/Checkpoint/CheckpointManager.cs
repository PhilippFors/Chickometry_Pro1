using System.Collections.Generic;
using Entities.Player;
using Interactables;
using UsefulCode.Utilities;
using UnityEngine;
using Utlities;

namespace Checkpoints
{
    /// <summary>
    /// Takes in a checkpoint and resets when triggered
    /// </summary>
    public class CheckpointManager : SingletonBehaviour<CheckpointManager>
    {
        private Checkpoint activeCheckopint;
        private List<IResettableBehaviour> resettableBehaviours = new List<IResettableBehaviour>();
        public void SetActiveCheckpoint(Checkpoint checkpoint)
        {
            activeCheckopint = checkpoint;
        }

        public void RegisterBehaviour(IResettableBehaviour b)
        {
            resettableBehaviours.Add(b);
        }

        public void ResetAllBehaviours()
        {
            foreach (var r in resettableBehaviours) {
                r.ResetBehaviour();
            }
        }

        public void ResetSingleBehaviour<T>() where T : Component
        {
            var b = resettableBehaviours.Find(x => x.GetComponent<T>() != null);
            b.ResetBehaviour();
        }

        public void UnregisterBehaviour(IResettableBehaviour b)
        {
            resettableBehaviours.Remove(b);
        }
        
        public void ResetToCheckpoint()
        {
            if (activeCheckopint.resetButton) {
                activeCheckopint.resetButton.Reset();
            }

            var checkPos = activeCheckopint.checkPointPosition;
            if (checkPos) {
                transform.position = checkPos.position;
                transform.rotation = checkPos.rotation;
            }
            else {
                transform.position = activeCheckopint.transform.position;
                transform.rotation = activeCheckopint.transform.rotation;

            }
        }
    }
}