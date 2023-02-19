using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowLauncher : MonoBehaviour
{
    public GameObject arrowPrefab;
    public Transform startPosition; 
    private float resetTimer;
    public float resetTime;
    public float timeBeforeAwake;
    public float arrowForce; 
    private bool isAwake = false;


    // Start is called before the first frame update
    void Start()
    {
        resetTimer = resetTime;
    }

    // Update is called once per frame
    void Update()
    {
        //Awake timer
        if (!isAwake)
        {
            timeBeforeAwake -= Time.deltaTime;
        }

        if (timeBeforeAwake <= 0)
        {
            timeBeforeAwake = 100;
            LaunchArrow();
            isAwake = true;
        }



        if (isAwake)
        {
            resetTimer -= Time.deltaTime;
        }

        if (resetTimer <= 0)
        {
            LaunchArrow();
            resetTimer = resetTime;
        }


    }

    private void LaunchArrow()
    {
        GameObject arrow = Instantiate(arrowPrefab, startPosition.position, startPosition.rotation);
        arrow.GetComponent<Arrow>().fromArrowLauncher = this;

    }
}
