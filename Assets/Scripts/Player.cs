using UnityEngine;
using Mirror;
using System.Collections.Generic;
using System;

/*
	Documentation: https://mirror-networking.com/docs/Guides/NetworkBehaviour.html
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkBehaviour.html
*/

// NOTE: Do not put objects in DontDestroyOnLoad (DDOL) in Awake.  You can do that in Start instead.

public class Player : NetworkBehaviour
{

    public GameObject uiManagerPrefab;
    public GameObject battleCharPrefab;

    public Transform[] charSpawns;

    [SyncVar]
    public int playerNo;

    [SyncVar]
    public int health, strenth, defense, speed, range, charge;

    public Ability[] abilities;

    [SyncVar]
    public BattleChar target;

    [SyncVar]
    public Ability selectedAbility;

    void Update()
    {
        
        if (isLocalPlayer && BattleManager.instance.activeBattleChar != null)
        {
            //Debug.Log("Clear 1");

            if (BattleManager.instance.activeBattleChar.owner == this)
            {
                BattleManager.instance.uiManagerObject.SetActive(true);
                //Debug.Log("Clear 2");
            }
            else
            {
                BattleManager.instance.uiManagerObject.SetActive(false);     
                //Debug.Log("Else 2");
            }
        }
        
    }


    #region Start & Stop Callbacks

    /// <summary>
    /// This is invoked for NetworkBehaviour objects when they become active on the server.
    /// <para>This could be triggered by NetworkServer.Listen() for objects in the scene, or by NetworkServer.Spawn() for objects that are dynamically created.</para>
    /// <para>This will be called for objects on a "host" as well as for object on a dedicated server.</para>
    /// </summary>
    public override void OnStartServer()
    {

        base.OnStartServer();
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
    public override void OnStartLocalPlayer()
    {
        CmdAddPlayer();
        CmdAddBattleChar();
    }

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

    [Command]
    void CmdAddPlayer()
    {
        //Player player = gameObject.GetComponent<Player>();
        Player player = this;
        BattleManager.instance.players.Add(player);
        if (BattleManager.instance.players.Count == 2)
            BattleManager.instance.BattleStart();
    }

    [Command]
    void CmdAddBattleChar()
    {
        for (int i = 0; i <= 2; i++)
        {
            GameObject battleCharObject = Instantiate(battleCharPrefab, charSpawns[i].position, charSpawns[i].rotation);
            Debug.Log("Instantiated BattleChar prefab");
            BattleChar battleChar = battleCharObject.GetComponent<BattleChar>();
            BattleManager.instance.battleChars.Add(battleChar);
            battleChar.owner = this;
            battleChar.charNo = BattleManager.instance.battleChars.Count;
            battleChar.charName = "Player " + battleChar.charNo;
            NetworkServer.Spawn(battleCharObject);
            Debug.Log("Spawned BattleChar prefab");
            
            
        }
    }

    [Command]
    public void CmdEndMove()
    {
        Debug.Log("Ending Move");
        BattleManager.instance.moveCount++;

        if (BattleManager.instance.moveCount < BattleManager.instance.battleChars.Count)
        {
            BattleManager.instance.activeBattleChar = BattleManager.instance.battleChars[BattleManager.instance.moveCount];
            BattleManager.instance.activePlayer = BattleManager.instance.activeBattleChar.owner;
        }
        else
            BattleManager.instance.NextTurn();
    }

    [Command]
    public void CmdSelectTarget(int charIndex)
    {
        BattleManager.instance.activeBattleChar.target = BattleManager.instance.battleChars[charIndex];
    }

    [Command]
    public void CmdSelectAbility(int abilityIndex)
    {
        BattleManager.instance.activeBattleChar.selectedAbility = BattleManager.instance.activeBattleChar.abilities[abilityIndex];
    }

    [Command]
    //public void CmdActOnTarget(Player initiator, Player target, Ability ability)
    public void CmdActOnTarget()
    {
        Debug.Log("Acting on Target");
        float multiplier = BattleManager.instance.activeBattleChar.strenth / BattleManager.instance.activeBattleChar.target.defense;
        BattleManager.instance.activeBattleChar.target.health += Convert.ToInt32(Math.Floor(multiplier * BattleManager.instance.activeBattleChar.selectedAbility.healthEffect));
    }
}
