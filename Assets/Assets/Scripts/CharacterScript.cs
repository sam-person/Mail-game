using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : MonoBehaviour
{

    public List<DialogueData> DialogueEMorning;
    public List<DialogueData> DialogueMMorning;
    public List<DialogueData> DialogueLunchTime;
    public List<DialogueData> DialogueAfternoon;
    public List<DialogueData> DialogueEvening;

    public CharacterData data;


    public bool characterIsTalking;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        
    }
}
