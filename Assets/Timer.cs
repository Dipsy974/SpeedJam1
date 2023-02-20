using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float currentTime;
    private bool timerEnded = false;

    TMP_Text textMesh;
    Mesh mesh;
    Vector3[] vertices; 

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!timerEnded)
        {
            currentTime += Time.deltaTime;
        }

        int minutes = (int)currentTime / 60;
        float seconds = (int)currentTime % 60;
        float milliseconds = (int)((currentTime % 1) * 1000);

        string minutesText = minutes.ToString("00");
        string secondsText = seconds.ToString("00");
        string millisecondsText = milliseconds.ToString("000");

        timerText.text = minutesText + ":" + secondsText + ":" + millisecondsText;


        timerText.ForceMeshUpdate();
        mesh = timerText.mesh;
        vertices = mesh.vertices; 

        for(int i = 0; i < vertices.Length; i++)
        {
            Vector3 offset = Wobble(Time.time + i);
            vertices[i] = vertices[i] + offset; 
        }

        mesh.vertices = vertices;
        timerText.canvasRenderer.SetMesh(mesh); 
    }

    Vector2 Wobble(float time)
    {
        return new Vector2(Mathf.Sin(time * 5f), Mathf.Cos(time * 2.5f)); 
    }
}
