using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerParticles : MonoBehaviour
{
    public ParticleSystem particleSystemToPlay;
    public float speedToTriggerParticles = 3;
    public bool playSound = false;
    public AudioClip audioClip; 
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.clip = audioClip;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (particleSystemToPlay != null && other.tag == "Player")
        {
            if (other.GetComponent<StarterAssets.ThirdPersonController>()._speed > speedToTriggerParticles)
            {
                particleSystemToPlay.Play();
                if (playSound && audioClip != null && !audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }
        }
    }
}
