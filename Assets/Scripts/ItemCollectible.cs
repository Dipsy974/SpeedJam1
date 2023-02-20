using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollectible : MonoBehaviour
{

    public string powerUnlocked;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.GetComponent<CapsuleCollider>() != null)
        {
            PlayerMovement player = other.transform.gameObject.transform.parent.GetComponent<PlayerMovement>();

            player.SetItemInRange(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<CapsuleCollider>() != null)
        {
            PlayerMovement player = other.transform.gameObject.transform.parent.GetComponent<PlayerMovement>();

            player.ClearItemInRange();
        }
    }

    public void SetInactive()
    {
        transform.parent.gameObject.SetActive(false); 
    }
}
