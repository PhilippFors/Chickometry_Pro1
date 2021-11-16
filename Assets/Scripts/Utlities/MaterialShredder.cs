using ObjectAbstraction.New;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utilities
{
    /// <summary>
    /// Deleting any instance material that get created during editor runtime;
    /// </summary>
    [DefaultExecutionOrder(-50)]
    public class MaterialShredder : MonoBehaviour
    {

        void Awake()
        {
#if UNITY_EDITOR
            var activeScene = SceneManager.GetActiveScene();
            var root = activeScene.GetRootGameObjects();
            foreach (var obj in root) {
                var modelChanger = obj.GetComponent<AdvModelChanger>();
                if (modelChanger) {
                    var children = modelChanger.GetComponentsInChildren<MeshRenderer>();
                    foreach (var child in children) {
                        var mat = child.material;
                        child.material = null;
                        Destroy(mat);
                    }
                }
            }
#endif
            Destroy(gameObject);
        }

    }
}