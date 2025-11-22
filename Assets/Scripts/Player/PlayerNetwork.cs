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
    private string displayName;

    public string DisplayName
    {
        get => displayName;
        [ServerCallback]
        set => displayName = value; 
    }

    private void HandleDisplayNameUpdate(string oldValue, string newValue)
    {
        OnPlayerInfoUpdated?.Invoke();
    }
}
