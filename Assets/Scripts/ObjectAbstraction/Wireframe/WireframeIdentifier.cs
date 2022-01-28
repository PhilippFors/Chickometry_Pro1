using DG.Tweening;
using ObjectAbstraction.ModelChanger;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using Utlities.MeshUtils;

namespace ObjectAbstraction.Wireframe
{
    /// <summary>
    /// Used to detect if an object can be turned into a wireframe.
    /// </summary>
    [RequireComponent(typeof(MeshDataBuilder))]
    public class WireframeIdentifier : MonoBehaviour
    {
        [SerializeField] private float minWireFrameBlend = 0.1f;
        [SerializeField] private int defaultLayer = 1;
        [SerializeField] private int wireframeLayer = 8;
        [SerializeField] private float transitionDuration = 0.2f;
        [SerializeField] private bool isWireframeOnStart;
        
        private AdvModelChanger modelChanger;
        private bool isWireframe;
        private int wireframeBlendID => Shader.PropertyToID("_WireframeBlend");
        private Collider abstractableCol;
        private void Start()
        {
            modelChanger = GetComponent<AdvModelChanger>();
            if (isWireframeOnStart) {
                isWireframe = false;
                ToggleWireFrame();
            }
        }

        public void ToggleWireFrame()
        {
            if (isWireframe) {
                if (modelChanger.IsAbstract) {
                    DisableWireframe(modelChanger.AbstractRend.materials);
                }
                else {
                    DisableWireframe(modelChanger.NormalRend.materials);
                }
            }
            else {
                if (modelChanger.IsAbstract) {
                    EnableWireFrame(modelChanger.AbstractRend.materials);
                }
                else {
                    EnableWireFrame(modelChanger.NormalRend.materials);
                }
            }

            isWireframe = !isWireframe;
        }

        private void EnableWireFrame(Material[] mats)
        {
            MaterialTransitions(mats, 1);
            var objs = GetComponentsInChildren<Collider>();
            foreach (var o in objs) {
                if (o.gameObject.layer == 11) {
                    abstractableCol = o;
                }
                o.gameObject.layer = wireframeLayer;
            }

            modelChanger.Shootable = false;
        }

        private void DisableWireframe(Material[] mats)
        {
            MaterialTransitions(mats, minWireFrameBlend);
            var objs = GetComponentsInChildren<Collider>();
            foreach (var o in objs) {
                if (o == abstractableCol) {
                    o.gameObject.layer = 11;
                }
                else {
                    o.gameObject.layer = defaultLayer;
                }
            }
            
            modelChanger.Shootable = true;
        }

        public void MaterialTransitions(Material[] mats, float endValue)
        {
            foreach (var mat in mats) {
                mat.DOFloat(endValue, wireframeBlendID, transitionDuration);
            }
        }
    }
}