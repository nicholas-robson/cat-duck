using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;


public class Body : MonoBehaviour
{
    public float minAttackDistance = 2f;
    
    private bool _hasBrain;
    private Brain _brain;
    private Transform _transform;
    private Moveable _moveable;

    public event Action EjectBrainEvent;

    [SerializeField] Transform _brainPlaceholderPosition;
    [SerializeField] Transform _attackParticlePosition;
    [SerializeField] GameObject collectableColliderObj;
    [SerializeField] GameObject _meleeColliderObject;
    [SerializeField] float yeet = 10f;
    [SerializeField] float attackForce;

    [SerializeField]
    private Transform _centerOfMass;

    [SerializeField] GameObject SetBrainParticles;
    [SerializeField] GameObject AttackParticles;

    IEnumerator attackCoroutine;


    private void Awake()
    {
        _moveable = GetComponent<Moveable>();
        _transform = transform;

        if (_centerOfMass && _moveable)
            _moveable.GetComponent<Rigidbody>().centerOfMass = _centerOfMass.transform.localPosition;
        
       

    }

    private void Start()
    {
        _moveable.SetMaxAngularVelocity(1000);
    }

    public void SetBrain(Brain brain)
    {
        Instantiate(SetBrainParticles, _brainPlaceholderPosition.position, Quaternion.identity);

        _hasBrain = true;   
        _brain = brain;
        _brain.transform.parent = _brainPlaceholderPosition;
        Rigidbody rb = _brain.GetComponent<Rigidbody>();
        rb.detectCollisions = false;
        rb.isKinematic = true;
        _brain.transform.localPosition = Vector3.zero;
        _brain.transform.localRotation = Quaternion.identity;

        if (_moveable && _centerOfMass)
            _moveable.GetComponent<Rigidbody>().centerOfMass = _centerOfMass.transform.localPosition;

        GameManager.Play("Squish");



    }

    public void EjectBrain(Transform parent)
    {

        Instantiate(SetBrainParticles, _brainPlaceholderPosition.position, Quaternion.identity);
        _transform.parent = null;

        GameManager.Play("Squish");

        _brain.transform.parent = parent;
        _brain.transform.localScale = Vector3.one * 0.5f;
        Rigidbody rb = _brain.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.detectCollisions = true;

        Vector3 upAngle = new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f));
        Vector3 randomTorque = new Vector3(Random.Range(-20, 20), Random.Range(-20, 20), Random.Range(-20, 20));

        rb.AddTorque(randomTorque * 5);
        rb.AddForce(upAngle * yeet, ForceMode.Impulse);

        if (_moveable && _centerOfMass)
        {
            Rigidbody mrgb = _moveable.GetComponent<Rigidbody>();
            mrgb.ResetCenterOfMass();
            Vector3 upDirection = Vector3.right * 50;
            mrgb.AddForceAtPosition(upDirection, mrgb.transform.TransformPoint(Vector3.up), ForceMode.Impulse);
        }

        _brain.SetImmune();

        _hasBrain = false;
        _brain = null;


     
    }

    public void GetWrecked()
    {
        if (HasBrain())
        {
            if (EjectBrainEvent != null)
                EjectBrainEvent.Invoke();
            EjectBrain(transform.parent);
        }

    }

    private void FixedUpdate()
    {
        if (HasBrain())
        {
            _moveable.TryToGetYourselfUpBoy();
        }
    }

    public bool HasBrain()
    {
        return _hasBrain;
    }
    
    public void Attack(Vector3 direction)
    {
        if (attackCoroutine != null)
            StopCoroutine(attackCoroutine);
        attackCoroutine = EnableMeleeColliderForTime(0.7f);
        StartCoroutine(attackCoroutine);
        _moveable.GetComponent<Rigidbody>().AddRelativeTorque(-Vector3.up * attackForce);
        Instantiate(AttackParticles, _attackParticlePosition.position, Quaternion.identity, _attackParticlePosition);

    }

    public void Move(Vector3 direction)
    {
        _moveable.Move(direction);
    }

    IEnumerator EnableMeleeColliderForTime(float time)
    {
        _meleeColliderObject.SetActive(true);
        yield return new WaitForSeconds(time);
        _meleeColliderObject.SetActive(false);
        attackCoroutine = null;
    }


}
