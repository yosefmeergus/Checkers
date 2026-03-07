using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceNetwork : NetworkBehaviour
{
    [SyncVar(hook = nameof(HandlePlayerUpdate))]
    private PlayerPiecesHandler player;

    private void HandlePieceCaptured(Vector3 capturedPosition)
    {
        if (transform.position == capturedPosition)
        {
            NetworkServer.Destroy(gameObject);
        }
    }

    public override void OnStartServer()
    {
        player =connectionToClient.identity.GetComponent<PlayerPiecesHandler>();
        Board.Instance.OnPieceCaptured += HandlePieceCaptured;
    }

    public override void OnStopServer()
    {
        Board.Instance.OnPieceCaptured -= HandlePieceCaptured;
    }
   

    private void HandlePlayerUpdate(PlayerPiecesHandler oldPlayer, PlayerPiecesHandler newPlayer)
    {
        transform.parent = newPlayer.PiecesParent;
    }
}
