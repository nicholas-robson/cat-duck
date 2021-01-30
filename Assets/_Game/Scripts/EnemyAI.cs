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
            var position = _transform.position;
            var direction = GameManager.player.transform.position - position;
            if (Physics.Raycast(position, GameManager.player.transform.position - position, out var hit, 20f))
            {
                var player = hit.collider.GetComponentInParent<Player>();
                if (player)
                {
                    body.Move(direction.normalized);
                }
            }
        }
        else
        {
            // TODO: Direction to nearest body.
            //brain.Move();
        }
    }
    
    public void OnEject()
    {
        var bodyGameObjects = GameObject.FindGameObjectsWithTag("Body");
        
        _targetBodies = new List<Body>();
    }
}
