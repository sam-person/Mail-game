using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class REC_SetFollowUpTrigger : Receiver //Intended to be used in scripts that would cause an animation lock, thus ensuring we can't accidentally walk away and get a new closest trigger
{

	public Trigger followUpTrigger;

	public override void Activate()
    {
        base.Activate();
		PlayerInteractionHandler.instance.SetFollowUpInteraction(followUpTrigger);
    }
}
