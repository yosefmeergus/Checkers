using Mirror;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetwork : Player
{
    public static event Action OnPlayerInfoUpdated;
    public static event Action<bool> OnPlayerLobbyOwnerUpdated;
    [SyncVar(hook = nameof(HandleDisplayNameUpdate))]
    private string displayName = "Waiting for player";
    [SyncVar(hook = nameof(HandleLobbyOwnerUpdate))]
    private bool lobbyOwner;



    public string DisplayName
    {
        get => displayName;
        [ServerCallback]
        set => displayName = value; 
    }
    public bool LobbyOwner 
    { 
        get => lobbyOwner;
        [ServerCallback]
        set => lobbyOwner = value;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);   
    }

    private void HandleLobbyOwnerUpdate(bool oldValue, bool newValue)
    {
        if (hasAuthority)
        {
            OnPlayerLobbyOwnerUpdated?.Invoke(newValue);
        }

    }

    private void HandleDisplayNameUpdate(string oldValue, string newValue)
    {
        OnPlayerInfoUpdated?.Invoke();
    }

    public override void OnStartClient()
    {
        if(!isClientOnly)
        {
            return;     
        }
        ((CheckersNetworkManager)(NetworkManager.singleton)).NetworkPlayers.Add(this);
    }

    public override void OnStopClient()
    {
        if (isClientOnly)
        {
            return;
        }
        ((CheckersNetworkManager)(NetworkManager.singleton)).NetworkPlayers.Remove(this);
        OnPlayerInfoUpdated?.Invoke();
    }

    [Command]
    public void CmdNextTurn()
    {
        TurnsHandler.Instance.NextTurn();
    }


}
