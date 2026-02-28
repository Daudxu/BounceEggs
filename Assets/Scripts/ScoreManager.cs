using UnityEngine;
using Unity.Netcode;
using TMPro;

/// <summary>
/// 得分管理器：监听鸡蛋落水事件，根据当前回合给对手加分，并通过 ClientRpc 同步到所有客户端。
/// </summary>
public class ScoreManager : NetworkBehaviour
{
    [Header("Elements")]
    [SerializeField] private TextMeshProUGUI scoreText;  // 分数显示文本
    [SerializeField] private int hostScore;              // Host 分数（绿色）
    [SerializeField] private int clientScore;             // Client 分数（蓝色）

    /// <summary>
    /// 网络对象生成时：订阅服务器启动事件。
    /// </summary>
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        NetworkManager.OnServerStarted += NetworkManager_OnServerStarted;
    }

    /// <summary>
    /// 服务器启动后的回调：仅服务端订阅鸡蛋落水与游戏状态变化事件。
    /// 鸡蛋落水由 Egg.ServerRpc 在服务端触发，故需服务端订阅。
    /// </summary>
    private void NetworkManager_OnServerStarted()
    {
        if (!IsServer) return;
        Egg.onFallInWater += EggFallInWaterCallback;
        GameManager.onGameStateChanged += GameStateChangedCallback;
    }

    /// <summary>
    /// 销毁时取消所有事件订阅，避免内存泄漏和空引用。
    /// </summary>
    public override void OnDestroy()
    {
        base.OnDestroy();
        try { NetworkManager.OnServerStarted -= NetworkManager_OnServerStarted; } catch { }
        try { Egg.onFallInWater -= EggFallInWaterCallback; } catch { }
        try { GameManager.onGameStateChanged -= GameStateChangedCallback; } catch { }
    }

    /// <summary>
    /// 游戏状态变化回调：进入 Game 时重置分数。
    /// </summary>
    private void GameStateChangedCallback(GameManager.State gameState)
    {
        switch (gameState)
        {
            case GameManager.State.Game:
                ResetScores();
                break;
        }
    }

    /// <summary>
    /// 重置分数为 0，并广播到所有客户端。
    /// </summary>
    private void ResetScores()
    {
        hostScore = 0;
        clientScore = 0;
        UpdateScoreClientRpc(hostScore, clientScore);
        UpdateScoreText();
    }

    private void CheckForEndGame()
    {
        if (hostScore >= 3)
        {
            
        }
        else if (clientScore >= 3)
        {

        }
        else
        {
            ReuseEgg();
        }
    }

    private void ReuseEgg()
    {
        if (EggManager.instance != null)
            EggManager.instance.ReuseEgg();
    }

    void Start()
    {
        // 网络未启动时直接更新本地 UI，避免调用 ClientRpc
        RefreshScoreTextLocally();
    }

    void Update()
    {
    }

    /// <summary>
    /// 鸡蛋落水回调：当前控球方是对手，给对手加分。
    /// IsHostTurn 为 true 表示 Host 在控球，则 Client 得分；否则 Host 得分。
    /// </summary>
    private void EggFallInWaterCallback()
    {
        if (PlayerSelector.instance.IsHostTurn())
        {
            clientScore++;
        }
        else
        {
            hostScore++;
        }
        UpdateScoreClientRpc(hostScore, clientScore);
        UpdateScoreText();
        CheckForEndGame();
    }

    /// <summary>
    /// 客户端 RPC：在所有客户端上同步 hostScore 和 clientScore。
    /// </summary>
    [ClientRpc]
    private void UpdateScoreClientRpc(int hostScore, int clientScore)
    {
        this.hostScore = hostScore;
        this.clientScore = clientScore;
    }

    /// <summary>
    /// 更新分数显示：网络已启动时广播 ClientRpc，否则仅更新本地 UI。
    /// </summary>
    private void UpdateScoreText()
    {
        if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsListening)
            UpdateScoreTextClientRpc();
        else
            RefreshScoreTextLocally();
    }
    

    /// <summary>
    /// 仅更新本地 UI，不发送 RPC。用于 Start 或网络未就绪时。
    /// </summary>
    private void RefreshScoreTextLocally()
    {
        if (scoreText != null)
            scoreText.text = "<color=#00FF00>" + hostScore + "</color> - <color=#0000FF>" + clientScore + "</color>";
    }

    /// <summary>
    /// 客户端 RPC：在所有客户端上更新 UI 分数文本（绿色 Host - 蓝色 Client）。
    /// </summary>
    [ClientRpc]
    private void UpdateScoreTextClientRpc()
    {
        RefreshScoreTextLocally();
    }
}
