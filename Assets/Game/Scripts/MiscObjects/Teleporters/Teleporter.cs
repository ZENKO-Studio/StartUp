using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [Tooltip("Whether teleportation happens automatically or on some trigger")]
    [SerializeField] protected bool bAutoTriggered = true;

    [Tooltip("Should player return here mandatorily from destination teleporter")]
    [SerializeField] protected bool bShouldReturn = false;

    //Set only if we want player to only go back to source 
    public Transform srcLocation;

    //List of possible teleport locations
    public List<Transform> dstLocations;

    protected Transform playerTrans;

    //Interact method to determine type of interaction
    protected virtual void Interact()
    {

    }

    private void Update()
    {
        if(!bAutoTriggered && playerTrans)
            Interact();
    }

    private void OnTriggerEnter(Collider other)
    {
        playerTrans = other.transform;
        if (bAutoTriggered)
            Teleport();
    }

    private void OnTriggerExit(Collider other)
    {
        playerTrans = null;
    }

    public void Teleport()
    {
        srcLocation = null;
        
        if(dstLocations.Count < 0)
        {
            Debug.LogError($"{transform.name} does not have any destination teleporter list...");
        }

        //Select a random teleporter (in case of many or just the one existing)
        int r = Random.Range(0, dstLocations.Count - 1);

        Debug.Log(r);

        if(bShouldReturn)
        {
            Teleporter t = null;
            if (dstLocations[r].TryGetComponent<Teleporter>(out t))
            {
                t.srcLocation = transform;
            }
        }
        Debug.Log(dstLocations[r].position);
        playerTrans.GetComponent<NellController>().Teleport(dstLocations[r]);
        //playerTrans.GetComponent<ThirdPersonController>().Teleport(dstLocations[r]);
        
    }
}
