using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] PlayerStats stats;
    BodyStats currentStats;

    [SerializeField] Camera _camera;


    public Brain _brain;
    public Body _body;

    Vector3 inputDirection;



    void Update()
    {
        HandleInputs();

    }

    private void FixedUpdate()
    {
        currentStats = stats.GetCurrentStats();

        HandleMovement();
    }

    public void OnCollectableEnter(Collider other)
    {
        SetBody(other.GetComponentInParent<Body>());
    }

    public void SetBody(Body body)
    {
        _body = body;
        // TODO make more efficient? Don't overengineer tho
        _body.transform.parent = transform;
        _body.SetBrain(_brain);
    }


    void HandleInputs()
    {
        inputDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;

    }

    void HandleMovement()
    {


        if (inputDirection.x != 0 || inputDirection.z != 0)
        {

            Vector3 cameraForward = _camera.transform.forward;
            cameraForward.y = 0f;
            Vector3 cameraRight = _camera.transform.right;
            cameraRight.y = 0f;
            cameraForward.Normalize();
            cameraRight.Normalize();

            Vector3 direction = cameraForward * inputDirection.z + cameraRight * inputDirection.x;
            if (_body)
            {

            } else
            {
                _brain.Move(direction);
            }
                

        }
    }
}

