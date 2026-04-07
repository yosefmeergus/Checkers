using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using UnityEngine.ProBuilder;

public class GameOverHandlerNetworked : GameOverHandler
{
    public override void OnStartServer()
    {
        TurnsHandler.Instance.OnGameOver += HandleGameOver;
    }

    public override void OnStopServer()
    {
        TurnsHandler.Instance.OnGameOver -= HandleGameOver;
    }

    [ClientRpc]
    private void RpcGameOver(string result)
    {
        CallGameOver(result);
    }

    [ServerCallback]
    private void HandleGameOver(string result)
    {
        RpcGameOver(result);
    }
}
