using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckObject : MonoBehaviour
{
    public GameObject GetPlayer;
    private PlayerMovement playerMovement;
    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetPlayer.GetComponent<PlayerMovement>();

    }

    // Update is called once per frame
    void Update()
    {
        if( playerMovement.hasShield==true && playerMovement.hasWallRun==true && playerMovement.hasFrog == true){
            this.GetComponent<Animator>().Play("BigDoorOpening");
        }
    }
}
