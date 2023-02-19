using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickToPlatform : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement player = other.transform.gameObject.transform.parent.GetComponent<PlayerMovement>(); 
        
        if(player != null)
        {
            player.transform.parent = transform; 
        }

    }

    private void OnTriggerExit(Collider other)
    {
        PlayerMovement player = other.transform.gameObject.transform.parent.GetComponent<PlayerMovement>();

        if (player != null)
        {
            player.transform.parent = null;
        }
    }

}
