using UnityEngine;
using Unity.Netcode;
public class PlayerSelector : NetworkBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // public override void OnNetworkSpawn()
    // {
    //     base.OnNetworkSpawn();
    //     NetworkManager.OnServerStarted += NetworkManager_OnServerStarted;
    // }

    // private void NetworkManager_OnServerStarted()
    // {
    //     if (!IsOwner) return;
    //     GameManager.onGameStateChanged += GameStateChangedCallback;
    // }

    // private void GameStateChangedCallback(GameManager.State gameState)
    // {
    //     if (!IsOwner) return;
    //     switch (gameState)
    //     {
    //         case GameManager.State.Game:
    //             break;
    //     }
    // }

    // Update is called once per frame
    void Update()
    {
        
    }
}
