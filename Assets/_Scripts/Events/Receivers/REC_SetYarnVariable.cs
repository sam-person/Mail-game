using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Yarn.Unity;
using System.Linq;
using UnityEditor;
using System;

/// <summary>
/// This is a deprecated component that only sets one variable. Use REC_SetYarnVariables
/// </summary>
[Obsolete("This is a deprecated component that only sets one variable. Use REC_SetYarnVariables")]
public class REC_SetYarnVariable : Receiver
{


    [ValueDropdown("GetYarnVariables", AppendNextDrawer = true)]
    public string variableName;
    public string variableValue;
    public REC_NPC.NPC_DialogueNode.NPC_DialogueCondition.VariableType VariableType = REC_NPC.NPC_DialogueNode.NPC_DialogueCondition.VariableType.String;

    public override void Activate()
    {
        base.Activate();
        GameManager.instance.SetYarnVariable(variableName, variableValue, VariableType);
        GameManager.instance._onDynamicYarnVariableChange.Invoke();
    }

#if UNITY_EDITOR
    private IEnumerable GetYarnVariables()
    {
        YarnProject project = ((GameObject)AssetDatabase.LoadAssetAtPath("Assets/_Prefabs/Bootstrap/[UI].prefab", typeof(GameObject))).GetComponent<DialogueRunner>().yarnProject;
        List<string> variables = project.InitialValues.Keys.ToList<string>();
        for (int i = 0; i < variables.Count; i++) { 
            yield return variables[i];
        }
    }
#endif

}
