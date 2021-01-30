using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public LayerMask playerLayerMask;
    
    public LayerMask bodyLayerMask;
    
    public string bodyTag;

    public Brain brain;
    
    public Body body;

    private List<Body> _targetBodies;
    
    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    // Update is called once per frame
    private void Update()
    {
        if (body)
        {
            
            
        }
        else
        {
            var position = _transform.position;
            if (Physics.Raycast(position, GameManager.player.transform.position - position, out var hit, 20f))
            {

            }
        }
    }
    
    public void OnEject()
    {
        var bodyGameObjects = GameObject.FindGameObjectsWithTag("Body");
        
        _targetBodies = new List<Body>();
    }
}
