using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public float impactForce;
    public PlayerMovement player; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Impact(Vector3 colliderImpact)
    {

        Collider[] objectsInRange = Physics.OverlapSphere(colliderImpact, 2);

        for (int i = 0; i < objectsInRange.Length; i++)
        {


            if (objectsInRange[i].gameObject == gameObject)
            {

            }
            else if (objectsInRange[i].GetComponent<CapsuleCollider>() != null)
            {

                GameObject parentObject = objectsInRange[i].gameObject.transform.parent.gameObject;

                Vector3 objectPos = objectsInRange[i].transform.position;

                Vector3 forceDirection = (objectPos - transform.position).normalized;

                parentObject.GetComponent<PlayerMovement>().GetAffectedByExplosion(forceDirection, impactForce);

                player.RemoveShield();
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ImpactObstacle")
        {
            Debug.Log("Boum"); 
            var collisionPoint = other.ClosestPoint(transform.position); 
            Impact(collisionPoint);
        }

    }
}
