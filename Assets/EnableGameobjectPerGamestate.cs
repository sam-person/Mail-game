using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class EnableGameobjectPerGamestate : MonoBehaviour
{
    public List<GameObject> objectsToEnable;

    public List<GameManager.GameState> statesToEnable;


    private void Start()
    {
        GameManager.instance._onGameStateChange += CheckGamestate;
    }

    public void CheckGamestate() {
        foreach (GameObject go in objectsToEnable) {
            if(go)go.SetActive(statesToEnable.Contains(GameManager.instance.gameState));
        }
    }
}
