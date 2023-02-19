using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrowing : MonoBehaviour
{
    public ParticleSystem flameParticles; 
    public ParticleSystem smokeParticles; 
    public float durationTime;
    private float resetTimer;
    public float resetTime;
    public float timeBeforeAwake; 
    private bool isOn;
    private bool isAwake = false; 
   

    // Start is called before the first frame update
    void Start()
    {
        resetTimer = resetTime;
        StopFlame(); 
    }

    // Update is called once per frame
    void Update()
    {
        //Awake timer
        if (!isAwake)
        {
            timeBeforeAwake -= Time.deltaTime;
        }

        if(timeBeforeAwake <= 0)
        {
            timeBeforeAwake = 100; 
            StartFlame(); 
            isAwake = true; 
        }
   
        

        if (!isOn && isAwake)
        {
            resetTimer -= Time.deltaTime;
        }

        if(resetTimer <= 0)
        {
            StartFlame();
            resetTimer = resetTime; 
        }


    }

    private void StartFlame()
    {
        isOn = true; 

        flameParticles.Play();
        smokeParticles.Play();

        Invoke(nameof(StopFlame), durationTime);
    }

    private void StopFlame()
    {
        isOn = false; 

        flameParticles.Stop();
        smokeParticles.Stop();
    }

}
