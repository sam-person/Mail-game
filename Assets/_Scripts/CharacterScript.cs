using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : Receiver
{

    public string YarnNode;

    public Animator animator;

    public override void Activate()
    {
        base.Activate();
        InterfaceManager.instance.StartDialogue(YarnNode);
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
