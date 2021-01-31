using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Analytics;

public class EnemyAI : MonoBehaviour
{
    public Brain brain;
    public LayerMask lineOfSightLayerMask;
    public float maxLineOfSightDistance;
    public float maxBodySearchDistance = 10f;

    private List<Body> _targetBodies;
    private Body _nearestBody;
    private Transform _transform;
    private Body _body;
    private bool _hasBody;
    [SerializeField]
    private float attackCooldown = 0.2f;
    private float timeToNextAttack;

    private void Awake()
    {
        _transform = transform;
    }

    private void Start()
    {
        brain.OnDeathEvent += Death;
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
                if (Time.time >= timeToNextAttack)
                {
                    _body.Attack(newDirection);
                    timeToNextAttack = Time.time + attackCooldown;
                }
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
        _body.transform.parent = transform;
        _body.EjectBrainEvent += EjectBody;

        body.SetBrain(brain);
    }



    public void EjectBody()
    {
        _body.EjectBrainEvent -= EjectBody;
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
        
        var brainPosition = brain.transform.position;
        
        foreach (var body in bodyGameObjects)
        {
            var diff = body.transform.position - brainPosition;
            var distance = diff.magnitude;

            if (distance > maxLineOfSightDistance)
            {
                // Too far.
                continue;
            }
            
            if (Physics.Raycast(brainPosition, diff.normalized, out var hit, maxLineOfSightDistance, lineOfSightLayerMask))
            {
                // Hit a wall.
                Debug.DrawLine(brainPosition, hit.point, Color.red);
                if (hit.distance < distance)
                {
                    continue;
                }
            }

            Debug.DrawLine(brainPosition, body.transform.position, Color.green);
            _nearestBody = body;
            return;
        }

        _nearestBody = null;
        
        //
        //
        // // Sort by distance.
        // bodyGameObjects.Sort((a, b) =>
        // {
        //     return Vector3.Distance(brainPosition, a.transform.position)
        //         .CompareTo(
        //             Vector3.Distance(brainPosition, b.transform.position)
        //         );
        // });
        //
        // if (Vector3.Distance(brainPosition, bodyGameObjects[0].transform.position) > maxBodySearchDistance)
        // {
        //     return;
        // }
        //
        // _nearestBody = bodyGameObjects[0];
    }
    
    public void OnCollectableEnter(Collider other)
    {
        if (!_hasBody)
        {
            var body = other.GetComponentInParent<Body>();
            if (body != null && !brain.IsImmute)
            {
                SetBody(body);
            }
        }
    }

    public void OnCollectableExit(Collider other)
    {
    }


    void Death()
    {
        Destroy(gameObject);
    }
}