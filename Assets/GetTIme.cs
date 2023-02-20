using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetTIme : MonoBehaviour
{

    public TextMeshProUGUI timerText;

    // Start is called before the first frame update
    void Start()
    {
        string saveTime = Timer.timeSaved;
        timerText.text = saveTime;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
