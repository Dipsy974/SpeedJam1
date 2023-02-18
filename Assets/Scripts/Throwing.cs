using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwing : MonoBehaviour
{
    [Header("References")]
    public Transform cam;
    public Transform attackPoint;
    public GameObject objectToThrow;

    [Header("Settings")]
    public int totalThrows;
    public float throwCooldown;
    public KeyCode throwKey = KeyCode.Mouse1;
    public float throwForce;
    public float throwUpwardForce;

    private bool readyToThrow;

    private void Start()
    {
        readyToThrow = true; 
    }

    private void Update()
    {
        attackPoint.localRotation = cam.localRotation; 

        if(Input.GetKeyDown(throwKey) && readyToThrow && totalThrows > 0)
        {
            Throw(); 
        }
    }

    private void Throw()
    {
        readyToThrow = false;

        //Instantiation
        GameObject projectile = Instantiate(objectToThrow, attackPoint.position, cam.rotation);

        Rigidbody projectileRigibody = projectile.GetComponent<Rigidbody>();

        //Add force to Rigidbody
        Vector3 forceToAdd = cam.transform.forward * throwForce + transform.up * throwUpwardForce;
        projectileRigibody.AddForce(forceToAdd, ForceMode.Impulse);

        totalThrows--;

        Invoke(nameof(ResetThrow), throwCooldown); 
    }

    private void ResetThrow()
    {
        readyToThrow = true; 
    }
}
