using UnityEngine;
using UnityEngine.Rendering;

namespace RoomLoop.Portal
{
    public class Portal : MonoBehaviour
    {
        public Camera portalCamera;
        public Transform pairPortal;
        public float offset = 0.1f;

        private Camera mainCam;
        private MeshRenderer meshRenderer;
        private void Awake()
        {
            
            mainCam = Camera.main;
        }

        private void OnEnable()
        {
            RenderPipelineManager.beginCameraRendering += UpdateCamera;
            if (portalCamera.targetTexture) {
                portalCamera.targetTexture.Release();
            }

            portalCamera.projectionMatrix = mainCam.projectionMatrix;

            var renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
            portalCamera.targetTexture = renderTexture;

            meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.material.SetTexture("_PortalTexture", renderTexture);
            meshRenderer.material.SetVector("_Forward", transform.forward);
        }

        private void OnDisable()
        {
            RenderPipelineManager.beginCameraRendering -= UpdateCamera;
            portalCamera.targetTexture.Release();
        }

        private void UpdateCamera(ScriptableRenderContext ctx, Camera cam)
        {
            if (meshRenderer.isVisible && cam.CompareTag("MainCamera")) {
                portalCamera.projectionMatrix = mainCam.projectionMatrix;
                var relativePosition = transform.InverseTransformPoint(cam.transform.position);
                relativePosition = Vector3.Scale(relativePosition, new Vector3(-1, 1, -1));
                portalCamera.transform.position = pairPortal.TransformPoint(relativePosition);
                
                var relativeRotation = transform.InverseTransformDirection(cam.transform.forward);
                relativeRotation = Vector3.Scale(relativeRotation, new Vector3(-1, 1, -1));
                portalCamera.transform.forward = pairPortal.TransformDirection(relativeRotation);
                // var m = transform.localToWorldMatrix * pairPortal.transform.worldToLocalMatrix * mainCam.transform.localToWorldMatrix;
                // portalCamera.transform.SetPositionAndRotation(m.GetColumn(3), m.rotation);
            }
        }

        // Calculates reflection matrix around the given plane
        private static void CalculateReflectionMatrix(ref Matrix4x4 reflectionMat, Vector4 plane)
        {
            reflectionMat.m00 = (1F - 2F * plane[0] * plane[0]);
            reflectionMat.m01 = (-2F * plane[0] * plane[1]);
            reflectionMat.m02 = (-2F * plane[0] * plane[2]);
            reflectionMat.m03 = (-2F * plane[3] * plane[0]);

            reflectionMat.m10 = (-2F * plane[1] * plane[0]);
            reflectionMat.m11 = (1F - 2F * plane[1] * plane[1]);
            reflectionMat.m12 = (-2F * plane[1] * plane[2]);
            reflectionMat.m13 = (-2F * plane[3] * plane[1]);

            reflectionMat.m20 = (-2F * plane[2] * plane[0]);
            reflectionMat.m21 = (-2F * plane[2] * plane[1]);
            reflectionMat.m22 = (1F - 2F * plane[2] * plane[2]);
            reflectionMat.m23 = (-2F * plane[3] * plane[2]);

            reflectionMat.m30 = 0F;
            reflectionMat.m31 = 0F;
            reflectionMat.m32 = 0F;
            reflectionMat.m33 = 1F;
        }
    }
}