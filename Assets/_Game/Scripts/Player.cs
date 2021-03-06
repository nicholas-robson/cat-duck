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
        if (collectable)
            SetTooltipToPosition(collectable);

    }

    private void Start()
    {
        _brain.OnDeathEvent += PlayerDeath;
    }

    private void FixedUpdate()
    {

        HandleMovement();
    }

    public void OnCollectableEnter(Collider other)
    {
        var body = other.GetComponentInParent<Body>();
        if (body != null && body.HasBrain()) return;
        
        collectable = other;
        
        SetTooltipToPosition(other);
        tooltipText.gameObject.SetActive(true);
    }

    public void OnCollectableExit(Collider other)
    {
        collectable = null;
        tooltipText.gameObject.SetActive(false);
    }

    void SetTooltipToPosition(Collider other)
    {
        float offsetY = other.transform.position.y;
        float offsetX = other.transform.position.x + 1.5f;

        Vector3 offsetPos = new Vector3(offsetX, offsetY, other.transform.position.z);

        Vector2 canvasPos;
        Vector2 screenPoint = _camera.WorldToScreenPoint(offsetPos);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.GetComponent<RectTransform>(), screenPoint, null, out canvasPos);

        tooltipText.transform.localPosition = canvasPos;

    }

    public void SetBody(Body body)
    {
        if (body.HasBrain()) return;
        
        _body = body;
        _body.transform.parent = transform;
        _body.SetBrain(_brain);
        _body.EjectBrainEvent += SetBodyToNull;
    }


    void HandleInputs()
    {
        inputDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;

        if (Input.GetKeyDown(KeyCode.E) && _body)
        {
            RemoveBrain();
            SetBodyToNull();

        } else if (Input.GetKeyDown(KeyCode.E) && collectable)
        {
            SetBody(collectable.GetComponentInParent<Body>());

        }
        if (Input.GetKeyDown(KeyCode.Space) && _body)
        {
            _body.Attack(inputDirection);
        } 

    }

    void RemoveBrain()
    {
        _body.EjectBrain(transform);
    }

    void SetBodyToNull()
    {
        _body.EjectBrainEvent -= SetBodyToNull;
        _body = null;

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

    public void SetPosition(Vector3 position)
    {
        if (_body)
        {
            _body.transform.position = position;
        }
        else
        {
            _brain.transform.position = position;
        }
    }

    void PlayerDeath()
    {
        GameManager.ResetLevel();
    }
}

