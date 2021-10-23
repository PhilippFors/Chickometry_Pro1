using UnityEngine;

namespace Utlities.Locators
{
    /// <summary>
    /// Base class for services. Just to avoid boilerplate code.
    /// </summary>
    [RequireComponent(typeof(ServiceID))]
    public class MonoBehaviourService : MonoBehaviour, IService
    {
    }
}