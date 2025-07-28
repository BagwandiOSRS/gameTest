using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ColliderManager : MonoBehaviour
{
    protected virtual void OnTriggerEnter(Collider other)
    {

    }
    protected virtual void OnTriggerExit(Collider other)
    {
        
    }
    /*
    [SerializeField] protected GameObject playerObject;
    protected virtual void ExecuteEnter(Collider collider)
    {
        // Override
    }
    protected virtual void ExecuteExit(Collider collider)
    {
        // Override
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject != playerObject) return;
        ExecuteEnter(collider);
    }
    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject != playerObject) return;
        ExecuteExit(collider);
    }
    */
}
