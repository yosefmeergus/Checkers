using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinLobbyMenu : MonoBehaviour
{
    [SerializeField] GameObject onlinePage;
    [SerializeField] InputField addressInput;

    private void Start()
    {
        CheckersNetworkManager.OnClientConnected += HandleClientConnection;
    }

    private void OnDestroy()
    {
        CheckersNetworkManager.OnClientConnected -= HandleClientConnection;
    }

    public void Join()
    {
        NetworkManager.singleton.networkAddress = addressInput.text;
        NetworkManager.singleton.StartClient();
    }

    private void HandleClientConnection()
    {
        onlinePage.SetActive(false);
        gameObject.SetActive(false);
    }
}
