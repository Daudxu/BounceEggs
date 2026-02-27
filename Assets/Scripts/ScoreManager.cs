using UnityEngine;
using Unity.Netcode;
using TMPro;

public class ScoreManager : NetworkBehaviour
{
    [Header("Elements")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private int hostScore;
    [SerializeField] private int clientScore;

    
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
        Egg.onFallInWater += EggFallInWaterCallback;
        GameManager.onGameStateChanged += GameStateChangedCallback;
    }
    /// <summary>
    /// 销毁时取消所有事件订阅，避免内存泄漏和空引用
    /// </summary>
    public override void OnDestroy()
    {
        base.OnDestroy();
        try { NetworkManager.OnServerStarted -= NetworkManager_OnServerStarted; } catch { }
        try { Egg.onFallInWater -= EggFallInWaterCallback; } catch { }
        try { GameManager.onGameStateChanged -= GameStateChangedCallback; } catch { }
    }

    private void GameStateChangedCallback(GameManager.State gameState)
    {
        switch (gameState)
        {
            case GameManager.State.Game:
                ResetScores();
                break;
        }
    }

    private void ResetScores()
    {
        hostScore = 0;
        clientScore = 0;
        UpdateScoreClientRpc(hostScore, clientScore);
        UpdateScoreText();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateScoreText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
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
    }

    [ClientRpc]
    private void UpdateScoreClientRpc(int hostScore, int clientScore)
    {
        this.hostScore = hostScore;
        this.clientScore = clientScore;
    }

    private void UpdateScoreText()
    {
        UpdateScoreTextClientRpc();
    }

    [ClientRpc]
    private void UpdateScoreTextClientRpc()
    {
        scoreText.text = "<color=#00FF00>" + hostScore.ToString() + "</color> - <color=#0000FF>" + clientScore.ToString() + "</color>";
    }
}
