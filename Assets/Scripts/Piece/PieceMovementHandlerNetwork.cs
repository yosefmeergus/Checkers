using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceMovementHandlerNetwork : PieceMovementHandler
{
    public override void OnStartAuthority()
    {
        TilesSelectionHandler.OnTileSelected += HandleTileSelected;
    }

    public override void OnStopClient()
    {
        TilesSelectionHandler.OnTileSelected -= HandleTileSelected;
    }

    protected override void Move(Vector3 position, bool nextTurn)
    {
        CmdMove(position, nextTurn);
    }

    [Command]
    private void CmdMove(Vector3 position, bool nextTurn)
    {
        base.Move(position, nextTurn);
    }

    protected override void Capture(Vector2Int position)
    {
        CmdCapture(position);
    }

    [Command]
    private void CmdCapture(Vector2Int position)
    {
        base.Capture(position);
    }
}
