using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;

public class NavmeshTest : MonoBehaviour
{
    public enum NavType {FollowPlayer, Nodes }
    public NavType navType;

    NavMeshAgent agent;

    public float nodeDistanceThreshold = 0.2f;
    [FoldoutGroup("Nodes"), ShowIf("navType", NavType.Nodes)] public List<NavNode> navNodes;
    [FoldoutGroup("Nodes"), ShowIf("navType", NavType.Nodes)] public bool loop = true;
    [FoldoutGroup("Nodes"), ShowIf("navType", NavType.Nodes), ShowInInspector, ReadOnly] int nodeIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (navType)
        {
            case NavType.FollowPlayer:
                if (Vector3.Distance(transform.position, PlayerInteractionHandler.instance.transform.position) > nodeDistanceThreshold) { 
                    agent.SetDestination(PlayerInteractionHandler.instance.transform.position);
                }
                break;
            case NavType.Nodes:
                if (Vector3.Distance(transform.position, navNodes[nodeIndex].transform.position) < nodeDistanceThreshold)
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
        Gizmos.color = Color.blue;
        for (int i = 0; i < navNodes.Count - 1; i++) {
            Gizmos.DrawLine(navNodes[i].transform.position, navNodes[i + 1].transform.position);
        }
        if (loop) { 
            Gizmos.DrawLine(navNodes[navNodes.Count - 1].transform.position, navNodes[0].transform.position);
        }
    }
}
