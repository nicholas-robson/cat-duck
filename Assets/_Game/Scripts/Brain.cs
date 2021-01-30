using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Brain : MonoBehaviour
{

    Player _player;
    private CinemachineImpulseSource _impulse;

    float nextTimeToPlaySound;

    
    protected AudioSource audioSource;
    [SerializeField] float soundCooldown;
    [SerializeField] float forwardForce;

    bool grounded = false;


    Rigidbody _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _player = GetComponentInParent<Player>();
        _impulse = GetComponent<CinemachineImpulseSource>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Attack(Vector3 direction)
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!grounded)
        {
            // Landed.
            if (_impulse)
                _impulse.GenerateImpulse();
        }
        grounded = true;
    }

    public void OnTriggerExit(Collider other)
    {
        grounded = false;
    }

    public void Move(Vector3 direction)
    {
        Debug.DrawLine(transform.position, transform.position + direction, Color.blue);

        if (Time.time >= nextTimeToPlaySound)
        {
            audioSource.PlayOneShot(audioSource.clip, 1.0f);
            nextTimeToPlaySound = Time.time + soundCooldown;
        }

        if (!grounded)
            return;


        Vector3 force = direction * forwardForce;
        _rb.AddForce(force, ForceMode.VelocityChange);


    }
}
