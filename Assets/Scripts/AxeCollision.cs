using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision); 
        if (collision.gameObject.GetComponent<PlayerMovement>() != null)
        {
            collision.gameObject.GetComponent<PlayerMovement>().Respawn();
        }


    }

}
