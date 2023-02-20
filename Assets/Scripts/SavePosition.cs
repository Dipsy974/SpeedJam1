using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePosition : MonoBehaviour
{
    public Transform save;
    public bool animatorCondition;
    public GameObject grossePorte;
    private void OnTriggerEnter(Collider other)
    {

        if (other.GetComponent<CapsuleCollider>() != null)
        {
            PlayerMovement player = other.transform.gameObject.transform.parent.GetComponent<PlayerMovement>();

            player.SavePosition(save.position); 
            if(player.hasShield && player.hasWallRun && player.hasFrog && animatorCondition ){
                grossePorte.GetComponent<Animator>().Play("BigDoorOpening");
            }
        }
    }
}
