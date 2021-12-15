using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace RoomLoop.Portal
{
    public class Portal : MonoBehaviour
    {
        public Transform pairPortal;

        [SerializeField] private Camera portalCam;
        [SerializeField] private float clipOffset;
        [SerializeField] private bool useClipping;
        [SerializeField] private MeshRenderer portalScreen;
        [SerializeField] private Vector2Int renderTextureSize;

        private Camera mainCam;
        private RenderTexture renderTexture;

        private void Awake()
        {
            renderTextureSize.x = Screen.width;
            renderTextureSize.y = Screen.height;
            mainCam = Camera.main;
            AssignRenderTexture();
            RenderPipelineManager.beginFrameRendering += UpdateCamera;
        }

        private void OnDisable()
        {
            RenderPipelineManager.beginFrameRendering -= UpdateCamera;
        }

        private void AssignRenderTexture()
        {
            if (portalCam.targetTexture == null || portalCam.targetTexture.width != renderTextureSize.x ||
                portalCam.targetTexture.height != renderTextureSize.y) {
                renderTexture = new RenderTexture(renderTextureSize.x, renderTextureSize.y, 0);
                portalCam.targetTexture = renderTexture;
                portalScreen.material.SetTexture("_PortalTexture", renderTexture);
                portalScreen.material.SetVector("_Forward", transform.forward);
            }
        }

        public void SetTargetPortal(Portal target)
        {
            pairPortal = target.transform;
        }

        private void UpdateCamera(ScriptableRenderContext ctx, Camera[] cams)
        {
            if (!VisibleFromCamera(portalScreen, mainCam) || !portalScreen.isVisible) {
                portalCam.enabled = false;
                return;
            }

            portalCam.enabled = true;

            AssignRenderTexture();

            // for (int i = 3; i >= 0; --i) {
            RenderCamera();
            // }
        }

        private void RenderCamera(int renderIndex = 0, ScriptableRenderContext ctx = default)
        {
            var cameraTransform = portalCam.transform;
            // for (int i = 0; i <= renderIndex; ++i) {
            var relativePosition = transform.InverseTransformPoint(mainCam.transform.position);
            relativePosition = Quaternion.Euler(0, 180, 0) * relativePosition;
            cameraTransform.position = pairPortal.TransformPoint(relativePosition);

            var relativeRotation = transform.InverseTransformDirection(mainCam.transform.forward);
            relativeRotation = Quaternion.Euler(0, 180, 0) * relativeRotation;
            cameraTransform.forward = pairPortal.TransformDirection(relativeRotation);
            // }

            var newMatrix = mainCam.projectionMatrix;
            var dirToCamera = portalCam.transform.position - pairPortal.position;
            
            var clip = clipOffset;
            if (Vector3.Distance(portalCam.transform.position, pairPortal.position) < 0.1f) {
                clip = 0;
            }
            
            if (useClipping) {
                Plane p = new Plane(pairPortal.forward, pairPortal.position + pairPortal.forward * clip);
                Vector4 clipPlaneWorldSpace = new Vector4(p.normal.x, p.normal.y, p.normal.z, p.distance);
                Vector4 clipPlaneCameraSpace = Matrix4x4.Transpose(Matrix4x4.Inverse(portalCam.worldToCameraMatrix)) *
                                               clipPlaneWorldSpace;

                newMatrix = mainCam.CalculateObliqueMatrix(clipPlaneCameraSpace);
            }

            portalCam.projectionMatrix = newMatrix;
            // UniversalRenderPipeline.RenderSingleCamera(ctx, portalCam);
        }

        public bool VisibleFromCamera(Renderer rend, Camera cam)
        {
            Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(cam);
            return GeometryUtility.TestPlanesAABB(frustumPlanes, rend.bounds);
        }
    }
}