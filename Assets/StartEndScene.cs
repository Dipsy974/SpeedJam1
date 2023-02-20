using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartEndScene : MonoBehaviour
{
    public Timer timer;

        private void OnTriggerEnter(Collider other)
    {

        if (other.GetComponent<CapsuleCollider>() != null)
        {
            timer.GetTimeSaved();
           SceneManager.LoadScene("EndScreen");
        }
    }
}

