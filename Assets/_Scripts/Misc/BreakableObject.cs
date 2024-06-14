using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    public GameObject brokenGO;
    private bool broken = false;
    [HideInInspector]
    public bool canBeBroken = false;
    public float multiplyScaleAmount = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (brokenGO == null)
        {
            Debug.LogError("Broken gameobject prefab is not assigned.");
            return;
        }
        if(canBeBroken)
        {
            Vector3 goScale = gameObject.transform.localScale;
            GameObject brokenObject = Instantiate(brokenGO, transform.position, transform.rotation);
            brokenObject.transform.localScale = goScale*multiplyScaleAmount;
            Debug.Log("Breakable object collision");
            gameObject.SetActive(false);
        }
    }
}
