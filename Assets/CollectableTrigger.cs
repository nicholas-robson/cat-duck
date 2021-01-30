using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableTrigger : MonoBehaviour
{
    Player _player;

    private void Awake()
    {
        _player = GetComponentInParent<Player>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_player) return; 
        
        _player.OnCollectableEnter(other);

    }

}
