using Cinemachine;
using UnityEngine;

public class Moveable: MonoBehaviour
{
    public float forwardForce;
    public float nextTimeToPlaySound;
    public float soundCooldown;
    
    private bool _grounded = false;
    private AudioSource _audioSource;
    private CinemachineImpulseSource _impulse;
    private Rigidbody _rb;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
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

    public bool IsGrounded()
    {
        return _grounded;
    }
    
    public void Move(Vector3 direction)
    {
        Debug.DrawLine(transform.position, transform.position + direction * 5f, Color.blue);

        if (Time.time >= nextTimeToPlaySound)
        {
            _audioSource.PlayOneShot(_audioSource.clip, 1.0f);
            nextTimeToPlaySound = Time.time + soundCooldown;
        }

        // if (!grounded)
        //     return;


        Vector3 force = direction * forwardForce;
        _rb.AddForce(force, ForceMode.VelocityChange);
        
    }
}
