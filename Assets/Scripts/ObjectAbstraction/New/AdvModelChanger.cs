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
        [SerializeField, EnableIf("hasSperarateMeshCol")] private List<Mesh> colMeshes;

        private float MouseDeltaX => PlayerInputController.Instance.MouseDelta.ReadValue().x;
        private bool isInThreshhold;
        private int ScreenWidth => Screen.width;
        private MeshCollider meshCollider;
        
        private float[] abstractionSections;
        private float realSensitivity;
        private float oldAbsolute;
        
        private void Start()
        {
            meshCollider = GetComponent<MeshCollider>();
            
            EnableLayer(abstrLayer);
            
            abstractionSections = new float[meshes.Count];
            
            var sec = 1f / meshes.Count;
            for (int i = 0; i < meshes.Count; i++) {
                if (i == 0) {
                    abstractionSections[i] = 0;
                    continue;
                } 
                if (i == meshes.Count - 1) {
                    abstractionSections[i] = 1;
                    continue;
                }

                abstractionSections[i] = sec * i;
            }

            realSensitivity = sensitivity / meshes.Count; // adjusting sensitivity so mouse speed is always the same
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
                        
                        // floating point precisions ahhhhhhhhh
                        if (absoluteMouseDelta >= abstractionSections[i] - 0.01f &&
                            absoluteMouseDelta <= abstractionSections[i] + 0.01f) {
                            if (oldAbsolute < absoluteMouseDelta) {
                                absoluteMouseDelta += 0.02f;
                            }
                            else {
                                absoluteMouseDelta -= 0.02f;
                            }

                            EnableLayer(i);
                            abstrLayer = i;
                            isInThreshhold = true;
                            break;
                        }
                    }
                }

                if (isInThreshhold) {
                    threshholdAbsolute += (MouseDeltaX / ScreenWidth) * sensitivity;
                    threshholdAbsolute = Mathf.Clamp(threshholdAbsolute, threshhold * -1, threshhold);
                    if (threshholdAbsolute <= threshhold * -1 || threshholdAbsolute >= threshhold) {
                        isInThreshhold = false;
                        threshholdAbsolute = 0;
                    }
                }
            }
        }

        private void EnableLayer(int layer)
        {
            if (hasSperarateMeshCol) {
                currentMeshFilter.sharedMesh = meshes[layer];
                meshCollider.sharedMesh = colMeshes[layer];
            }
            else {
                currentMeshFilter.sharedMesh = meshes[layer];
                meshCollider.sharedMesh = meshes[layer];
            }
        }

        private void OnValidate()
        {
            if (abstrLayer < meshes.Count) {
                meshCollider = GetComponent<MeshCollider>();
                EnableLayer(abstrLayer);
            }
        }
    }
}