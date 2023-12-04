using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Yarn.Unity;
using System.Linq;
using UnityEditor;
using Unity.VisualScripting;

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
    }

#if UNITY_EDITOR
    private IEnumerable GetYarnVariables()
    {
        YarnProject project = AssetDatabase.LoadAssetAtPath("Assets/_Prefabs/Bootstrap/[UI].prefab", typeof(GameObject)).GetComponent<Yarn.Unity.DialogueRunner>().yarnProject;
        List<string> variables = project.InitialValues.Keys.ToList<string>();
        for (int i = 0; i < variables.Count; i++) { 
            yield return variables[i];
        }
    }
#endif

}
