using System.Collections.Generic;
using ObjectAbstraction.Wireframe;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utlities.MeshUtils
{
    public class SceneMeshDataBuilder : MonoBehaviour
    {
        private HashSet<MeshFilter> sceneCache = new HashSet<MeshFilter>();

        [Button("Set Meshdatabuilders")]
        void Reset()
        {
#if UNITY_EDITOR
            var activeScene = SceneManager.GetActiveScene();
            var root = activeScene.GetRootGameObjects();
            foreach (var obj in root) {
                var childMeshFilters = obj.GetComponentsInChildren<MeshFilter>();
                
                foreach (var meshFilter in childMeshFilters) {
                    var wireframe = meshFilter.GetComponentInParent<WireframeIdentifier>();
                    var meshdatabuilder = meshFilter.GetComponentInParent<MeshDataBuilder>();
                    if (wireframe && !meshdatabuilder && !sceneCache.Contains(meshFilter)) {
                        sceneCache.Add(meshFilter);
                        meshFilter.gameObject.AddComponent<MeshDataBuilder>();
                    } else if (sceneCache.Contains(meshFilter) && !wireframe) {
                        sceneCache.Remove(meshFilter);
                        if(meshdatabuilder){
                            Destroy(meshdatabuilder);
                        }
                    }
                }
            }
#endif
        }

        private void Awake()
        {
            Destroy(gameObject);
        }
    }
}