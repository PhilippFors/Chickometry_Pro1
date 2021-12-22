using System;
using UnityEngine;

namespace Utilities
{
    public class LayerChangeOverride : MonoBehaviour
    {
        public int defaultLayer;
        public int overrideLayer = -1;

        private void Awake()
        {
            defaultLayer = gameObject.layer;
        }
    }
}