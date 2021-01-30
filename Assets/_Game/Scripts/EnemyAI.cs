using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyAI : MonoBehaviour
{
    public LayerMask playerLayerMask;
    
    
    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    // Update is called once per frame
    private void Update()
    {
        var position = _transform.position;
        if (Physics.Raycast(position, GameManager.player.transform.position - position, out var hit, 20f,
            playerLayerMask))
        {
            
        }
    }
}
