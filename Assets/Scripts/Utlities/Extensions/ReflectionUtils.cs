using UnityEngine;
using System.Reflection;

namespace Utlities
{
    public static class ReflectionUtils
    {
        public static T GetPropertyValue<T>(this Object obj, string fieldName)
        {
            var type = obj.GetType();
            var property = type.GetProperty(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);

            T value = (T)property.GetValue(obj);

            return value;
        }
        
        public static T GetFieldValue<T>(this Object obj, string fieldName)
        {
            var type = obj.GetType();
            var field = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            
            T value = (T)field.GetValue(obj);

            return value;
        }

        public static T GetSaticFieldValue<T>(this Object obj, string fieldName)
        {
            var type = obj.GetType();
            var field = type.GetField(fieldName, BindingFlags.Static | BindingFlags.NonPublic);

            T value = (T)field.GetValue(obj);

            return value;
        }

        public static void SetFieldValue<T>(this Object obj, string fieldName, T value)
        {
            var type = obj.GetType();
            var field = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);

            field.SetValue(obj, value);
        }
        
        public static void SetFieldValue<T>(this System.Object obj, string fieldName, T value)
        {
            var type = obj.GetType();
            var field = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);

            field.SetValue(obj, value);
        }
    }
}