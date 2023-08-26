using UnityEngine;

public class LightProbeGroupSwitcher : MonoBehaviour
{
    public LightProbeGroup[] lightProbeGroups; // An array of Light Probe Groups to switch between.
    public int activeGroupIndex = 0; // The index of the currently active Light Probe Group.

    private void Start()
    {
        if (activeGroupIndex >= 0 && activeGroupIndex < lightProbeGroups.Length)
        {
            SetActiveLightProbeGroup(activeGroupIndex);
        }
        else
        {
            Debug.LogError("Invalid activeGroupIndex. Make sure it's within the valid range.");
        }
    }

    public void SetActiveLightProbeGroup(int groupIndex)
    {
        if (groupIndex >= 0 && groupIndex < lightProbeGroups.Length)
        {
            for (int i = 0; i < lightProbeGroups.Length; i++)
            {
                lightProbeGroups[i].gameObject.SetActive(i == groupIndex);
            }

            activeGroupIndex = groupIndex;

            Debug.Log("Switched to Light Probe Group index: " + groupIndex);
        }
        else
        {
            Debug.LogError("Invalid groupIndex. Make sure it's within the valid range.");
        }
    }
}