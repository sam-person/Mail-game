using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Cinemachine;
using UnityEditor;
using Yarn.Unity;
using System.Linq;
using System;
using UnityEngine.AI;

public class REC_NPC : Receiver
{
    public NPCDefinition NPCDefinition;
    public CinemachineVirtualCamera dialogueCamera;
    public bool doTalkingAnimation = false;
    [ShowIf("doTalkingAnimation")]public Animator anim;
    

    public List<NPC_DialogueNode> primaryDialogue;
    public List<NPC_DialogueNode> secondaryDialogue;

    [System.Serializable]
    public class NPC_DialogueNode {
        [ValueDropdown("GetYarnNodes", AppendNextDrawer = true)]
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

#if UNITY_EDITOR
        private IEnumerable GetYarnNodes()
        {
            YarnProject project = ((GameObject)AssetDatabase.LoadAssetAtPath("Assets/_Prefabs/Bootstrap/[UI].prefab", typeof(GameObject))).GetComponent<Yarn.Unity.DialogueRunner>().yarnProject;
            List<string> nodes = project.NodeNames.ToList<string>();
            for (int i = 0; i < nodes.Count; i++)
            {
                yield return nodes[i];
            }

        }
#endif

        [System.Serializable]
        public class NPC_DialogueCondition {
            [HorizontalGroup("var"), LabelWidth(50), ValueDropdown("GetYarnVariables", AppendNextDrawer = true)]
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
                        Single intResult = GameManager.instance.GetYarnVariable<Single>(variable);
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

#if UNITY_EDITOR
            private IEnumerable GetYarnVariables()
            {
                YarnProject project = AssetDatabase.LoadAssetAtPath("Assets/_Prefabs/Bootstrap/[UI].prefab", typeof(GameObject)).GetComponent<Yarn.Unity.DialogueRunner>().yarnProject;
                List<string> variables = project.InitialValues.Keys.ToList<string>();
                for (int i = 0; i < variables.Count; i++)
                {
                    yield return variables[i];
                }
            }

            
#endif
        }
    }

    public override void Activate()
    {
        base.Activate();
        //go through each primary dialogue and activate the first one that is valid
        foreach (NPC_DialogueNode node in primaryDialogue) {
            if (node.GetIsValid()) {
                StartDialogueNode(node);
                return;
            }
        }

        //create a new list to shuffle into
        List<NPC_DialogueNode> shuffledList = new List<NPC_DialogueNode>();
        shuffledList.AddRange(secondaryDialogue);
        shuffledList.Shuffle();

        //go through secondary nodes
        foreach (NPC_DialogueNode node in shuffledList)
        {
            if (node.GetIsValid())
            {
                StartDialogueNode(node);
                return;
            }
        }

        //we ran out of dialogue!
        Debug.Log("Ran out of dialogue!");
    }

    void StartDialogueNode(NPC_DialogueNode node) {
        if (useNavAgent) agent.isStopped = true;
        GameManager.instance.currentNPC = this;
        node.triggered = true;
        InterfaceManager.instance.StartDialogue(node.YarnNode, dialogueCamera, NPCDefinition);
        
    }

    public void OnDialogueLine() {
        if (doTalkingAnimation && anim != null) anim.SetTrigger("Talk");
    }

    private void Awake()
    {
        if (dialogueCamera != null) dialogueCamera.gameObject.SetActive(false);
        if (useNavAgent && agent == null) { 
            useNavAgent = false;
            Debug.LogError(name + " has nav disable due to no agent assigned!");
        }
    }

    private void Update()
    {
        if (useNavAgent) HandleNav();
    }

    public void OnDialogueEnd() {
        if (useNavAgent) agent.isStopped = false;
        //if (doTalkingAnimation && anim != null) {
        //    anim.SetBool("Talking", false);
        //}
    }


    #region NAVIGATION

    [FoldoutGroup("Navigation")]
    public bool useNavAgent = false;
    [FoldoutGroup("Navigation"), ShowIf("useNavAgent")] public NavMeshAgent agent;

    public enum NavType { FollowPlayer, Nodes }
    [FoldoutGroup("Navigation"), ShowIf("useNavAgent")] public NavType navType;

    [FoldoutGroup("Navigation"), ShowIf("useNavAgent")] public float distanceThreshold = 0.2f;
    [FoldoutGroup("Navigation/Nodes"), ShowIf("@(navType == NavType.Nodes) && useNavAgent", NavType.Nodes)] public List<NavNode> navNodes;
    [FoldoutGroup("Navigation/Nodes"), ShowIf("@(navType == NavType.Nodes) && useNavAgent", NavType.Nodes)] public bool loop = true;
    [FoldoutGroup("Navigation/Nodes"), ShowIf("@(navType == NavType.Nodes) && useNavAgent", NavType.Nodes), ShowInInspector, ReadOnly] int nodeIndex = 0;

    void HandleNav() {
        switch (navType)
        {
            case NavType.FollowPlayer:
                if (Vector3.Distance(transform.position, PlayerInteractionHandler.instance.transform.position) > distanceThreshold)
                {
                    agent.SetDestination(PlayerInteractionHandler.instance.transform.position);
                }
                break;
            case NavType.Nodes:
                if (Vector3.Distance(transform.position, navNodes[nodeIndex].transform.position) < distanceThreshold)
                {
                    nodeIndex++;
                    if (nodeIndex >= navNodes.Count) nodeIndex = 0;
                }
                agent.SetDestination(navNodes[nodeIndex].transform.position);
                break;
            default:
                break;
        }
    }

    private void OnDrawGizmos()
    {
        if (!useNavAgent || navType != NavType.Nodes) return;
        Gizmos.color = Color.blue;
        for (int i = 0; i < navNodes.Count - 1; i++)
        {
            Gizmos.DrawLine(navNodes[i].transform.position, navNodes[i + 1].transform.position);
        }
        if (loop)
        {
            Gizmos.DrawLine(navNodes[navNodes.Count - 1].transform.position, navNodes[0].transform.position);
        }
    }
    #endregion
}

public static class IListExtensions
{
    /// <summary>
    /// Shuffles the element order of the specified list.
    /// </summary>
    public static void Shuffle<T>(this IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }
}
