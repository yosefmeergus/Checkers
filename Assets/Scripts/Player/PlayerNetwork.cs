using Mirror;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetwork : Player
{
    public static event Action OnPlayerInfoUpdated;
    [SyncVar(hook = nameof(HandleDisplayNameUpdate))]
    private string displayName = "Waiting for player";

    public string DisplayName
    {
        get => displayName;
        [ServerCallback]
        set => displayName = value; 
    }

    private void HandleDisplayNameUpdate(string oldValue, string newValue)
    {
        print("display name synced");
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
}
