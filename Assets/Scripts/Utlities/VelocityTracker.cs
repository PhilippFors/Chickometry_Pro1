using System.Collections.Generic;
using Interactables;
using Interaction.Items;
using UnityEngine;
using Utlities.Locators;

namespace Utlities
{
    public class VelocityData
    {
        public GameObject referenceObject;
        public float totalVelocity;
        public float currentVelocityPerSecond;

        public Vector3 oldPos;
        public float currentVelocityRef;
    }

    /// <summary>
    /// A class that calculates the velocities for every active interactable item;
    /// Returns a reference to the velocity data when an object is registered.
    /// </summary>
    [RequireComponent(typeof(ServiceID))]
    public class VelocityTracker : MonoBehaviour, IService
    {
        [SerializeField] private int averageOutOverFrames = 5;
        [SerializeField] private float velocitySmoothTime = 0.1f;

        private int currentFrameCounter = 0;
        
        private Dictionary<BasePickUpInteractable, VelocityData> velocityObjects = new Dictionary<BasePickUpInteractable, VelocityData>();
        
        public void Register(BasePickUpInteractable obj)
        {
            var data = new VelocityData();
            data.referenceObject = obj.gameObject;
            data.oldPos = data.referenceObject.transform.position;

            velocityObjects.Add(obj, data);
        }

        public void Unregister(BasePickUpInteractable obj)
        {
            velocityObjects.Remove(obj);
        }

        public float GetVelocity(BasePickUpInteractable obj)
        {
            if (velocityObjects.TryGetValue(obj, out var value))
            {
                return value.currentVelocityPerSecond;
            }

            return 0;
        }
        
        private void Update()
        {
            CalculateVelocities();
        }

        public void CalculateVelocities()
        {
            currentFrameCounter++;

            if (currentFrameCounter >= averageOutOverFrames)
            {
                foreach (var vel in velocityObjects)
                {
                    CalculateTotalVelocity(vel.Value);
                }

                currentFrameCounter = 0;
            }

            foreach (var vel in velocityObjects)
            {
                UpdateCurrentVelocity(vel.Value);
            }
        }

        private void CalculateTotalVelocity(VelocityData data)
        {
            var position = data.referenceObject.transform.position;
            data.totalVelocity = (data.oldPos - position).magnitude;
            data.oldPos = position;
        }

        private void UpdateCurrentVelocity(VelocityData data)
        {
            var targetValue = (data.totalVelocity / Mathf.Clamp(averageOutOverFrames, 1, 999999)) * (1.0f / Time.deltaTime);
            data.currentVelocityPerSecond = Mathf.SmoothDamp(data.currentVelocityPerSecond, targetValue, ref data.currentVelocityRef,
                velocitySmoothTime);
        }
    }
}
