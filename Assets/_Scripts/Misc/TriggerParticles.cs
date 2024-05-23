using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerParticles : MonoBehaviour
{
    public ParticleSystem particleSystemToPlay;
    public float speedToTriggerParticles = 3;

    private void OnTriggerEnter(Collider other)
    {
        if (particleSystemToPlay != null && other.tag == "Player")
        {
            if(other.GetComponent<StarterAssets.ThirdPersonController>()._speed > speedToTriggerParticles)
            particleSystemToPlay.Play();
        }
    }
}
