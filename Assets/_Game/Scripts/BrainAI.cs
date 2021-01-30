using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainAI : MonoBehaviour
{
    public LayerMask bodyLayerMask;
    
    public string bodyTag;
    
    private Transform _transform;
    
    private List<Body> _targetBodies;

    private void Awake()
    {
        _transform = transform;
    }

    void OnEject()
    {
        var bodyGameObjects = GameObject.FindGameObjectsWithTag("Body");
        
        _targetBodies = new List<Body>();
    }
    
    // Update is called once per frame
    void Update()
    {
        //Physics.Raycast(_transform.position, GameManager.player);
    }
}
