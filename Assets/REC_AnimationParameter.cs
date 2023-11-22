using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class REC_AnimationParameter : Receiver
{
    public Animator anim;
    public enum AnimationParameterType {Float, Int, Bool, Trigger };
    public AnimationParameterType type;

    public string paramater;
    [HideIf("type", AnimationParameterType.Trigger)]
    public string value;
    public override void Activate()
    {
        base.Activate();
        switch (type)
        {
            case AnimationParameterType.Float:
                anim.SetFloat(paramater, float.Parse(value));
                break;
            case AnimationParameterType.Int:
                anim.SetInteger(paramater, int.Parse(value));
                break;
            case AnimationParameterType.Bool:
                anim.SetBool(paramater, bool.Parse(value));
                break;
            case AnimationParameterType.Trigger:
                anim.SetTrigger(paramater);
                break;
        }
    }
}
