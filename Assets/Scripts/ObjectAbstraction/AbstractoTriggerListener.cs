using ObjectAbstraction;
using UnityEngine;

public class AbstractoTriggerListener : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var tr = other.GetComponentInChildren<AbstractoRadius>();
        if (tr) {
            Debug.LogError("Just entered an Abstracto Radious!");
        }
    }
}
