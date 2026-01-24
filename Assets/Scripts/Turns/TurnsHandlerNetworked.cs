using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnsHandlerNetworked : TurnsHandler
{
    public override void OnStartServer()
    {
        PlayerPiecesHandler.OnPiecesSpawned += NextTurn;
        Players = ((CheckersNetworkManager)NetworkManager.singleton).Players;
    }

    public override void OnStopServer()
    {
        PlayerPiecesHandler.OnPiecesSpawned -= NextTurn;
    }

    protected override void FillMovesList()
    {
        base.FillMovesList();
        GenerateMovesRpc(piecesHandler.PiecesParent);
    }

    [ClientRpc]
    private void GenerateMovesRpc(Transform playerPiecesParent)
    {
        if (NetworkServer.active) return;
        GenerateMoves(playerPiecesParent);
    }
}
