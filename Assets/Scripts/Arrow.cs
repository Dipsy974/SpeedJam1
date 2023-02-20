using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private float force; 
    public ArrowLauncher fromArrowLauncher;
    public Rigidbody arrowRigidbody; 

    // Start is called before the first frame update
    void Start()
    {
        force = fromArrowLauncher.arrowForce; 
    }

    // Update is called once per frame
    void Update()
    {
        MoveArrow(); 
    }

    private void MoveArrow()
    {
        Vector3 moveDir = transform.forward * force;
        arrowRigidbody.AddForce(moveDir, ForceMode.Force);
    }

    private void OnCollisionEnter(Collision collision)
    {


        if (collision.gameObject.GetComponent<PlayerMovement>() != null)
        {
            if (!collision.gameObject.GetComponent<PlayerMovement>().isShielding)
            {
                collision.gameObject.GetComponent<PlayerMovement>().Respawn();
            }

        }

        Destroy(gameObject); 
    }
}
