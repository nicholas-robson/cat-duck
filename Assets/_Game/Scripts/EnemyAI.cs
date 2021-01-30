using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Brain brain;

    private List<Body> _targetBodies;
    private Body _nearestBody;
    private Transform _transform;
    private Body _body;
    private bool _hasBody;

    private void Awake()
    {
        _transform = transform;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (_hasBody)
        {
            var position = brain.transform.position;
            var direction = GameManager.player.transform.position - position;
            var distance = 20f;
            if (Physics.Raycast(position, direction, out var hit, distance))
            {
                Debug.DrawLine(position, hit.point, Color.green);
                var player = hit.collider.GetComponentInParent<Player>();
                if (player)
                {
                    if (hit.distance <= _body.minAttackDistance)
                    {
                        _body.Attack(direction.normalized);
                    }
                    else
                    {
                        _body.Move(direction.normalized);
                    }
                }
            }
            else
            {
                Debug.DrawLine(position, direction * distance, Color.red);
            }
        }
        else
        {
            GetNearestBody();
            
            if (_nearestBody != null)
            {
                var diff = _nearestBody.transform.position - brain.transform.position;
                diff.y = 0;
                brain.Move(diff.normalized);
            }

            // TODO: Direction to nearest body.
            //brain.Move();
        }
    }

    public Body GetBody()
    {
        return _body;
    }

    public bool HasBody()
    {
        return _hasBody;
    }

    public void SetBody(Body body)
    {
        _hasBody = true;
        _body = body;
    }

    public void EjectBody()
    {
        _hasBody = false;
    }

    private void GetNearestBody()
    {
        var bodyGameObjects = new List<Body>(FindObjectsOfType<Body>()).FindAll(body => !body.HasBrain());
        
        if (bodyGameObjects.Count == 0)
        {
            _nearestBody = null;
            return;
        }
        
        // Sort by distance.
        bodyGameObjects.Sort((a, b) =>
        {
            var brainPosition = brain.transform.position;
            return Vector3.Distance(brainPosition, a.transform.position)
                .CompareTo(
                    Vector3.Distance(brainPosition, b.transform.position)
                );
        });
        
        _nearestBody = bodyGameObjects[0];
    }
}