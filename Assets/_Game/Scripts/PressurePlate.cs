using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public List<MeshRenderer> lights;
    public Material lightsInactive;
    public Material lightsActive;

    public Door door;

    private List<Collider> _colliders;

    private void Awake()
    {
        _colliders = new List<Collider>();
        SetLights(lightsInactive);
    }

    public bool IsActivated()
    {
        return _colliders.Count > 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_colliders.Count == 0)
        {
            SetLights(lightsActive);
        }
        _colliders.Add(other);
        door.CheckOpen();
    }

    private void OnTriggerExit(Collider other)
    {
        _colliders.Remove(other);

        if (_colliders.Count == 0)
        {
            SetLights(lightsInactive);
        }
        
        door.CheckOpen();
    }

    private void SetLights(Material material)
    {
        foreach (var meshRenderer in lights)
        {
            meshRenderer.material = material;
        }
    }
}
