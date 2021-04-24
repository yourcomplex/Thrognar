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

    [SyncVar]
    public int playerNo;

    [SyncVar]
    public int health, strenth, defense, speed, range, charge;

    public Ability[] abilities;

    [SyncVar]
    public Player target;

    [SyncVar]
    public Ability selectedAbility;

    void LateUpdate()
    {
        if (isLocalPlayer && BattleManager.instance.activePlayer)
        {

            if (BattleManager.instance.activePlayer == this)
            {
                BattleManager.instance.menu.SetActive(true);
            }
            else
            {
                BattleManager.instance.menu.SetActive(false);
                BattleManager.instance.abilitySelector.SetActive(false);
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
        //instance = this;

        // Flips the second player object so it faces left
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        if (playerNo == 2)
        {
            spriteRenderer.flipX = true;
        }

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
        Player player = gameObject.GetComponent<Player>();
        BattleManager.instance.players.Add(player);
        if (BattleManager.instance.players.Count == 2)
            BattleManager.instance.BattleStart();
    }

    [Command]
    public void CmdEndMove()
    {
        Debug.Log("You have pressed the FIGHT button.");

        BattleManager.instance.moveCount++;

        if (BattleManager.instance.moveCount < BattleManager.instance.players.Count)
            BattleManager.instance.activePlayer = BattleManager.instance.players[BattleManager.instance.moveCount];
        else
            BattleManager.instance.NextTurn();
    }

    [Command]
    public void CmdSelectTarget()
    {
        for (int i=0; i<BattleManager.instance.players.Count; i++)
        {
            // Selects the player that is not this player as the target
            // Only works for two players, need to update for additional players
            if (BattleManager.instance.players[i] != this)
                target = BattleManager.instance.players[i];
        }
    }

    [Command]
    public void CmdSelectAbility()
    {
        selectedAbility = BattleManager.instance.activePlayer.abilities[0];
    }

    [Command]
    //public void CmdActOnTarget(Player initiator, Player target, Ability ability)
    public void CmdActOnTarget()
    {

        Debug.Log("Acting on Target");
        float multiplier = BattleManager.instance.activePlayer.strenth / BattleManager.instance.activePlayer.target.defense;
        BattleManager.instance.activePlayer.target.health += Convert.ToInt32(Math.Floor(multiplier * BattleManager.instance.activePlayer.selectedAbility.healthEffect));

    }
}
