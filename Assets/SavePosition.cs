using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePosition : MonoBehaviour
{
    public Transform save;
    private void OnTriggerEnter(Collider other)
    {

        if (other.GetComponent<CapsuleCollider>() != null)
        {
            PlayerMovement player = other.transform.gameObject.transform.parent.GetComponent<PlayerMovement>();

            player.SavePosition(save.position); 
        }
    }
}
