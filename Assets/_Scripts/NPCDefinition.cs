using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "New_NPC", menuName = "NPC")]
public class NPCDefinition : ScriptableObject
{
    public string NPCName = "NPC";
    public Color fontColour = Color.white;
    public TMP_FontAsset font;
    public Color backgroundColor = new Color(0.9245283f, 0.5712887f, 0.6278643f, 1f);
    public Color outlineColor = Color.white;
    public Color leftWhiskerColor = Color.white;
    public Color rightWhiskerColor = Color.white;



}
