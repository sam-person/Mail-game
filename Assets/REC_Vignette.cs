using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class REC_Vignette : Receiver
{
    public enum ToggleType { Toggle, Enable, Disable };
    [SerializeField] private ToggleType toggleType;

    private GameObject mainCamera;
    private PostProcessVolume volume;

    private Vignette vignette = null;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        volume = mainCamera.GetComponent<PostProcessVolume>();

        volume.profile.TryGetSettings(out vignette);
    }

    public override void Activate()
    {
        base.Activate();
        
        switch (toggleType)
        {

            case ToggleType.Toggle:
                vignette.active = !vignette.active;
                Debug.Log("Vignette has toggled");
                break;
            case ToggleType.Enable:
                vignette.active = true;
                Debug.Log("Vignette has enabled");
                break;
            case ToggleType.Disable:
                vignette.active = false;
                Debug.Log("Vignette has disabled");
                break;
        }

    }

}
