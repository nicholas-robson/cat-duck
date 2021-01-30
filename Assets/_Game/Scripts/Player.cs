using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{

    [SerializeField] Camera _camera;
    [SerializeField] Canvas _canvas;
    [SerializeField] TextMeshProUGUI tooltipText;

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
        SetTooltipToPosition(other);
    }

    public void OnCollectableExit(Collider other)
    {
        collectable = null;
        tooltipText.gameObject.SetActive(false);
    }

    void SetTooltipToPosition(Collider other)
    {
        Debug.Log("This!");
        float offsetY = other.transform.position.y;
        float offsetX = other.transform.position.x + 1.5f;

        Vector3 offsetPos = new Vector3(offsetX, offsetY, other.transform.position.z);

        Vector2 canvasPos;
        Vector2 screenPoint = _camera.WorldToScreenPoint(offsetPos);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.GetComponent<RectTransform>(), screenPoint, null, out canvasPos);

        tooltipText.gameObject.SetActive(true);
        tooltipText.transform.localPosition = canvasPos;

    }

    public void SetBody(Body body)
    {
        if (body.HasBrain()) return;
        
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

