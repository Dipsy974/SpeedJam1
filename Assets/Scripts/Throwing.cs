using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwing : MonoBehaviour
{
    [Header("References")]
    public Transform cam;
    public Transform attackPoint;
    public Projectile objectToThrow;

    [Header("Settings")]
    public float throwCooldown;
    public KeyCode throwKey = KeyCode.Mouse1;
    public float throwForce;
    public float throwUpwardForce;

    private bool readyToThrow;

    private Projectile instanceProjectile; 

    private void Start()
    {
        readyToThrow = true; 
    }

    private void Update()
    {


        if(Input.GetKeyDown(throwKey))
        {
            if(instanceProjectile == null && readyToThrow)
            {
                Throw();
            }
            else if(instanceProjectile != null)
            {
                instanceProjectile.Explode(); 
            }
            
        }
    }

    private void Throw()
    {
        readyToThrow = false;

        //Instantiation
        Projectile projectile = Instantiate(objectToThrow, attackPoint.position, cam.rotation);

        instanceProjectile = projectile; 

        Rigidbody projectileRigibody = projectile.GetComponent<Rigidbody>();

        //Add force to Rigidbody
        Vector3 forceToAdd = cam.transform.forward * throwForce + transform.up * throwUpwardForce;
        projectileRigibody.AddForce(forceToAdd, ForceMode.Impulse);


        Invoke(nameof(ResetThrow), throwCooldown); 
    }

    private void ResetThrow()
    {
        readyToThrow = true; 
    }
}
