using System.Collections.Generic;
using UnityEngine;

namespace Trajectory
{
    public class TrajectoryPredictor : MonoBehaviour
    {
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private int iterations;
        [SerializeField] private float timeStep;
        
        public void Simulate(Vector3 startPoint, Vector3 initVel, Vector3 force, Rigidbody rb)
        {
            var points = new List<Vector3>();
            for (int i = 0; i < iterations; i++) {
                var point = GetPoint(startPoint, force, rb.mass, (i) * timeStep, rb.drag);
                points.Add(point);
            }
            lineRenderer.positionCount = iterations;
            lineRenderer.SetPositions(points.ToArray());
        }
        
        public void EnableTrajectory()
        {
            lineRenderer.enabled = true;
        }

        public void DisableTrajectory()
        {
            lineRenderer.enabled = false;
            lineRenderer.positionCount = 0;
        }
        
        private Vector3 GetPoint(Vector3 start, Vector3 force, float mass, float t, float drag = 1) =>
            start + (force / mass * t) + Physics.gravity * (t * t) / 2;
    }
}