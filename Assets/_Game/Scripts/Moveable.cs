using System;
using Cinemachine;
using UnityEngine;

public class Moveable: MonoBehaviour
{
    public float forwardForce;
    public float maxVelocity;
    public float turnCooldown = 1;
    float nextTimeToTurn;
    [SerializeField] float uprightForce = 50f;
    [SerializeField] float turnForce = 1f;

    private bool _grounded = false;
    private CinemachineImpulseSource _impulse;
    private Rigidbody _rb;



    private void Awake()
    {
        _impulse = GetComponent<CinemachineImpulseSource>();
        _rb = GetComponent<Rigidbody>();

    }

    public void SetMaxAngularVelocity(float number)
    {
        _rb.maxAngularVelocity = number;
    }
    
    public void OnGroundedTriggerEnter(Collider other)
    {
        if (!_grounded)
        {
            // Landed.
            if (_impulse)
                _impulse.GenerateImpulse();
        }
        _grounded = true;
    }

    public void OnGroundedTriggerExit(Collider other)
    {
        _grounded = false;
    }

    private void FixedUpdate()
    {
        var planarVelocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
        
        var velocityMagnitude = planarVelocity.magnitude;
        if (velocityMagnitude > maxVelocity)
        {
            _rb.AddForce(planarVelocity.normalized * (-1 * (velocityMagnitude - maxVelocity)), ForceMode.VelocityChange);
        }
    }

    public bool IsGrounded()
    {
        return _grounded;
    }

    public void TryToGetYourselfUpBoy()
    {
        
        Vector3 upDirection = Vector3.up * uprightForce;
        _rb.AddForceAtPosition(upDirection, _rb.transform.TransformPoint(Vector3.up));
        _rb.AddForceAtPosition(-upDirection, _rb.transform.TransformPoint(-Vector3.up));
    }

    public void Move(Vector3 direction)
    {
        Debug.DrawLine(transform.position + Vector3.up * 0.05f, (transform.position + direction * 5f) + Vector3.up * 0.05f, Color.blue);

        //if (!grounded)
        //    return;


        Vector3 force = direction * forwardForce;
        _rb.AddForce(force, ForceMode.VelocityChange);

        
        float angle = Vector3.SignedAngle(direction, _rb.transform.forward, Vector3.up);
        if (Time.time >= nextTimeToTurn)
        {
            _rb.AddTorque(_rb.transform.up * angle * turnForce, ForceMode.VelocityChange);
            nextTimeToTurn = Time.time + turnCooldown;
        }


    }
}
