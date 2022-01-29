using Entities.Companion;
using UnityEngine;
using UsefulCode.Utilities;

namespace Checkpoints
{
    public class GudrunNestManager : SingletonBehaviour<GudrunNestManager>
    {
        public Transform gudrun;

        public override void Awake()
        {
            gudrun = FindObjectOfType<Gudrun>().transform;
        }
    }
}