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
        public float yIncrement = 1;
        public float minDist = 1f;
        public float yRange = 0.5f;
        public BoxCollider bounds;
        public BoxCollider excludeBounds;
        [SerializeField] private List<Vector3> cubePositions = new List<Vector3>();
        
        [Button]
        private void Generate()
        {
            KillChildren();
            bounds.enabled = true;
            if (excludeBounds) {
                excludeBounds.enabled = true;
            }

            var boundsY = bounds.size.y;
            var iterations = Mathf.RoundToInt(boundsY / yIncrement);
            var cubesPerIteration = Mathf.RoundToInt(cubeAmount / iterations);
            var currentY = bounds.transform.position.y + boundsY / 2 - yIncrement / 2;

            for (int i = 0; i < iterations; i++) {
                var localCubeList = new List<Vector3>();
                for (int j = 0; j < cubesPerIteration; j++) {
                    var pos = MathUtils.FindRandomInArea(gameObject, bounds, currentY, yRange);
                    var tries = 0;
                    if (excludeBounds) {
                        while (excludeBounds.bounds.Contains(pos) || (CubeIsNearOthers(localCubeList, pos, minDist) && tries < 1200)) {
                            pos = MathUtils.FindRandomInArea(gameObject, bounds, currentY, yRange);
                            tries++;
                        }
                    }
                    else {
                        while (CubeIsNearOthers(localCubeList, pos, minDist) && tries < 1200) {
                            pos = MathUtils.FindRandomInArea(gameObject, bounds, currentY, yRange);
                            tries++;
                        }
                    }
                    
                    localCubeList.Add(pos);
                    pos = transform.InverseTransformPoint(pos);
                    cubePositions.Add(pos);
                }
                currentY -= yIncrement;
            }

            bounds.enabled = false;
            if (excludeBounds) {
                excludeBounds.enabled = false;
            }
        }
        
        private bool CubeIsNearOthers(List<Vector3> other, Vector3 pos, float minDistance)
        {
            if (other.Count == 0) {
                return false;
            }
            
            foreach (var tr in other) {
                if (Vector3.Distance(tr, pos) < minDistance) {
                    return true;
                }
            }

            return false;
        }
        
        private void KillChildren()
        {
            cubePositions.Clear();
        }
    }
}