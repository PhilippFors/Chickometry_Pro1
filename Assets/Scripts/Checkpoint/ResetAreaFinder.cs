﻿using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Checkpoints
{
    /// <summary>
    /// Finds all IResettable types in a defined area and caches them
    /// </summary>
    public class ResetAreaFinder : MonoBehaviour
    {
        [SerializeField] private LayerMask layerMask;
        private List<IResettableItem> resettables = new List<IResettableItem>();
        private void Start()
        {
            Find();
        }

        [Button]
        private void Find()
        {
            var objs = Physics.OverlapBox(transform.position, transform.localScale / 2, transform.rotation, layerMask);

            foreach (var s in objs) {
                var resettable = s.GetComponentsInParent<IResettableItem>();
                foreach (var r in resettable) {
                    if (!resettables.Contains(r)) {
                        resettables.Add(r);
                    }
                }
            }
        }

        public void Reset()
        {
            foreach (var r in resettables) {
                r.ResetToCheckpoint();
            }
        }
        
        private void OnDrawGizmos()
        {
            var col = transform.localScale;
            Gizmos.color = new Color(255, 100,100, 0.1f);
            Gizmos.DrawCube(transform.position, col);
            
            Gizmos.color = new Color(255, 100,100, 0.5f);
            Gizmos.DrawWireCube(transform.position, col);
        }
    }
}