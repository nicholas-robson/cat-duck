using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] Camera _camera;


    public Brain _brain;
    public Body _body;

    Vector3 inputDirection;

    Collider collectable = null;

    void Update()
    {
        HandleInputs();

    }

    private void FixedUpdate()
    {

        HandleMovement();
    }

    public void OnCollectableEnter(Collider other)
    {
        collectable = other;
    }

    public void OnCollectableExit(Collider other)
    {
        collectable = null;
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

        if (Input.GetKeyDown(KeyCode.E) && _body)
        {
            _body.EjectBrain(transform);
            _body.transform.parent = null;
            _body = null;
        } else if (Input.GetKeyDown(KeyCode.E) && collectable)
        {
            SetBody(collectable.GetComponentInParent<Body>());

        }

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
                _body.Move(direction);

            } else
            {
                _brain.Move(direction);
            }
                

        }
    }

    public Vector3 GetPosition()
    {
        return _body ? _body.transform.position : _brain.transform.position;
    }
}

