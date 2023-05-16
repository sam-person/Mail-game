using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class CharacterData : ScriptableObject
{
    public string CharacterName;
    public Color CharacterColor;
    public Color CharacterNameColor;
}

