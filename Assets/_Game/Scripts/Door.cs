
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public List<PressurePlate> requiredPressurePlates;
    private Animator _animator;
    private bool _open;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void CheckOpen()
    {
        var wasOpen = _open;

        _open = requiredPressurePlates.TrueForAll(r => r.IsActivated());

        if (_open && !wasOpen)
        {
            // Open animation.
            _animator.Play("Open");
        }
        else if (!_open && wasOpen)
        {
            _animator.Play("Close");
        }
    }
}
