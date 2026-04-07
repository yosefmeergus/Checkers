using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardNetwork : Board
{
    private readonly SyncList<int[]> boardList = new SyncList<int[]>();
    public override IList<int[]> BoardList => boardList;

    public override event Action<Vector3> OnPieceCaptured;

    public override void OnStartServer()
    {
        FillBoardList(boardList);
        PieceMovementHandlerNetwork.OnPieceReachedBackline += TryPromotePieceOnBoard;
    }

    public override void OnStopServer()
    {
        PieceMovementHandlerNetwork.OnPieceReachedBackline -= TryPromotePieceOnBoard;
    }

    [ServerCallback]
    private bool TryPromotePieceOnBoard(PiecePromotionHandler piece, int x, int z)
    {
        PromotePieceOnBoard(BoardList, x, z);
        RpcPromotePieceOnBoard(x, z);
        return true;
    }

    [ClientRpc]
    private void RpcPromotePieceOnBoard(int x, int z)
    {
        if (NetworkServer.active)
        {
            return;
        }
        PromotePieceOnBoard(BoardList, x, z);
    }

    [ServerCallback]
    public override void MoveOnBoard(Vector2Int oldPosition, Vector2Int newPosition, bool nextTurn)
    {
        MoveOnBoard(boardList, oldPosition, newPosition);
        RpcMoveOnBoard(oldPosition, newPosition, nextTurn);
    }

    [ClientRpc]
    private void RpcMoveOnBoard(Vector2Int oldPosition, Vector2Int newPosition, bool nextTurn)
    {
        if(NetworkServer.active) return;
        MoveOnBoard(boardList, oldPosition, newPosition);
        if (nextTurn)
        {
            NetworkClient.connection.identity.GetComponent<PlayerNetwork>().CmdNextTurn();
        }
    }
    [ServerCallback]
    public override void CaptureOnBoard(Vector2Int piecePosition)
    {
        Capture(boardList, piecePosition);
        RpcCaptureOnBoard(piecePosition);
        OnPieceCaptured?.Invoke(new Vector3(piecePosition.x, 0, piecePosition.y));
    }

    [ClientRpc]
    private void RpcCaptureOnBoard(Vector2Int piecePosition)
    {
        Capture(boardList, piecePosition);
    }
}
