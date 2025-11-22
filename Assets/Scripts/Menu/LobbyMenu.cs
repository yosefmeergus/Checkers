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
        
    }

    private void OnEnable()
    {
        PlayerNetwork.OnPlayerInfoUpdated += HandlePlayerInfoUpdated;
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
    }

    private void OnDisable()
    {
        PlayerNetwork.OnPlayerInfoUpdated -= HandlePlayerInfoUpdated;
    }
}
