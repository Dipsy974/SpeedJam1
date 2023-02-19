using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform platform;
    public Transform startPoint;
    public Transform endPoint;
    public float speed; 

    private int direction = 1;


    private void Start()
    {
        platform.position = startPoint.position; 
    }
    private void Update()
    {
        MoveTowardsTarget(); 
    }

    private Vector3 GetTarget()
    {
        if(direction == 1)
        {
            return startPoint.position; 
        }
        else
        {
            return endPoint.position; 
        }
    }

    private void MoveTowardsTarget()
    {
        Vector3 target = GetTarget();

        platform.position = Vector3.Lerp(platform.position, target, speed * Time.deltaTime);

        //Reverse direction 
        float distance = (target - platform.position).magnitude;
        
        if(distance <= 0.1f)
        {
            direction *= -1; 
        }
    }

    private void OnDrawGizmos()
    {
        if(platform != null && startPoint != null && endPoint != null)
        {
            Gizmos.DrawLine(platform.transform.position, startPoint.position);
            Gizmos.DrawLine(platform.transform.position, endPoint.position);
        }
    }

}
