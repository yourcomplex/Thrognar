using UnityEngine;
using UnityEngine.UI;
using Mirror;
using System.Collections.Generic;

/*
	Documentation: https://mirror-networking.com/docs/Guides/NetworkBehaviour.html
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkBehaviour.html
*/

// NOTE: Do not put objects in DontDestroyOnLoad (DDOL) in Awake.  You can do that in Start instead.

public class UIManager : NetworkBehaviour
{

    public Button fightButton;
    public Button abilityAButton, abilityBButton, abilityCButton, abilityDButton, abilityEButton, abilityFButton;
    public Button targetAButton, targetBButton, targetCButton, targetDButton, targetEButton, targetFButton; 
    

    public GameObject menu;
    public GameObject abilitySelector;
    public GameObject targetSelector;

    #region Start & Stop Callbacks

    /// <summary>
    /// This is invoked for NetworkBehaviour objects when they become active on the server.
    /// <para>This could be triggered by NetworkServer.Listen() for objects in the scene, or by NetworkServer.Spawn() for objects that are dynamically created.</para>
    /// <para>This will be called for objects on a "host" as well as for object on a dedicated server.</para>
    /// </summary>
    public override void OnStartServer() 
    {

    }

    /// <summary>
    /// Invoked on the server when the object is unspawned
    /// <para>Useful for saving object data in persistent storage</para>
    /// </summary>
    public override void OnStopServer() { }

    /// <summary>
    /// Called on every NetworkBehaviour when it is activated on a client.
    /// <para>Objects on the host have this function called, as there is a local client on the host. The values of SyncVars on object are guaranteed to be initialized correctly with the latest state from the server when this function is called on the client.</para>
    /// </summary>
    public override void OnStartClient() 
    {
        gameObject.SetActive(false);
        BattleManager.instance.uiManager = this;
        BattleManager.instance.uiManagerObject = gameObject;

        //abilitySelector.SetActive(false);
        //targetSelector.SetActive(false);

        fightButton.onClick.AddListener(FightButtonPress);      
        
        abilityAButton.onClick.AddListener(AbilityAPress);
        abilityBButton.onClick.AddListener(AbilityBPress);
        abilityCButton.onClick.AddListener(AbilityCPress);
        abilityDButton.onClick.AddListener(AbilityDPress);
        abilityEButton.onClick.AddListener(AbilityEPress);
        abilityFButton.onClick.AddListener(AbilityFPress);

        targetAButton.onClick.AddListener(TargetAPress);
        targetBButton.onClick.AddListener(TargetBPress);
        targetCButton.onClick.AddListener(TargetCPress);
        targetDButton.onClick.AddListener(TargetDPress);
        targetEButton.onClick.AddListener(TargetEPress);
        targetFButton.onClick.AddListener(TargetFPress);

    }

    /// <summary>
    /// This is invoked on clients when the server has caused this object to be destroyed.
    /// <para>This can be used as a hook to invoke effects or do client specific cleanup.</para>
    /// </summary>
    public override void OnStopClient() { }

    /// <summary>
    /// Called when the local player object has been set up.
    /// <para>This happens after OnStartClient(), as it is triggered by an ownership message from the server. This is an appropriate place to activate components or functionality that should only be active for the local player, such as cameras and input.</para>
    /// </summary>
    public override void OnStartLocalPlayer() { }

    /// <summary>
    /// This is invoked on behaviours that have authority, based on context and <see cref="NetworkIdentity.hasAuthority">NetworkIdentity.hasAuthority</see>.
    /// <para>This is called after <see cref="OnStartServer">OnStartServer</see> and before <see cref="OnStartClient">OnStartClient.</see></para>
    /// <para>When <see cref="NetworkIdentity.AssignClientAuthority">AssignClientAuthority</see> is called on the server, this will be called on the client that owns the object. When an object is spawned with <see cref="NetworkServer.Spawn">NetworkServer.Spawn</see> with a NetworkConnection parameter included, this will be called on the client that owns the object.</para>
    /// </summary>
    public override void OnStartAuthority() { }

    /// <summary>
    /// This is invoked on behaviours when authority is removed.
    /// <para>When NetworkIdentity.RemoveClientAuthority is called on the server, this will be called on the client that owns the object.</para>
    /// </summary>
    public override void OnStopAuthority() { }

    #endregion


