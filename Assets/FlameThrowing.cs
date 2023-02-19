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
    private bool isOn; 
   

    // Start is called before the first frame update
    void Start()
    {
        resetTimer = resetTime; 
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOn)
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
