using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidFall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {

        if (other.GetComponent<CapsuleCollider>() != null)
        {
            PlayerMovement player = other.transform.gameObject.transform.parent.GetComponent<PlayerMovement>();

            player.Respawn();
        }
    }
}
