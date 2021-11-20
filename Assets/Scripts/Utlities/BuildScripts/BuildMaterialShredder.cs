using ObjectAbstraction.New;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utilities.BuildScripts
{
    /// <summary>
    /// Delete instance materials that were created in the editor.
    /// </summary>
    public class BuildMaterialShredder : IPreprocessBuildWithReport
    {
        public int callbackOrder
        {
            get => 0;
        }

        
        public void OnPreprocessBuild(BuildReport report)
        {
            // var sceneCount = SceneManager.sceneCountInBuildSettings;
            //
            // for (int i = 0; i < sceneCount; i++) {
            //     SceneManager.LoadScene(i);
            //     var activeScene = SceneManager.GetActiveScene();
            //     var root = activeScene.GetRootGameObjects();
            //     foreach (var obj in root) {
            //         var modelChanger = obj.GetComponent<AdvModelChanger>();
            //         if (modelChanger) {
            //             var children = modelChanger.GetComponentsInChildren<MeshRenderer>();
            //             foreach (var child in children) {
            //                 var mat = child.material;
            //                 child.material = null;
            //                 Object.Destroy(mat);
            //             }
            //         }
            //     }
            //
            //     SceneManager.UnloadSceneAsync(activeScene);
            // }
        }
    }
}