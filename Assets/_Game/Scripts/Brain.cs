using UnityEngine;

public class Brain : MonoBehaviour
{
    private Player _player;
    private Rigidbody _rb;
    private Moveable _movable;

    void Awake()
    {
        _player = GetComponentInParent<Player>();
        _movable = GetComponent<Moveable>();
        _rb = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 direction)
    {
        _movable.Move(direction);
    }


}
