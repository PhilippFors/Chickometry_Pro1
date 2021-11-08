using System.Collections.Generic;
using Entities.Player.PlayerInput;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ObjectAbstraction.New
{
    public class AdvModelChanger : MonoBehaviour, IModelChanger
    {
        public bool Shootable => true;
        public bool IsAbstract => abstrLayer != 0;
        
        [SerializeField] private bool sperarateMeshCol;
        [SerializeField, Min(0)] private int abstrLayer; //current abstraction layer
        [SerializeField] private float sensitivity = 1f;
        [SerializeField] private float absolute;
        [SerializeField] private float threshholdAbsolute;
        [SerializeField] private float threshhold;

        [SerializeField] private List<Mesh> meshes;
        [SerializeField, EnableIf("sperarateMeshCol")] private List<Mesh> colMeshes;

        private float MouseDeltaX => PlayerInputController.Instance.MouseDelta.ReadValue().x;
        private bool inThreshhold;
        private int width => Screen.width;
        private MeshCollider meshCollider;
        private MeshFilter meshFilter;
        
        private void Start()
        {
            meshCollider = GetComponent<MeshCollider>();
            meshFilter = GetComponent<MeshFilter>();
        }

        // Called every frame by the abstracto gun
        public void ToggleModels()
        {
            // works but idk if UI can be integrated all that nicely
            if (!inThreshhold) {
                absolute += (MouseDeltaX / width) * sensitivity;
                absolute = Mathf.Clamp(absolute, -0.05f, 1.05f);

                if (absolute > 1) {
                    if (abstrLayer + 1 < meshes.Count) {
                        abstrLayer++;
                        EnableLayer();
                        absolute = 0;
                        inThreshhold = true;
                    }
                }
                else if (absolute < 0f) {
                    if (abstrLayer > 0) {
                        abstrLayer--;
                        EnableLayer();
                        absolute = 1;
                        inThreshhold = true;
                    }
                }
            }
            
            if (inThreshhold) {
                threshholdAbsolute += (MouseDeltaX / width) * sensitivity;
                threshholdAbsolute = Mathf.Clamp(threshholdAbsolute, threshhold * -1, threshhold);
                if (threshholdAbsolute <= threshhold * -1 || threshholdAbsolute >= threshhold) {
                    inThreshhold = false;
                    threshholdAbsolute = 0;
                }
            }
        }

        private void EnableLayer()
        {
            if (sperarateMeshCol) {
                meshFilter.sharedMesh = meshes[abstrLayer];
                meshCollider.sharedMesh = colMeshes[abstrLayer];
            }
            else {
                meshFilter.sharedMesh = meshes[abstrLayer];
                meshCollider.sharedMesh = meshes[abstrLayer];
            }
        }

        private void OnValidate()
        {
            meshFilter = GetComponent<MeshFilter>();
            meshCollider = GetComponent<MeshCollider>();
            EnableLayer();
        }
    }
}