using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Yarn.Unity;
using System.Linq;
using UnityEditor;
using System;
using Sirenix.Utilities;

public class REC_SetYarnVariables : Receiver
{
    [Serializable]
    public class VariableSetter {
        [ValueDropdown("GetYarnVariables", AppendNextDrawer = true), OnValueChanged("SetVariableType")]
        public string variableName;
        public REC_NPC.NPC_DialogueNode.NPC_DialogueCondition.VariableType VariableType = REC_NPC.NPC_DialogueNode.NPC_DialogueCondition.VariableType.String;
        [InfoBox("@GetVerificationString()", VisibleIf = "@GetVerificationString() != \"\"")]
        public string variableValue;

#if UNITY_EDITOR
        private IEnumerable GetYarnVariables()
        {
            YarnProject project = ((GameObject)AssetDatabase.LoadAssetAtPath("Assets/_Prefabs/Bootstrap/[UI].prefab", typeof(GameObject))).GetComponent<Yarn.Unity.DialogueRunner>().yarnProject;
            List<string> variables = project.InitialValues.Keys.ToList<string>();
            for (int i = 0; i < variables.Count; i++)
            {
                yield return variables[i];
            }
            
        }

        private void SetVariableType() {
            YarnProject project = ((GameObject)AssetDatabase.LoadAssetAtPath("Assets/_Prefabs/Bootstrap/[UI].prefab", typeof(GameObject))).GetComponent<Yarn.Unity.DialogueRunner>().yarnProject;
            List<string> variables = project.InitialValues.Keys.ToList<string>();
            if (variables.Contains(variableName))
            {
                project.InitialValues.TryGetValue(variableName, out IConvertible convertible);
                switch (convertible.GetType().ToString()) {
                    case "System.String":
                        VariableType = REC_NPC.NPC_DialogueNode.NPC_DialogueCondition.VariableType.String;
                        break;
                    case "System.Boolean":
                            VariableType = REC_NPC.NPC_DialogueNode.NPC_DialogueCondition.VariableType.Bool;
                        break;
                    case "System.Single":
                        VariableType = REC_NPC.NPC_DialogueNode.NPC_DialogueCondition.VariableType.Int;
                        break;
                }
            }
            else {
                Debug.Log(variableName + " not found");
            }
        }

        private string GetVerificationString() {
            string verification = "";
            if (variableValue.IsNullOrWhitespace()) {
                verification += "Value is empty!\n";
            }
                switch (VariableType)
            {
                case REC_NPC.NPC_DialogueNode.NPC_DialogueCondition.VariableType.String:
                    break;
                case REC_NPC.NPC_DialogueNode.NPC_DialogueCondition.VariableType.Bool:
                    if (!(variableValue == "true" || variableValue == "false")) {
                        verification += "Bool should either be 'true' or 'false'\n";
                    }
                    break;
                case REC_NPC.NPC_DialogueNode.NPC_DialogueCondition.VariableType.Int:
                    if (!int.TryParse(variableValue, out int result)) {
                        verification += variableValue + "Is not an int\n";
                    }
                    break;
            }
            return verification;
        }
#endif
    }

    public List<VariableSetter> variables;

    public override void Activate()
    {
        base.Activate();
        foreach (VariableSetter setter in variables) { 
            GameManager.instance.SetYarnVariable(setter.variableName, setter.variableValue, setter.VariableType);
        
        }
        GameManager.instance._onDynamicYarnVariableChange.Invoke();
    }


}
