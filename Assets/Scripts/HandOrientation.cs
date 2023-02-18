using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandOrientation : MonoBehaviour
{

    public Transform orientation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = orientation.localRotation; 
    }
}
