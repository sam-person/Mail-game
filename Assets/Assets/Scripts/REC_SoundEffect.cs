using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class REC_SoundEffect : Receiver
{
    public AudioSource soundEffect;

    public override void Activate()
    {
        base.Activate();
        soundEffect.Play();
    }
}
