using System.Collections.Generic;
using ObjectAbstraction.Wireframe;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utlities.MeshUtils
{
    public class SceneMeshDataBuilder : MonoBehaviour
    {
        private HashSet<WireframeIdentifier> sceneCache = new HashSet<WireframeIdentifier>();
        private static int maxDepth = 4;
        private static int depth;
        [Button("Set Meshdatabuilders")]
#if UNITY_EDITOR
        void OnValidate()
        {

            var activeScene = SceneManager.GetActiveScene();
            var root = activeScene.GetRootGameObjects();
            foreach (var obj in root) {
                CheckChildren(obj);
            }

        }

        private void CheckChildren(GameObject obj)
        {
            depth++;
            if (obj.transform.childCount > 0 && depth <= maxDepth) {
                int i = 0;
                while (i < obj.transform.childCount) {
                    AddMeshBuilder(obj.transform.GetChild(i));
                    CheckChildren(obj.transform.GetChild(i).gameObject);
                    i++;
                }
            }
            else {
                AddMeshBuilder(obj.transform);
            }

            if (depth > 0) {
                depth--;
            }
        }

        private void AddMeshBuilder(Transform obj)
        {
            var wireframe = obj.GetComponent<WireframeIdentifier>();
            var meshDataBuilder = obj.GetComponent<MeshDataBuilder>();
            if (wireframe && !meshDataBuilder && !sceneCache.Contains(wireframe)) {
                sceneCache.Add(wireframe);
                var m = wireframe.gameObject.AddComponent<MeshDataBuilder>();
                m.GenerateMeshData();
            }
            else if (sceneCache.Contains(wireframe) && !wireframe && meshDataBuilder) {
                sceneCache.Remove(wireframe);
                if (meshDataBuilder) {
                    Destroy(meshDataBuilder);
                }
            }
        }

        [Button]
        private void Clear() => sceneCache.Clear();
            
#endif
            
        private void Awake()
        {
            Destroy(gameObject);
        }
    }
}