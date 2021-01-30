using UnityEngine;

public class Body : MonoBehaviour
{
    private Transform _triggerObj;
    private Transform _transform;
    private Moveable _movable;

    private void Awake()
    {
        _transform = transform;
        _triggerObj = _transform.Find("PlayerTrigger");

        _movable = GetComponent<Moveable>();

    }
    
    public void Attack(Vector3 direction)
    {
        
    }
    
    public void Move(Vector3 direction)
    {
        _movable.Move(direction);
    }

}
