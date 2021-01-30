using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTrigger : MonoBehaviour
{
    private Moveable _moveable;

    private void Awake()
    {
        _moveable = GetComponentInParent<Moveable>();
    }

    private void OnTriggerEnter(Collider other)
    {
       _moveable.OnGroundedTriggerEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        _moveable.OnGroundedTriggerExit(other);
    }
}
