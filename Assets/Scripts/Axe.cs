using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{
    public GameObject axeToRotate;
    public float rotateSpeed;
    public float angle;
    private int direction = 1;
    private float initialAngle; 

    // Start is called before the first frame update
    void Start()
    {
        initialAngle = axeToRotate.transform.eulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {
        Rotate(); 
    }

    private void Rotate()
    {
        float angleTarget = GetTargetRotation();

        //float angle = Mathf.LerpAngle(axeToRotate.transform.rotation.z, angleTarget, Time.deltaTime * rotateSpeed);
        //axeToRotate.transform.eulerAngles = new Vector3(0, 0, angle);

        
        axeToRotate.transform.localRotation = Quaternion.Lerp(axeToRotate.transform.localRotation, Quaternion.Euler(axeToRotate.transform.localRotation.x, axeToRotate.transform.localRotation.y, angleTarget), Time.deltaTime * rotateSpeed);


        //Reverse direction
        float angleDiff = Mathf.Abs(angleTarget) - Mathf.Abs(axeToRotate.transform.eulerAngles.z);
 
        

        if (Mathf.Abs(angleDiff) <= 20f)
        {
            direction *= -1;
        }
    }

    private float GetTargetRotation()
    {
        if (direction == 1)
        {
            return initialAngle + angle;
        }
        else
        {
            return initialAngle - angle;
        }
    }

}
