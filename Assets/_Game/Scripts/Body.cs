using UnityEngine;
using System.Collections;

public class Body : MonoBehaviour
{
    public float minAttackDistance = 2f;
    
    private bool _hasBrain;
    private Brain _brain;
    //private Transform _triggerObj;
    private Moveable _moveable;
    [SerializeField] Transform _brainPlaceholderPosition;
    [SerializeField] Transform _attackParticlePosition;
    [SerializeField] GameObject collectableColliderObj;
    [SerializeField] GameObject _meleeColliderObject;
    [SerializeField] float yeet = 10f;
    [SerializeField] float attackForce;

    [SerializeField]
    private Transform _centerOfMass;

    AudioSource _audioSource;
    [SerializeField] GameObject SetBrainParticles;
    [SerializeField] GameObject AttackParticles;

    [SerializeField] GameManager _gm;


    SphereCollider _meleeCollider;

    private void Awake()
    {
        //_triggerObj = _transform.Find("PlayerTrigger");
        _moveable = GetComponent<Moveable>();
        _audioSource = GetComponent<AudioSource>();

        _meleeCollider = _meleeColliderObject.GetComponent<SphereCollider>();
        _meleeCollider = _meleeColliderObject.GetComponent<SphereCollider>();

        if (_centerOfMass && _moveable)
            _moveable.GetComponent<Rigidbody>().centerOfMass = _centerOfMass.transform.localPosition;
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


        //_audioSource.PlayOneShot(_audioSource.clip, 1.0f);
        _gm.Play("Squish");


    }

    public void EjectBrain(Transform parent)
    {
        Instantiate(SetBrainParticles, _brainPlaceholderPosition.position, Quaternion.identity);
        //_audioSource.PlayOneShot(_audioSource.clip, 1.0f);
        _gm.Play("Squish");

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

        _hasBrain = false;
        _brain = null;

     
    }

    public void GetWrecked()
    {
        Debug.Log("Taking Damage!");
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
        StartCoroutine(EnableMeleeColliderForTime(0.2f));
        _moveable.GetComponent<Rigidbody>().AddRelativeTorque(-Vector3.up * attackForce);
        Instantiate(AttackParticles, _attackParticlePosition.position, Quaternion.identity, _attackParticlePosition);

    }

    public void Move(Vector3 direction)
    {
        _moveable.Move(direction);
    }

    IEnumerator EnableMeleeColliderForTime(float time)
    {
        _meleeCollider.enabled = true;
        yield return new WaitForSeconds(time);
        _meleeCollider.enabled = false;
    }


}
