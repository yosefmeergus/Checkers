using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnsHandlerNetworked : TurnsHandler
{
    protected override void FillMovesList()
    {
        base.FillMovesList();

    }

    [ClientRpc]
    private void GenerateMovesRpc(Transform playerPiecesParent)
    {
        if (NetworkServer.active) return;
        GenerateMoves(playerPiecesParent);
    }
}
