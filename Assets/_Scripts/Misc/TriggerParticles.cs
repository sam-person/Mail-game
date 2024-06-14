using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerParticles : MonoBehaviour
{
    public ParticleSystem particleSystemToPlay;
    public GameObject ParticleSystemPrefab;
    public float speedToTriggerParticles = 3;
    public bool playSound = false;
    public AudioClip audioClip; 
    private AudioSource audioSource;
    public bool triggerOnExit = false;
    public bool spawnAtTriggerPoint = false;

    private void Start()
    {
        if(!spawnAtTriggerPoint)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            audioSource.clip = audioClip;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayLeafEffect(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if(triggerOnExit)
        {
            PlayLeafEffect(other);
        }
    }

    private void PlayLeafEffect(Collider other)
    {
        if (particleSystemToPlay != null && other.tag == "Player")
        {
            if (other.GetComponent<StarterAssets.ThirdPersonController>()._speed > speedToTriggerParticles)
            {
                if(spawnAtTriggerPoint)
                {
                    GameObject spawnedParticles = Instantiate(ParticleSystemPrefab, other.transform.position, transform.rotation);
                    //other.ClosestPointOnBounds(transform.position)
                    Debug.Log("Spawning leaf particles");
                }
                else
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
}
