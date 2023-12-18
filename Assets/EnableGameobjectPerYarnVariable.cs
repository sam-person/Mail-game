using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static REC_NPC.NPC_DialogueNode;

public class EnableGameobjectPerYarnVariable : MonoBehaviour
{
    public List<REC_NPC.NPC_DialogueNode.NPC_DialogueCondition> conditions;

    public enum CheckType {Dynamic, Update, OnEnable };
    public CheckType whenToCheck = CheckType.Dynamic;

    public List<GameObject> objectsToToggle;

    public void ToggleObjects(bool state) { 
        foreach(GameObject obj in objectsToToggle) {
            if(obj) obj.SetActive(state);
        }
    }

    bool CheckConditions() {
        foreach (NPC_DialogueCondition condition in conditions)
        {
            if (!condition.Resolve())
            {
                Debug.Log("Condition Failed: " + condition.variable);
                return false;
            }
        }
        return true;
    }

    private void Update()
    {
        if (whenToCheck == CheckType.Update) {
            ToggleObjects(CheckConditions());
        }
    }

    private void OnEnable()
    {
        if (whenToCheck == CheckType.OnEnable)
        {
            ToggleObjects(CheckConditions());
        }


    }

    private void Start()
    {
        if (whenToCheck == CheckType.Dynamic) {
            GameManager.instance._onDynamicYarnVariableChange += OnDynamicYarnVariableChange;
        }
    }

    public void OnDynamicYarnVariableChange() {
        ToggleObjects(CheckConditions());
    }
}
