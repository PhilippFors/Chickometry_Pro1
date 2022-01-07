using UnityEngine;

namespace Utilities.Math
{
    public static class MathUtils
    {
        public static Vector3 FindRandomInArea(GameObject obj, BoxCollider bounds)
        {
            return new Vector3(
                Random.Range(
                    obj.transform.position.x - obj.transform.localScale.x * bounds.size.x * 0.5f,
                    obj.transform.position.x + obj.transform.localScale.x * bounds.size.x * 0.5f),
                Random.Range(
                    obj.transform.position.y - obj.transform.localScale.y * bounds.size.y * 0.5f,
                    obj.transform.position.y + obj.transform.localScale.y * bounds.size.y * 0.5f),
                Random.Range(
                    obj.transform.position.z - obj.transform.localScale.z * bounds.size.z * 0.5f,
                    obj.transform.position.z + obj.transform.localScale.z * bounds.size.z * 0.5f)
            );
        }
    }
}