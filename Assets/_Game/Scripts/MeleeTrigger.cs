using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeTrigger : MonoBehaviour
{

    [SerializeField] GameObject DoNotHit;
    Collider _dontHitYourself;

    private void Awake()
    {
        _dontHitYourself = DoNotHit.GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == _dontHitYourself)
            return;
        else Debug.Log("I hit " + other.name);

        other.gameObject.SendMessage("GetWrecked", SendMessageOptions.DontRequireReceiver);
    }
}
