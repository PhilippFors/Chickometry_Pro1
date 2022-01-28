using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Utilities.Math;
using UnityEngine;

namespace Visual
{
    /// <summary>
    /// Generate a set amount of cubes in a defined volume
    /// </summary>
    public class VolumeCubeGenerator : MonoBehaviour
    {
        public List<Vector3> CubePositions => cubePositions;
        public int cubeAmount;
        public BoxCollider bounds;
        public BoxCollider excludeBounds;
        [SerializeField] private List<Vector3> cubePositions = new List<Vector3>();

        [Button]
        private void Generate()
        {
            KillChildren();
            for (int i = 0; i < cubeAmount; i++) {
                var pos = MathUtils.FindRandomInArea(gameObject, bounds);
                if (excludeBounds) {
                    while (excludeBounds.bounds.Contains(pos)) {
                        pos = MathUtils.FindRandomInArea(gameObject, bounds);
                    }
                }

                pos = transform.InverseTransformPoint(pos);
                cubePositions.Add(pos);
                // var cube = Instantiate(cubePrefab, pos, Quaternion.identity, bounds.transform);
                // cube.transform.localScale = new Vector3(cubeScale, cubeScale, cubeScale);
            }
        }

        private void KillChildren()
        {
            cubePositions.Clear();
            // var cubes = bounds.GetComponentsInChildren<MeshRenderer>();
            // for (int i = 0; i < cubes.Length; i++) {
            //     if (!Application.isPlaying) {
            //         DestroyImmediate(cubes[i].gameObject);
            //     }
            //     else {
            //         Destroy(cubes[i].gameObject);
            //     }
            // }
        }
    }
}