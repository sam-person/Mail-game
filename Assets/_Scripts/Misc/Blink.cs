using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{
    public SkinnedMeshRenderer EyeRenderer;

    private SkinnedMeshRenderer lidRenderer = null;
    private int frameCount = 4;

    public float minTime;
    public float maxTime;
    public float speed = 0.03f;


    void Awake()
    {
        lidRenderer = GetComponent<SkinnedMeshRenderer>();
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(blink());
    }

    private IEnumerator blink()
    {
        

        while(gameObject.activeSelf)
        {
            yield return new WaitForSeconds(Random.Range(minTime, maxTime));

            for(int i = 0; i < frameCount; i++)
            {
                yield return new WaitForSeconds(speed * Time.deltaTime);
                SetNewFrame(i);
            }

            Vector2 eyeShift = new Vector2(Random.Range(-0.04f, 0.04f), Random.Range(-0.04f, 0.04f));
            EyeRenderer.material.SetTextureOffset("_MainTex", eyeShift);

            yield return new WaitForSeconds(0.25f);

            for(int j = frameCount -1; j >= 0; j--)
            {
                yield return new WaitForSeconds(speed * Time.deltaTime);
                SetNewFrame(j);
            }
        }
    }

    private void SetNewFrame(int frameIndex)
    {
        Vector2 newOffset = new Vector2(frameIndex * (1.0f / frameCount), 0);
        lidRenderer.material.SetTextureOffset("_MainTex", newOffset);
    }
    
}
