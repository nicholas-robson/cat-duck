using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTrigger : MonoBehaviour
{
    Brain _brain;

    private void Awake()
    {
        _brain = GetComponentInParent<Brain>();
        
    }

    private void OnTriggerEnter(Collider other)
    {
        _brain.OnTriggerEnter(other);
        
    }

    private void OnTriggerExit(Collider other)
    {
        _brain.OnTriggerExit(other);
    }
}
