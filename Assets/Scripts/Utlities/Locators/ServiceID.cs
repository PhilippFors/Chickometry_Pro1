using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Utlities.Locators
{
    /// <summary>
    /// Attach this to a gameobject with a service and it will register the object.
    /// </summary>
    public class ServiceID : MonoBehaviour
    {
        [SerializeField, ReadOnly] private Object registeredObject;
        private void OnEnable()
        {
            var service = GetComponent<IService>();
            registeredObject = (Component) service;
            ServiceLocator.Register(registeredObject);
        }

        private void OnDisable()
        {
            ServiceLocator.UnRegister(registeredObject);
            registeredObject = null;
        }
    }
}
