using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageArea : MonoBehaviour
{
    NellController nellRef;

    private void OnTriggerStay(Collider other)
    {
        if(nellRef == null)
            nellRef = other.GetComponent<NellController>();

        if (nellRef != null)
            nellRef.TakeDamage(.1f);
    }


}