    public void FightButtonPress()
    {
        BattleManager.instance.uiManager.menu.SetActive(false);
        BattleManager.instance.uiManager.abilitySelector.SetActive(true);
        BattleManager.instance.uiManager.abilitySelector.SetActive(true);
        Debug.Log("You pressed the FIGHT button.");
    }
    public void AbilityAPress()
    {
        Debug.Log("You pressed the ABILITY A button.");
        BattleManager.instance.activePlayer.CmdSelectAbility(0);
        BattleManager.instance.uiManager.abilitySelector.SetActive(false);
        BattleManager.instance.uiManager.targetSelector.SetActive(true);
    }

    public void AbilityBPress()
    {
        Debug.Log("You pressed the ABILITY B button.");
        BattleManager.instance.activePlayer.CmdSelectAbility(1);
        BattleManager.instance.uiManager.abilitySelector.SetActive(false);
        BattleManager.instance.uiManager.targetSelector.SetActive(true);
    }

    public void AbilityCPress()
    {
        Debug.Log("You pressed the ABILITY C button.");
        BattleManager.instance.activePlayer.CmdSelectAbility(2);
        BattleManager.instance.uiManager.abilitySelector.SetActive(false);
        BattleManager.instance.uiManager.targetSelector.SetActive(true);
    }

    public void AbilityDPress()
    {
        Debug.Log("You pressed the ABILITY D button.");
        BattleManager.instance.activePlayer.CmdSelectAbility(3);
        BattleManager.instance.uiManager.abilitySelector.SetActive(false);
        BattleManager.instance.uiManager.targetSelector.SetActive(true);
    }

    public void AbilityEPress()
    {
        Debug.Log("You pressed the ABILITY E button.");
        BattleManager.instance.activePlayer.CmdSelectAbility(4);
        BattleManager.instance.uiManager.abilitySelector.SetActive(false);
        BattleManager.instance.uiManager.targetSelector.SetActive(true);
    }

    public void AbilityFPress()
    {
        Debug.Log("You pressed the ABILITY F button.");
        BattleManager.instance.activePlayer.CmdSelectAbility(5);
        BattleManager.instance.uiManager.abilitySelector.SetActive(false);
        BattleManager.instance.uiManager.targetSelector.SetActive(true);
    }

    public void TargetAPress()
    {
        Debug.Log("You pressed the TARGET A button.");
        BattleManager.instance.activePlayer.CmdSelectTarget(0);
        BattleManager.instance.activePlayer.CmdActOnTarget();
        BattleManager.instance.uiManager.targetSelector.SetActive(false);
        BattleManager.instance.activePlayer.CmdEndMove();
        BattleManager.instance.uiManager.menu.SetActive(true);
    }
    public void TargetBPress()
        {
            Debug.Log("You pressed the TARGET A button.");
            BattleManager.instance.activePlayer.CmdSelectTarget(1);
            BattleManager.instance.activePlayer.CmdActOnTarget();
            BattleManager.instance.uiManager.targetSelector.SetActive(false);
            BattleManager.instance.activePlayer.CmdEndMove();
    }
    public void TargetCPress()
    {
        Debug.Log("You pressed the TARGET A button.");
        BattleManager.instance.activePlayer.CmdSelectTarget(2);
        BattleManager.instance.activePlayer.CmdActOnTarget();
        BattleManager.instance.uiManager.targetSelector.SetActive(false);
        BattleManager.instance.activePlayer.CmdEndMove();
    }
    public void TargetDPress()
    {
        Debug.Log("You pressed the TARGET A button.");
        BattleManager.instance.activePlayer.CmdSelectTarget(3);
        BattleManager.instance.activePlayer.CmdActOnTarget();
        BattleManager.instance.uiManager.targetSelector.SetActive(false);
        BattleManager.instance.activePlayer.CmdEndMove();
    }
    public void TargetEPress()
    {
        Debug.Log("You pressed the TARGET A button.");
        BattleManager.instance.activePlayer.CmdSelectTarget(4);
        BattleManager.instance.activePlayer.CmdActOnTarget();
        BattleManager.instance.uiManager.targetSelector.SetActive(false);
        BattleManager.instance.activePlayer.CmdEndMove();
    }
    public void TargetFPress()
    {
        Debug.Log("You pressed the TARGET A button.");
        BattleManager.instance.activePlayer.CmdSelectTarget(5);
        BattleManager.instance.activePlayer.CmdActOnTarget();
        BattleManager.instance.uiManager.targetSelector.SetActive(false);
        BattleManager.instance.activePlayer.CmdEndMove();
    }

    public void SetAbilityButtons()
    {
        //abilityAButton.text = BattleManager.instance.activeBattleChar.abilities[0].abilityName;    
    }
}
