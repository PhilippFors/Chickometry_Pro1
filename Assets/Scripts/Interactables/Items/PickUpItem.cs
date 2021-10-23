using Audio.SFX;
using UnityEngine;
using Utlities;

namespace Interactables.Items
{
    /// <summary>
    /// Base class for all items that the player can pick up.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class PickUpItem : MonoBehaviour, IPickUpInteractable
    {
        protected GameObject audioGameObject;
        protected GlobalSFXController sfxController;
        protected VelocityTracker velocityTracker;
        
        private void OnEnable()
        {
            velocityTracker = ServiceLocator.Get<VelocityTracker>();
            velocityTracker.Register(this);
            sfxController = ServiceLocator.Get<GlobalSFXController>();

            var audioSource = GetComponentInChildren<AudioSource>();

            // Temporary fix so prototpye prefabs always have an audiosource object
            if (!audioSource)
            {
                var obj = new GameObject();
                obj.transform.parent = transform;
                audioSource = obj.AddComponent<AudioSource>();
            }

            audioGameObject = audioSource.gameObject;
        }

        private void OnDisable()
        {
            velocityTracker.Unregister(this);
        }

        public virtual void OnInteract()
        {
        }

        public virtual void OnUse()
        {
            Debug.Log($"You just used {gameObject.name}");
        }

        public virtual void OnThrow()
        {
            var col = GetComponent<Collider>();
            col.enabled = true;
        }

        public virtual void OnCollisionEnter(Collision other)
        {
            var vel = velocityTracker.GetVelocity(this);
            if (vel > 2f)
            {
                // Play sfx
            }
            if (vel > 20f)
            {
                Debug.Log($"Just hit something with {vel} velocity");
            }
        }
    }
}