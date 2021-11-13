using System;
using System.Collections.Generic;
using Entities.Player.PlayerInput;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ObjectAbstraction.New
{
    [RequireComponent(typeof(MeshCollider))]
    public class AdvModelChanger : MonoBehaviour, IModelChanger
    {
        public bool Shootable => true;
        public bool IsAbstract => abstrLayer != 0;
        public bool SimpleToggle => simpleToggle;

        [SerializeField] private bool simpleToggle;
        [SerializeField] private bool hasSperarateMeshCol;
        [SerializeField, Min(0)] private int abstrLayer; //current abstraction layer
        [SerializeField] private float sensitivity = 2f;
        [SerializeField] private float threshhold;
        [SerializeField, ReadOnly] private float absoluteMouseDelta;
        [SerializeField, ReadOnly] private float threshholdAbsolute;
        [SerializeField] private MeshFilter currentMeshFilter;
        [SerializeField] private MeshFilter nextMeshFilter;

        [SerializeField] private List<Mesh> meshes;

        [SerializeField, EnableIf("hasSperarateMeshCol")]
        private List<Mesh> colMeshes;

        private float MouseDeltaX => PlayerInputController.Instance.MouseDelta.ReadValue().x;
        private bool isInThreshhold;
        private int ScreenWidth => Screen.width;
        private MeshCollider meshCollider;

        public float[] abstractionSections;
        private float realSensitivity;
        private float oldAbsolute;
        private float oldThreshholdAbsolute;
        private void Start()
        {
            meshCollider = GetComponent<MeshCollider>();

            abstractionSections = new float[meshes.Count];

            var sec = 1f / (meshes.Count - 1);
            abstractionSections[0] = 0;
            abstractionSections[meshes.Count-1] = 1;
            for (int i = 1; i < meshes.Count - 1; i++) {
                abstractionSections[i] = sec * i;
            }

            realSensitivity = sensitivity / meshes.Count; // adjusting sensitivity so mouse speed is always the same

            currentMeshFilter.sharedMesh = meshes[abstrLayer];
        }

        // Called every frame by the abstracto gun
        public void ToggleModels()
        {
            if (!simpleToggle) {
                if (!isInThreshhold) {
                    oldAbsolute = absoluteMouseDelta;
                    absoluteMouseDelta += (MouseDeltaX / ScreenWidth) * realSensitivity;
                    absoluteMouseDelta = Mathf.Clamp(absoluteMouseDelta, 0, 1);

                    for (int i = 0; i < abstractionSections.Length; i++) {
                        //if the delta is so high that it skips a section
                        if (oldAbsolute < abstractionSections[i] && absoluteMouseDelta > abstractionSections[i] ||
                            oldAbsolute > abstractionSections[i] && absoluteMouseDelta < abstractionSections[i]) {
                            abstrLayer = i;
                            EnableLayer(i);
                            break;
                        }
                        
                        // floating point precisions ahhhhhhhhh
                        if (absoluteMouseDelta >= abstractionSections[i] - 0.01f &&
                            absoluteMouseDelta <= abstractionSections[i] + 0.01f) {

                            abstrLayer = i;
                            EnableLayer(i);
                            isInThreshhold = true;
                            break;
                        }
                    }
                }

                if (isInThreshhold) {
                    
                    // big load of nothing. When exiting the threshold and going back to the previous section,
                    // the nextMeshFilter is basically stuck with what it thought was gonna be the next mesh, either one lower or higher,
                    // without considering the possibility that it could go to where it just was previously.
                    // lot of code that doesn't do shit aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa. 
                    // extend this with a previousLayer variable so it can compare where it ends up?
                    oldThreshholdAbsolute = threshholdAbsolute;
                    threshholdAbsolute += (MouseDeltaX / ScreenWidth) * sensitivity;
                    threshholdAbsolute = Mathf.Clamp(threshholdAbsolute, threshhold * -1, threshhold);
                    if (oldThreshholdAbsolute < threshholdAbsolute) {
                        if (abstrLayer + 1 < meshes.Count) {
                            nextMeshFilter.sharedMesh = meshes[abstrLayer + 1];
                        }
                        else {
                            nextMeshFilter.sharedMesh = meshes[meshes.Count - 1];
                        }
                    }
                    else if(oldThreshholdAbsolute > threshholdAbsolute) {
                        if (abstrLayer > 0) {
                            nextMeshFilter.sharedMesh = meshes[abstrLayer - 1];
                        }
                        else {
                            nextMeshFilter.sharedMesh = meshes[0];
                        }
                    }

                    if (threshholdAbsolute <= threshhold * -1 || threshholdAbsolute >= threshhold) {
                        if (oldThreshholdAbsolute < threshholdAbsolute) {
                            absoluteMouseDelta += 0.02f;
                            absoluteMouseDelta = Mathf.Clamp(absoluteMouseDelta, 0, 1);
                        }
                        else if(oldThreshholdAbsolute > threshholdAbsolute) {
                            absoluteMouseDelta -= 0.02f;
                            absoluteMouseDelta = Mathf.Clamp(absoluteMouseDelta, 0, 1);
                        }
                        isInThreshhold = false;
                        threshholdAbsolute = 0;
                    }
                }
            }
            else {
                abstrLayer++;
                if (abstrLayer == meshes.Count) {
                    abstrLayer = 0;
                }

                EnableLayer(abstrLayer);
            }
        }

        private void EnableLayer(int layer)
        {
            if (nextMeshFilter.sharedMesh) {
                currentMeshFilter.sharedMesh = nextMeshFilter.sharedMesh;
                if (hasSperarateMeshCol) {
                    meshCollider.sharedMesh = colMeshes[layer];
                }
                else {
                    meshCollider.sharedMesh = nextMeshFilter.sharedMesh;
                }
            }
            // if (hasSperarateMeshCol) {
                //     currentMeshFilter.sharedMesh = meshes[layer];
                //     meshCollider.sharedMesh = colMeshes[layer];
                // }
                // else {
                //     currentMeshFilter.sharedMesh = meshes[layer];
                //     meshCollider.sharedMesh = meshes[layer];
                // }
            
        }

        private void OnValidate()
        {
            if (abstrLayer < meshes.Count) {
                meshCollider = GetComponent<MeshCollider>();
                currentMeshFilter.sharedMesh = meshes[abstrLayer];
                if (hasSperarateMeshCol) {
                    meshCollider.sharedMesh = colMeshes[abstrLayer];
                }
                else {
                    meshCollider.sharedMesh = meshes[abstrLayer];
                }
            }
        }
    }
}