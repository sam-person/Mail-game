using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


public class NavNode : MonoBehaviour
{
    public enum NodeType {None, Wait, Animation };
    public NodeType nodeType;

    [ShowIf("nodeType", NodeType.Wait)] public float waitTime;

    public void Visit() {
        if (GetComponent<TRI_NavNode>() != null)
        {
            GetComponent<TRI_NavNode>().Activate();
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 0.15f);
        Gizmos.DrawLine(transform.position, transform.position + (transform.forward * 0.5f));

        switch (nodeType)
        {
            case NodeType.None:
                break;
            case NodeType.Wait:
                UnityEditor.Handles.Label(transform.position + Vector3.up, "Wait: " + waitTime.ToString("F2"));
                break;
            case NodeType.Animation:
                break;
        }
    }
#endif
}
