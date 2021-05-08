using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSelector : MonoBehaviour
{
    


    // Start is called before the first frame update
    void Start()
    {
        BattleManager.instance.uiManager.targetSelector = gameObject;
        gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
