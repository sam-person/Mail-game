using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TRI_Time : Trigger
{
    public float LevelTime = 3f;

    bool triggered = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!triggered) {
            if (Time.timeSinceLevelLoad > LevelTime) {
                Activate();
                triggered = true;
            }
        }
    }

    public override void Activate()
    {
        base.Activate();
    }
}
