using UnityEngine;
using System.Collections;

public class Body : MonoBehaviour
{
    public float minAttackDistance = 2f;
    
    private bool _hasBrain;
    private Brain _brain;
    //private Transform _triggerObj;
    private Transform _transform;
    private Moveable _movable;
    [SerializeField] Transform _brainPlaceholderPosition;
    [SerializeField] GameObject collectableColliderObj;

    AudioSource _audioSource;
    SphereCollider _collectableCollider;

    private void Awake()
    {
        _transform = transform;
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
        Vector3 upAngle = new Vector3(Random.Range(-5f, 5f), 1f, Random.Range(-5f, 5f));
        rb.AddForce(upAngle * 10, ForceMode.Impulse);
        rb.detectCollisions = true;

        _hasBrain = false;
        _brain = null;

     
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
