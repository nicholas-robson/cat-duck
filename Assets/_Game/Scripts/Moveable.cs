using System;
using Cinemachine;
using UnityEngine;

public class Moveable: MonoBehaviour
{
    public float forwardForce;
    public float nextTimeToPlaySound;
    public float soundCooldown;
    public float maxVelocity;
    
    private bool _grounded = false;
    private CinemachineImpulseSource _impulse;
    private Rigidbody _rb;

    private void Awake()
    {
        _impulse = GetComponent<CinemachineImpulseSource>();
        _rb = GetComponent<Rigidbody>();

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
        var velocityMagnitude = _rb.velocity.magnitude;
        if (velocityMagnitude > maxVelocity)
        {
            _rb.AddForce(_rb.velocity.normalized * (-1 * (velocityMagnitude - maxVelocity)), ForceMode.VelocityChange);
        }
    }

    public bool IsGrounded()
    {
        return _grounded;
    }

    public void TryToGetYourselfUpBoy()
    {
        float uprightStrength = 50.0f;
        Vector3 upDirection = Vector3.up * uprightStrength;
        _rb.AddForceAtPosition(upDirection, _rb.transform.TransformPoint(Vector3.up));
        _rb.AddForceAtPosition(-upDirection, _rb.transform.TransformPoint(-Vector3.up));
    }

    public void Move(Vector3 direction)
    {
        Debug.DrawLine(transform.position + Vector3.up * 0.05f, (transform.position + direction * 5f) + Vector3.up * 0.05f, Color.blue);

        // if (!grounded)
        //     return;


        Vector3 force = direction * forwardForce;
        _rb.AddForce(force, ForceMode.VelocityChange);


        float angle = Vector3.SignedAngle(direction, _rb.transform.forward, Vector3.up);
        if (angle >= 10 || angle <= -10)
            _rb.AddTorque(_rb.transform.up * angle * 0.5f, ForceMode.VelocityChange);





    }
}
