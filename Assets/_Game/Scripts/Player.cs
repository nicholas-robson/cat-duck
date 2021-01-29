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

    [SerializeField] float brainForce, headForce, torsoForce, armsForce, legsForce;

    Transform _transform;
    Rigidbody _rb;

    public GameObject playerObject;

    private void Awake()
    {
        _transform = transform;
        _rb = playerObject.GetComponent<Rigidbody>();
    }


    void Update()
    {
        Debug.Log(grounded);
        HandleHotkeys();

        
    }

    private void FixedUpdate()
    {
        _transform.position = playerObject.transform.position;

    }

    public void OnTriggerEnter(Collider other)
    {
        grounded = true;
    }

    public void OnTriggerExit(Collider other)
    {
        grounded = false;
    }

    void HandleHotkeys()
    {
        if (!grounded)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("What");
            _rb.AddForce(Vector3.up * brainForce, ForceMode.Impulse);
        }

    }
}
