using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTrigger : MonoBehaviour
{
    Player _player;

    private void Awake()
    {
        _player = GetComponentInParent<Player>();
        
    }

    private void OnTriggerEnter(Collider other)
    {
        _player.OnTriggerEnter(other);
        
    }

    private void OnTriggerExit(Collider other)
    {
        _player.OnTriggerExit(other);
    }
}
