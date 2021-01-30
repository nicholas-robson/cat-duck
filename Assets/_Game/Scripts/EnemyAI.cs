using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Brain brain;
    public LayerMask lineOfSightLayerMask;
    public float maxLineOfSightDistance;

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
        if (HasBody())
        {
            var playerPosition = GameManager.player.GetPosition();
            var position = _body.transform.position;
            
            var playerPositionDiff = playerPosition - position;
            var playerDirection = playerPositionDiff.normalized;
            var playerDistance = playerPositionDiff.magnitude;
            if (playerDistance > 20f)
            {
                return;
            }

            if (Physics.Raycast(position, playerDirection, out var hit, maxLineOfSightDistance, lineOfSightLayerMask))
            {
                Debug.DrawLine(position, hit.point, Color.red);

                if (hit.distance < playerDistance)
                {
                    return;
                }
            }
            
            Debug.DrawLine(position, playerPosition, Color.green);

            var newDirection = new Vector3(playerDirection.x, 0f, playerDirection.z);
            newDirection = newDirection.normalized;
            if (playerDistance <= _body.minAttackDistance)
            {
                _body.Attack(newDirection);
            }
            else
            {
                _body.Move(newDirection);
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
        if (body.HasBrain()) return;
        _hasBody = true;
        _body = body;
        body.SetBrain(brain);
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
    
    public void OnCollectableEnter(Collider other)
    {
        if (!_hasBody)
        {
            var body = other.GetComponentInParent<Body>();
            if (body != null)
            {
                SetBody(body);
            }
        }
    }

    public void OnCollectableExit(Collider other)
    {
    }
}