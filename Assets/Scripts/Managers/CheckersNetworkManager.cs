using Mirror;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckersNetworkManager : NetworkManager
{
    [SerializeField] GameObject gameOverHandlerPrefab, boardPrefab, turnsHandlerPrefab;
    public static event Action OnClientConnected;
    public static event Action OnGameStarted;
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
        player.LobbyOwner = numPlayers == 1;

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

    public override void OnStartServer()
    {
        GameObject board = Instantiate(boardPrefab);
        NetworkServer.Spawn(board);
        GameObject turnsHandler = Instantiate(turnsHandlerPrefab);
        NetworkServer.Spawn(turnsHandler);
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        if(sceneName == "Game Scene")
        {
            GameObject gameOverHandler = Instantiate(gameOverHandlerPrefab);
            NetworkServer.Spawn(gameOverHandler);
            OnGameStarted?.Invoke();
        }
    }

    public override void OnStopServer()
    {
        networkPlayers.Clear();
    }
}
