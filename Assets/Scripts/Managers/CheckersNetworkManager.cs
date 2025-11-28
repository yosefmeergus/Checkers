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
    [SerializeField]
    private List<PlayerNetwork> networkPlayers = new List<PlayerNetwork>();

    public List<PlayerNetwork> NetworkPlayers { get => networkPlayers; set => networkPlayers = value; }

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        OnClientConnected?.Invoke();
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        SceneManager.LoadScene("Lobby Scene");
        Destroy(gameObject);
    }

    public override void OnServerAddPlayer(NetworkConnection connection)
    {
        GameObject playerInstance = Instantiate(playerPrefab);
        NetworkServer.AddPlayerForConnection(connection, playerInstance);   
        PlayerNetwork player = playerInstance.GetComponent<PlayerNetwork>();
        player.IsWhite = numPlayers == 1;
        networkPlayers.Add(player);
        player.DisplayName = player.IsWhite ? "White" : "black";
    }
    public override void OnServerDisconnect(NetworkConnection connection)
    {
        base.OnServerDisconnect(connection);
        if(connection.identity == null)
        {
            return;
        }
        PlayerNetwork player = connection.identity.GetComponent<PlayerNetwork>();
        networkPlayers.Remove(player);

    }

    public override void OnStopServer()
    {
        networkPlayers.Clear();
    }
}
