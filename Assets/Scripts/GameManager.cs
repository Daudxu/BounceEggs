using UnityEngine;
using Unity.Netcode;
using System;

/// <summary>
/// 游戏管理器：负责游戏状态、网络同步、玩家连接等核心逻辑
/// </summary>
public class GameManager : NetworkBehaviour
{
    /// <summary>单例实例，方便其他脚本全局访问</summary>
    public static GameManager Instance;

    /// <summary>游戏状态枚举：菜单、游戏中、胜利、失败</summary>
    public enum State { Menu, Game, Win, Lose }
    /// <summary>当前游戏状态</summary>
    private State gameState;
    /// <summary>已连接的玩家数量</summary>
    private int connectedPlayers;

    /// <summary>游戏状态变化时触发的事件，其他脚本可订阅以响应状态切换</summary>
    public static event Action<State> onGameStateChanged;

    /// <summary>
    /// 初始化单例：若已有实例则销毁自身，确保场景中只有一个 GameManager
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 销毁时取消所有事件订阅，避免内存泄漏和空引用
    /// </summary>
    public override void OnDestroy()
    {
        base.OnDestroy();
        try
        {
            NetworkManager.OnServerStarted -= NetworkManager_OnServerStarted;
        }
        catch { }

        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= Singleton_OnClientConnectedCallback;
        }
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
        if (!IsServer) return;
        connectedPlayers++;
        NetworkManager.Singleton.OnClientConnectedCallback += Singleton_OnClientConnectedCallback;
    }

    /// <summary>
    /// 有客户端连接时的回调：人数达到 2 人时自动开始游戏。
    /// 排除 Host 连接自己的情况，避免重复触发 StartGame。
    /// </summary>
    private void Singleton_OnClientConnectedCallback(ulong clientId)
    {
        if (clientId == NetworkManager.ServerClientId)
            return;
        connectedPlayers++;
        if (connectedPlayers >= 2)
        {
            StartGame();
        }
    }

    /// <summary>
    /// 开始游戏：通过 ClientRpc 通知所有客户端
    /// </summary>
    private void StartGame()
    {
        StartGameClientRpc();
    }

    /// <summary>
    /// 客户端 RPC：在所有客户端上把状态改为「游戏中」并触发事件
    /// </summary>
    [ClientRpc]
    private void StartGameClientRpc()
    {
        gameState = State.Game;
        onGameStateChanged?.Invoke(gameState);
    }

    /// <summary>
    /// 初始化：将游戏状态设为菜单
    /// </summary>
    void Start()
    {
        gameState = State.Menu;
    }

    public void SetGameState(State gameState)
    {
        this.gameState = gameState;
        onGameStateChanged?.Invoke(gameState);
    }

    void Update()
    {
    }
}
