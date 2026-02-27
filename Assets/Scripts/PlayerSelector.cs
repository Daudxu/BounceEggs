using UnityEngine;
using Unity.Netcode;
public class PlayerSelector : NetworkBehaviour
{
    private bool isHostTurn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    /// <summary>
    /// 网络对象生成时：订阅「服务器已启动」事件
    /// </summary>
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        NetworkManager.OnServerStarted += NetworkManager_OnServerStarted;
    }

    /// <summary>
    /// 服务器启动后的回调：仅服务端执行，订阅「客户端连接」事件
    /// </summary>
    private void NetworkManager_OnServerStarted()
    {
        if (!IsOwner) return;
        GameManager.onGameStateChanged += GameStateChangedCallback;
        Egg.onHit += SwitchPlayer;
    }
    /// <summary>
    /// 销毁时取消所有事件订阅，避免内存泄漏和空引用
    /// </summary>
    public override void OnDestroy()
    {
        base.OnDestroy();
        try { NetworkManager.OnServerStarted -= NetworkManager_OnServerStarted; } catch { }
        try { GameManager.onGameStateChanged -= GameStateChangedCallback; } catch { }
        try { Egg.onHit -= SwitchPlayer; } catch { }
    }

    /// <summary>
    /// 游戏状态改变时的回调：仅服务端执行，订阅「客户端连接」事件
    /// </summary>
    private void GameStateChangedCallback(GameManager.State gameState)
    {
        if (!IsOwner) return;
        switch (gameState)
        {
            case GameManager.State.Game:
                Initialize();
                break;
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Initialize()
    {
        //查找游戏中的每个玩家

       PlayerStateManager[] playerStateManagers = FindObjectsByType<PlayerStateManager>(FindObjectsSortMode.None);
       for(int i = 0; i < playerStateManagers.Length; i++)
       {
          if (playerStateManagers[i].GetComponent<NetworkObject>().OwnerClientId == NetworkManager.ServerClientId)
          {
                if(isHostTurn)
                {
                    playerStateManagers[i].Enable();
                }    
                else
                {
                    playerStateManagers[i].Disable();
                }
          }
          else
          {
                if(isHostTurn)
                {
                    playerStateManagers[i].Disable();
                }    
                else
                {
                    playerStateManagers[i].Enable();
                }
          }
       }

    }

    private void SwitchPlayer()
    {
        isHostTurn = !isHostTurn;
        Initialize();
    }
}
