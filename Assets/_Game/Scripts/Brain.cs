using UnityEngine;
using System.Collections;
using System;

public class Brain : MonoBehaviour
{
    private Player _player;
    private Rigidbody _rb;
    private Moveable _movable;
    private bool isImmune = false;
    public bool IsImmute { get => isImmune; }
    [SerializeField]
    float immunityPeriod = 1f;
    [SerializeField]
    private GameObject BrainDeathEffect;

    public event Action OnDeathEvent;

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

    public void GetWrecked()
    {
        if (!isImmune)
        {
            if (OnDeathEvent != null)
                OnDeathEvent.Invoke();

            if (BrainDeathEffect)
                Instantiate(BrainDeathEffect, _rb.position, Quaternion.identity);
            //Destroy(gameObject);

        }
    }

    public void SetImmune()
    {
        StartCoroutine(SetImmuneWhileBeingEjected(immunityPeriod));
    }

    IEnumerator SetImmuneWhileBeingEjected(float immunityPeriod)
    {
        isImmune = true;
        yield return new WaitForSeconds(immunityPeriod);
        isImmune = false;
    }





}
