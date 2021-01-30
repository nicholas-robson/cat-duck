using UnityEngine;

public class Body : MonoBehaviour
{
    public float minAttackDistance = 2f;
    
    private bool _hasBrain;
    private Brain _brain;
    private Transform _triggerObj;
    private Transform _transform;
    private Moveable _movable;

    private void Awake()
    {
        _transform = transform;
        _triggerObj = _transform.Find("PlayerTrigger");
        _movable = GetComponent<Moveable>();
    }

    public void SetBrain(Brain brain)
    {
        _hasBrain = true;
        _brain = brain;
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
