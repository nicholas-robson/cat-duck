using System.Collections;
using System.Collections.Generic;
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


    Transform _transform;
    Rigidbody _rb;
    Transform _triggerObj;
    public GameObject playerObject;

    Vector2 inputDirection;
    float nextTimeToJump;

    protected AudioSource audioSource;


    private void Awake()
    {
        _transform = transform;
        _rb = playerObject.GetComponent<Rigidbody>();
        _triggerObj = _transform.Find("PlayerTrigger");
        audioSource = GetComponent<AudioSource>();

    }


    void Update()
    {
        HandleInputs();


    }

    private void FixedUpdate()
    {
        currentStats = stats.GetCurrentStats();
        Vector3 rotAxis = Vector3.up * currentStats.rotationSpeed;
        Quaternion deltaRotation = Quaternion.Euler(inputDirection.x * rotAxis * Time.fixedDeltaTime);
        _rb.MoveRotation(_rb.rotation * deltaRotation);

        HandleMovement();
            

        _triggerObj.position = playerObject.transform.position;
        _triggerObj.rotation = playerObject.transform.rotation;
    } 

    public void OnTriggerEnter(Collider other)
    {
        grounded = true;
    }

    public void OnTriggerExit(Collider other)
    {
        grounded = false;
    }

    void HandleInputs()
    {
        inputDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;

    }

    void HandleMovement()
    {
        if (!grounded)
            return;

        if (Time.time >= nextTimeToJump && Mathf.Abs(inputDirection.y) > 0)
        {
            audioSource.PlayOneShot(audioSource.clip, 1.0f);
            Vector3 forwardForce = _rb.transform.forward * inputDirection.y * currentStats.jumpForce;
            Vector3 jumpForce = Vector3.up * currentStats.forwardForce;
            _rb.AddForce(forwardForce + jumpForce, ForceMode.Impulse);

            nextTimeToJump = Time.time + currentStats.jumpCooldown;
        }
    }
}
