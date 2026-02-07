using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceNetwork : NetworkBehaviour
{
    [SyncVar(hook = nameof(HandlePlayerUpdate))]
    private PlayerPiecesHandler player;

    public override void OnStartServer()
    {
        player =connectionToClient.identity.GetComponent<PlayerPiecesHandler>();
    }

    private void HandlePlayerUpdate(PlayerPiecesHandler oldPlayer, PlayerPiecesHandler newPlayer)
    {
        transform.parent = newPlayer.PiecesParent;
    }
}
