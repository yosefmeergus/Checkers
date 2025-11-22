using Mirror;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckersNetworkManager : NetworkManager
{
    [SerializeField] GameObject gameOverHandlerPrefab, boardPrefab, 
        turnsHandlerPrefab;
    public static event Action OnClientConnected;
    private List<PlayerNetwork> networkPlayers = new List<PlayerNetwork>();

    public List<PlayerNetwork> NetworkPlayers { get => networkPlayers; set => networkPlayers = value; }

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        OnClientConnected?.Invoke();
    }

    public override void OnServerAddPlayer(NetworkConnection connection)
    {
        GameObject playerInstance = Instantiate(playerPrefab);
        NetworkServer.AddPlayerForConnection(connection, playerInstance);   
        PlayerNetwork player = playerInstance.GetComponent<PlayerNetwork>();
        player.IsWhite = numPlayers == 1;
        player.DisplayName = player.IsWhite ? "White" : "black";
        networkPlayers.Add(player);
    }
    public override void OnServerDisconnect(NetworkConnection connection)
    {
        base.OnServerDisconnect(connection);
        PlayerNetwork player = connection.identity.GetComponent<PlayerNetwork>();
        networkPlayers.Remove(player);

    }

    public override void OnStopServer()
    {
        networkPlayers.Clear();
    }
}
