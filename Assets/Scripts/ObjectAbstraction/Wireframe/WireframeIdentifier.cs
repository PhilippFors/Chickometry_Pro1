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
        public bool IsWireframe => isWireframe;
        
        [SerializeField] private float minWireFrameBlend = 0.1f;
        [SerializeField] private MeshRenderer currentRend;
        [SerializeField] private int defaultLayer = 1;
        [SerializeField] private int wireframeLayer = 8;

        private AdvModelChanger modelChanger;
        private bool isWireframe;
        private Material currentMat;
        private int wireframeBlendID => Shader.PropertyToID("_WireframeBlend");
        
        private void Start()
        {
            modelChanger = GetComponent<AdvModelChanger>();
            currentMat = currentRend.sharedMaterial;
        }
        
        public void ToggleWireFrame()
        {
            if (isWireframe) {
                currentMat.DOFloat(minWireFrameBlend, wireframeBlendID, 0.2f);
                gameObject.layer = defaultLayer;
                modelChanger.Shootable = true;
            }
            else {
                currentMat.DOFloat(1, wireframeBlendID, 0.2f);
                gameObject.layer = wireframeLayer;
                modelChanger.Shootable = false;
            }

            isWireframe = !isWireframe;
        }
    }
}