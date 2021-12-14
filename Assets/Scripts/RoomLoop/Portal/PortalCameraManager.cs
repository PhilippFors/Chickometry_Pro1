using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UsefulCode.Utilities;

namespace RoomLoop.Portal
{
    public class PortalCameraManager : SingletonBehaviour<PortalCameraManager>
    {
        private List<Portal> portals = new List<Portal>();

        public void AddPortal(Portal portal) => portals.Add(portal);

        // private void Start()
        // {
        //     RenderPipelineManager.beginFrameRendering += UpdateAllCameras;
        // }
        //
        // private void OnDisable()
        // {
        //     RenderPipelineManager.beginFrameRendering -= UpdateAllCameras;
        // }

        private void UpdateAllCameras(ScriptableRenderContext ctx, Camera[] cams)
        {
            // if (portals.Count > 0) {
            //     for (int i = 0; i < portals.Count; i++) {
            //         portals[i].UpdateCamera();
            //     }
            // }
        }
    }
}