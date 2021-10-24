using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Utilities.AI
{
    public enum PathStatus
    {
        NoPath,
        PathPending,
        PathReady
    }

    /// <summary>
    /// Wrapper for important data for handling navigation.
    /// </summary>
    public class NavData
    {
        public NavData() {
            navMeshPath = new NavMeshPath();
            pathStatus = PathStatus.NoPath;
        }

        public readonly NavMeshPath navMeshPath;

        public Vector3 source;
        public Vector3 target;
        public Vector3[] Path => navMeshPath.corners;
        public PathStatus pathStatus;
    }

    /// <summary>
    /// Entities can request a path to be calculated.
    /// Calculates a limited amount of paths per frame to avoid performance issues.
    /// </summary>
    public static class AiPathManager
    {
        private static Queue<NavData> pathQueue = new Queue<NavData>();
        private static bool isUpdaterRunning;

        private static int BatchSize = 5;
        
        public static void RequestPath(NavData data) {
            // TODO: Pathrequests can be denied based on various conditions. Maximum amount of denied requests until one is finally approved. Denied request amount handled in NavData.
            pathQueue.Enqueue(data);
            data.pathStatus = PathStatus.PathPending;
            if (!isUpdaterRunning) {
                PathUpdater().Forget();
            }
        }

        private static async UniTaskVoid PathUpdater() {
            isUpdaterRunning = true;
            int batchCount = 0;

            while (pathQueue.Count > 0) {
                var data = pathQueue.Dequeue();

                // TODO: Get valid navmesh areas for the specific entity from NavData
                if (NavMesh.CalculatePath(data.source, data.target, NavMesh.AllAreas, data.navMeshPath)) {
                    data.pathStatus = PathStatus.PathReady;
                }
                else {
                    data.pathStatus = PathStatus.NoPath;
                }

                batchCount++;
                if (batchCount > BatchSize) {
                    await UniTask.Yield();
                    batchCount = 0;
                }
            }

            isUpdaterRunning = false;
        }
    }
}