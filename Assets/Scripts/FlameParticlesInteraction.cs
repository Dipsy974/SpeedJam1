using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameParticlesInteraction : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.GetComponent<PlayerMovement>() != null)
        {
            other.gameObject.GetComponent<PlayerMovement>().Respawn();
        }

    }
}
