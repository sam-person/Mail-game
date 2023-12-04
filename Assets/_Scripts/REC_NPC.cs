using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Cinemachine;

public class REC_NPC : Receiver
{
    public NPCDefinition NPCDefinition;
    public CinemachineVirtualCamera dialogueCamera;

    public List<NPC_DialogueNode> primaryDialogue;
    public List<NPC_DialogueNode> secondaryDialogue;

    [System.Serializable]
    public class NPC_DialogueNode {
        public string YarnNode = "";
        [HorizontalGroup("os")]
        public bool oneShot = true;
        [ReadOnly, HorizontalGroup("os")]public bool triggered = false;
        public List<NPC_DialogueCondition> conditions;

        public bool GetIsValid() {
            //if we're oneshot and triggered, return false
            if (oneShot && triggered) return false;

            foreach (NPC_DialogueCondition condition in conditions) {
                if (!condition.Resolve()) return false;
            }

            return true;
        }

        

        [System.Serializable]
        public class NPC_DialogueCondition {
            [HorizontalGroup("var"), LabelWidth(50)]
            public string variable;
            public enum VariableType {String, Bool, Int };
            [HorizontalGroup("var", 100), HideLabel]
            public VariableType type = VariableType.String;
            public enum StringComparison { Equals, NotEqual };
            [ShowIf("type", VariableType.String), HideLabel]
            public StringComparison stringComparison = StringComparison.Equals;

            public enum IntComparison { Equals, NotEqual, MoreThan, LessThan };
            [ShowIf("type", VariableType.Int), HideLabel]
            public IntComparison intComparison = IntComparison.Equals;

            public enum BoolComparison { True, False };
            [ShowIf("type", VariableType.Bool), HideLabel]
            public BoolComparison boolComparison = BoolComparison.True;

            [HideIf("type", VariableType.Bool), LabelWidth(50)]
            public string value;

            public bool Resolve() {
                Debug.Log("Resolving Condition " + variable + " " + type.ToString() + " " + value);
                switch (type)
                {
                    case VariableType.String:
                        string stringResult = GameManager.instance.GetYarnVariable<string>(variable);
                        Debug.Log("Got yarn string variable " + variable + " as " + stringResult);
                        switch (stringComparison)
                        {
                            case StringComparison.Equals:
                                return stringResult == value;
                            case StringComparison.NotEqual:
                                return stringResult != value;
                        }
                        break;
                    case VariableType.Bool:
                        bool boolResult = GameManager.instance.GetYarnVariable<bool>(variable);
                        Debug.Log("Got yarn bool variable " + variable + " as " + boolResult.ToString());
                        switch (boolComparison)
                        {
                            case BoolComparison.True:
                                return boolResult;
                            case BoolComparison.False:
                                return !boolResult;
                        }
                        break;
                    case VariableType.Int:
                        int intResult = GameManager.instance.GetYarnVariable<int>(variable);
                        Debug.Log("Got yarn int variable " + variable + " as " + intResult.ToString());
                        switch (intComparison)
                        {
                            case IntComparison.Equals:
                                return intResult == int.Parse(value);
                            case IntComparison.NotEqual:
                                return intResult != int.Parse(value);
                            case IntComparison.MoreThan:
                                return intResult > int.Parse(value);
                            case IntComparison.LessThan:
                                return intResult < int.Parse(value);
                        }
                        break;
                }
                return false;
            }
        }
    }

    public override void Activate()
    {
        base.Activate();
        //go through each primary dialogue and activate the first one that is valid
        foreach (NPC_DialogueNode node in primaryDialogue) {
            if (node.GetIsValid()) {
                node.triggered = true;
                InterfaceManager.instance.StartDialogue(node.YarnNode, dialogueCamera, NPCDefinition);
                return;
            }
        }

        //go through secondary nodes //TODO: make random
        foreach (NPC_DialogueNode node in secondaryDialogue)
        {
            if (node.GetIsValid())
            {
                node.triggered = true;
                InterfaceManager.instance.StartDialogue(node.YarnNode, dialogueCamera, NPCDefinition);
                return;
            }
        }

        //we ran out of dialogue!
        Debug.Log("Ran out of dialogue!");
    }

    private void Awake()
    {
        if (dialogueCamera != null) dialogueCamera.gameObject.SetActive(false);
    }
}
