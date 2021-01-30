using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum BodyState {
        Brain,
        Head,
        Torso,
        Arms,
        Legs
    }

    bool grounded = false;

    [SerializeField] float jumpCooldown;
    [SerializeField] float brainForce;
    [SerializeField] float brainJumpForce;

    [SerializeField] float headForce;
    [SerializeField] float torsoForce;
    [SerializeField] float armsForce;
    [SerializeField] float legsForce;
    [SerializeField] float rotationSpeed;

    Transform _transform;
    Rigidbody _rb;
    Transform _triggerObj;
    public GameObject playerObject;

    Vector2 inputDirection;
    float nextTimeToJump;

    private void Awake()
    {
        _transform = transform;
        _rb = playerObject.GetComponent<Rigidbody>();
        _triggerObj = _transform.Find("PlayerTrigger");
    }


    void Update()
    {
        HandleInputs();


    }

    private void FixedUpdate()
    {
        Vector3 rotAxis = Vector3.up * rotationSpeed;
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

        if (Time.time >= nextTimeToJump)
        {
            Vector3 forwardForce = _rb.transform.forward * inputDirection.y * brainJumpForce;
            Vector3 jumpForce = Vector3.up * brainForce;
            _rb.AddForce((forwardForce + jumpForce) * Mathf.Abs(inputDirection.y), ForceMode.Impulse);

            nextTimeToJump = Time.time + jumpCooldown;
        }
    }
}
