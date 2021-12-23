using Sirenix.OdinInspector;
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
                var pos = FindRandomInArea();
                if (excludeBounds) {
                    while (excludeBounds.bounds.Contains(pos)) {
                        pos = FindRandomInArea();
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

        private Vector3 FindRandomInArea()
        {
            return new Vector3(
                Random.Range(
                    gameObject.transform.position.x - gameObject.transform.localScale.x * bounds.size.x * 0.5f,
                    gameObject.transform.position.x + gameObject.transform.localScale.x * bounds.size.x * 0.5f),
                Random.Range(
                    gameObject.transform.position.y - gameObject.transform.localScale.y * bounds.size.y * 0.5f,
                    gameObject.transform.position.y + gameObject.transform.localScale.y * bounds.size.y * 0.5f),
                Random.Range(
                    gameObject.transform.position.z - gameObject.transform.localScale.z * bounds.size.z * 0.5f,
                    gameObject.transform.position.z + gameObject.transform.localScale.z * bounds.size.z * 0.5f)
            );
        }
    }
}