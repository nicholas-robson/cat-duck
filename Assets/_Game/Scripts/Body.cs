using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : Wearable
{
    Transform _triggerObj;
    Transform _transform;

    void Awake()
    {
        _transform = transform;
        _triggerObj = _transform.Find("PlayerTrigger");


    }

}
