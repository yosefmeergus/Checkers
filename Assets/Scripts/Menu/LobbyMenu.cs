using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyMenu : MonoBehaviour
{
    [SerializeField] Button startGameButton;
    [SerializeField] Text[] playerNameTexts = new Text[2];

    public void StartGame()
    {
        NetworkManager.singleton.ServerChangeScene("Game Scene");
    }

    private void OnEnable()
    {
        PlayerNetwork.OnPlayerInfoUpdated += HandlePlayerInfoUpdated;
        PlayerNetwork.OnPlayerLobbyOwnerUpdated += HandlePlayerLobbyOwnerUpdated;
    }

    private void HandlePlayerInfoUpdated()
    {
        List<PlayerNetwork> players = ((CheckersNetworkManager)(NetworkManager.singleton)).NetworkPlayers;
        for(int i = 0; i<players.Count; i++)
        {
            playerNameTexts[i].text = players[i].DisplayName;
        }
        for(int i = players.Count; i<playerNameTexts.Length; i++)
        {
            playerNameTexts[i].text = "Waiting for player";
        }
        startGameButton.interactable = players.Count == 2;
    }

    private void HandlePlayerLobbyOwnerUpdated(bool isLobbyOwner)
    {
        startGameButton.gameObject.SetActive(isLobbyOwner);
    }

    private void OnDisable()
    {
        PlayerNetwork.OnPlayerInfoUpdated -= HandlePlayerInfoUpdated;
    }
}
