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
    [SerializeField] GameObject collectableColliderObj;
    [SerializeField] float yeet = 10f;

    [SerializeField]
    private Transform _centerOfMass;

    AudioSource _audioSource;
    SphereCollider _collectableCollider;

    private void Awake()
    {
        //_triggerObj = _transform.Find("PlayerTrigger");
        _moveable = GetComponent<Moveable>();
        _audioSource = GetComponent<AudioSource>();
        _collectableCollider = collectableColliderObj.GetComponent<SphereCollider>();
        if (_centerOfMass && _moveable)
            _moveable.GetComponent<Rigidbody>().centerOfMass = _centerOfMass.transform.localPosition;

    }

    public void SetBrain(Brain brain)
    {
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

        _audioSource.PlayOneShot(_audioSource.clip, 1.0f);


    }

    public void EjectBrain(Transform parent)
    {
        StartCoroutine(DisableCollectableColliderForTime(1));

        _brain.transform.parent = parent;
        _brain.transform.localScale = Vector3.one;
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
        
    }
    
    public void Move(Vector3 direction)
    {
        _moveable.Move(direction);
    }

    IEnumerator DisableCollectableColliderForTime(float time)
    {
        _collectableCollider.enabled = false;
        yield return new WaitForSeconds(time);
        _collectableCollider.enabled = true;
    }

}
