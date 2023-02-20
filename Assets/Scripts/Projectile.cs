using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float explosionRadius;
    public float explosionForce;
    public Animator animator;
    public GameObject particles; 

    private Rigidbody rigibody;
    private bool hitTarget;

    private void Start()
    {
        rigibody = GetComponent<Rigidbody>();

        Physics.IgnoreLayerCollision(9, 11);

        //Effects at start
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 6 || collision.gameObject.layer == 7 ){

            Vector3 normal = collision.contacts[0].normal;
            transform.rotation = Quaternion.FromToRotation(transform.up, normal);

            rigibody.isKinematic = true;
            transform.SetParent(collision.transform);
        }
        
    }

    public void Explode()
    {
        //Explosion effect


        //Loop to find all objects interacting with explosion radius
      
        Collider[] objectsInRange = Physics.OverlapSphere(transform.position, explosionRadius);
       
        for (int i = 0; i < objectsInRange.Length; i++)
        {
            

            if (objectsInRange[i].gameObject == gameObject)
            {

            }else if(objectsInRange[i].GetComponent<CapsuleCollider>() != null)
            {
                
                GameObject parentObject = objectsInRange[i].gameObject.transform.parent.gameObject;

                Vector3 objectPos = objectsInRange[i].transform.position;

                Vector3 forceDirection = (objectPos - transform.position).normalized;

                parentObject.GetComponent<PlayerMovement>().GetAffectedByExplosion(forceDirection, explosionForce);
            }
        }

        animator.Play("FrogExplosionV2");
        
        Invoke(nameof(DestroyProjectile), 0.3f); 
   
    }

    private void DestroyProjectile()
    {
        Instantiate(particles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
