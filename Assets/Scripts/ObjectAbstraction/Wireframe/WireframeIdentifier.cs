using DG.Tweening;
using ObjectAbstraction.ModelChanger;
using UnityEngine;

namespace ObjectAbstraction.Wireframe
{
    /// <summary>
    /// Used to detect if an object can be turned into a wireframe.
    /// </summary>
    public class WireframeIdentifier : MonoBehaviour
    {
        [SerializeField] private float minWireFrameBlend = 0.1f;
        [SerializeField] private int defaultLayer = 1;
        [SerializeField] private int wireframeLayer = 8;

        private AdvModelChanger modelChanger;
        private bool isWireframe;
        private int wireframeBlendID => Shader.PropertyToID("_WireframeBlend");
        
        private void Start()
        {
            modelChanger = GetComponent<AdvModelChanger>();
        }
        
        public void ToggleWireFrame()
        {
            if (isWireframe) {
                if (modelChanger.IsAbstract) {
                    MaterialTransitions(modelChanger.AbstractRend.materials, minWireFrameBlend);
                    gameObject.layer = defaultLayer;
                    modelChanger.Shootable = true;
                }
                else {
                    MaterialTransitions(modelChanger.NormalRend.materials, minWireFrameBlend);
                    gameObject.layer = defaultLayer;
                    modelChanger.Shootable = true;
                }
            }
            else {
                if (modelChanger.IsAbstract) {
                    MaterialTransitions(modelChanger.AbstractRend.materials, 1);
                    gameObject.layer = wireframeLayer;
                    modelChanger.Shootable = false;
                }
                else {
                    MaterialTransitions(modelChanger.NormalRend.materials, 1);
                    gameObject.layer = wireframeLayer;
                    modelChanger.Shootable = false;
                }
            }

            isWireframe = !isWireframe;
        }
        
        public void MaterialTransitions(Material[] mats, float endValue, float duration = 0.2f)
        {
            foreach (var mat in mats) {
                mat.DOFloat(endValue, wireframeBlendID, duration);
            }
        }
    }
}