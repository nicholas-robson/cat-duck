using UnityEngine;

public class Body : MonoBehaviour
{
    public float minAttackDistance = 2f;
    
    private bool _hasBrain;
    private Brain _brain;
    //private Transform _triggerObj;
    private Transform _transform;
    private Moveable _movable;
    [SerializeField] Transform _brainPlaceholderPosition;
    AudioSource _audioSource;

    private void Awake()
    {
        _transform = transform;
        //_triggerObj = _transform.Find("PlayerTrigger");
        _movable = GetComponent<Moveable>();
        _audioSource = GetComponent<AudioSource>();
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

    public void EjectBrain()
    {
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

}
