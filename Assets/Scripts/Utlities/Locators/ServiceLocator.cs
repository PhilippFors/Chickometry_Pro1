using System.Collections.Generic;
using UnityEngine;

namespace Utlities
{
    /// <summary>
    /// Contains a static collection of Monobehaviour or other Unity Object classes.
    /// Only unique services should be used here, as it doesn't support multiple entities of a service.
    /// </summary>
    public static class ServiceLocator
    {
        private static Dictionary<string, Object> services = new Dictionary<string, Object>();

        public static T Get<T>() where T : Object
        {
            Object value;
            var st = typeof(T).ToString();

            services.TryGetValue(st, out value);
            if (!value)
            {
                Debug.LogError($"Service type '{typeof(T).Name}' does not exist.");
            }
            return (T) value;
        }

        public static void Register(Object obj)
        {
            var hash = obj.GetType().ToString();
            if (services.ContainsKey(hash))
            {
                UnRegister(obj);
            }

            services.Add(hash, obj);
        }

        public static void UnRegister(Object obj)
        {
            var hash = obj.GetType().ToString();
            if (services.ContainsKey(hash))
            {
                services.Remove(hash);
            }
        }
    }
}