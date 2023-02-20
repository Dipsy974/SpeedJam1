using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; 

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private float sensX, sensY;
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform camHolder;

    private float xRotation, yRotation; 

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true; 
    }

    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.smoothDeltaTime * sensX; 
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.smoothDeltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        camHolder.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);

    }

    public void DoFov(float endValue)
    {
        GetComponent<Camera>().DOFieldOfView(endValue, 0.3f); 
    }

    public void DoTilt(float zTilt)
    {
        transform.DOLocalRotate(new Vector3(0, 0, zTilt), 0.3f); 
    }
}
