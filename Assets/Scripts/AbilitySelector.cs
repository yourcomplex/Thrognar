using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

/*
	Documentation: https://mirror-networking.com/docs/Guides/NetworkBehaviour.html
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkBehaviour.html
*/

// NOTE: Do not put objects in DontDestroyOnLoad (DDOL) in Awake.  You can do that in Start instead.

public class AbilitySelector : MonoBehaviour
{

    void Start()
    {
        BattleManager.instance.uiManager.abilitySelector = gameObject;
        gameObject.SetActive(false);
    }


}