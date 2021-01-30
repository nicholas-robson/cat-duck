using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Player : MonoBehaviour
{


    bool grounded = false;

    //[SerializeField] float jumpCooldown;
    //[SerializeField] float brainForce;
    //[SerializeField] float brainJumpForce;

    //[SerializeField] float headForce;
    //[SerializeField] float torsoForce;
    //[SerializeField] float armsForce;
    //[SerializeField] float legsForce;
    //[SerializeField] float rotationSpeed;

    [SerializeField] PlayerStats stats;
    BodyStats currentStats;

    [SerializeField] Camera _camera;

    Transform _transform;
    Rigidbody _rb;
    Transform _triggerObj;
    private CinemachineImpulseSource _impulse;
    public GameObject playerObject;

    Vector3 inputDirection;
    float nextTimeToPlaySound;

    protected AudioSource audioSource;


    private void Awake()
    {
        _transform = transform;
        _rb = playerObject.GetComponent<Rigidbody>();
        _triggerObj = _transform.Find("PlayerTrigger");
        audioSource = GetComponent<AudioSource>();
        _impulse = GetComponent<CinemachineImpulseSource>();
    }


    void Update()
    {
        HandleInputs();


    }

    private void FixedUpdate()
    {
        currentStats = stats.GetCurrentStats();
        //Vector3 rotAxis = Vector3.up * currentStats.rotationSpeed;
        //Quaternion deltaRotation = Quaternion.Euler(inputDirection.x * rotAxis * Time.fixedDeltaTime);
        //_rb.MoveRotation(_rb.rotation * deltaRotation);

        HandleMovement();
            

        _triggerObj.position = playerObject.transform.position;
        _triggerObj.rotation = playerObject.transform.rotation;
    } 

    public void OnTriggerEnter(Collider other)
    {
        if (!grounded)
        {
            // Landed.
            _impulse.GenerateImpulse();
        }
        grounded = true;
    }

    public void OnTriggerExit(Collider other)
    {
        grounded = false;
    }

    void HandleInputs()
    {
        inputDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;

    }

    void HandleMovement()
    {
        if (!grounded)
            return;

        if (inputDirection.x != 0 || inputDirection.z != 0)
        {
            if (Time.time >= nextTimeToPlaySound)
                audioSource.PlayOneShot(audioSource.clip, 1.0f);

            Vector3 cameraForward = _camera.transform.forward;
            cameraForward.y = 0f;
            Vector3 cameraRight = _camera.transform.right;
            cameraRight.y = 0f;
            cameraForward.Normalize();
            cameraRight.Normalize();

            Vector3 direction = cameraForward * inputDirection.z + cameraRight * inputDirection.x;

            Vector3 force = direction * currentStats.forwardForce;
            _rb.AddForce(force, ForceMode.VelocityChange);
            nextTimeToPlaySound = Time.time + currentStats.soundCooldown;

        }
    }
}
