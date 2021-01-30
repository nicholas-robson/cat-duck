using UnityEngine;
using System.Collections;

public class Body : MonoBehaviour
{
    public float minAttackDistance = 2f;
    
    private bool _hasBrain;
    private Brain _brain;
    //private Transform _triggerObj;
    private Moveable _movable;
    [SerializeField] Transform _brainPlaceholderPosition;
    [SerializeField] GameObject collectableColliderObj;
    [SerializeField] float yeet = 10f;

    AudioSource _audioSource;
    SphereCollider _collectableCollider;

    private void Awake()
    {
        //_triggerObj = _transform.Find("PlayerTrigger");
        _movable = GetComponent<Moveable>();
        _audioSource = GetComponent<AudioSource>();
        _collectableCollider = collectableColliderObj.GetComponent<SphereCollider>();
    
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

        _audioSource.PlayOneShot(_audioSource.clip, 1.0f);


    }

    public void EjectBrain(Transform parent)
    {
        StartCoroutine(DisableCollectableColliderForTime(1));
        _brain.transform.parent = parent;
        _brain.transform.localScale = Vector3.one;
        Rigidbody rb = _brain.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        Vector3 upAngle = new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f));
        Vector3 randomTorque = new Vector3(Random.Range(-20, 20), Random.Range(-20, 20), Random.Range(-20, 20));
        rb.AddTorque(randomTorque * 5);
        rb.AddForce(upAngle * yeet, ForceMode.Impulse);
        rb.detectCollisions = true;

        _hasBrain = false;
        _brain = null;

     
    }

    private void FixedUpdate()
    {
        if (HasBrain())
        {
            _movable.TryToGetYourselfUpBoy();
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
        _movable.Move(direction);
    }

    IEnumerator DisableCollectableColliderForTime(float time)
    {
        _collectableCollider.enabled = false;
        yield return new WaitForSeconds(time);
        _collectableCollider.enabled = true;
    }

}
