using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


public class AbilitySelector : MonoBehaviour
{

    void Start()
    {
        BattleManager.instance.uiManager.abilitySelector = gameObject;
        gameObject.SetActive(false);
    }


}