using UnityEngine;

public class CollectableTrigger : MonoBehaviour
{
    private Player _player;
    private EnemyAI _enemy;

    private void Awake()
    {
        _player = GetComponentInParent<Player>();
        _enemy = GetComponentInParent<EnemyAI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_player) _player.OnCollectableEnter(other);
        if (_enemy) _enemy.OnCollectableEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (_player) _player.OnCollectableExit(other);
        if (_enemy) _enemy.OnCollectableExit(other);
    }

}
