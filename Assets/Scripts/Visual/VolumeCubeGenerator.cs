using Sirenix.OdinInspector;
using Utilities.Math;
using UnityEngine;

namespace Visual
{
    public class VolumeCubeGenerator : MonoBehaviour
    {
        public int cubeAmount;
        public BoxCollider bounds;
        public GameObject cubePrefab;
        public BoxCollider excludeBounds;

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

                Instantiate(cubePrefab, pos, Quaternion.identity, bounds.transform).GetComponent<MeshRenderer>();
            }
        }

        private void KillChildren()
        {
            var cubes = bounds.GetComponentsInChildren<MeshRenderer>();
            for (int i = 0; i < cubes.Length; i++) {
                if (!Application.isPlaying) {
                    DestroyImmediate(cubes[i].gameObject);
                }
                else {
                    Destroy(cubes[i].gameObject);
                }
            }
        }
    }
}