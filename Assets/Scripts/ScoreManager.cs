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
    }
    /// <summary>
    /// 销毁时取消所有事件订阅，避免内存泄漏和空引用
    /// </summary>
    public override void OnDestroy()
    {
        base.OnDestroy();
        try { NetworkManager.OnServerStarted -= NetworkManager_OnServerStarted; } catch { }
        try { Egg.onFallInWater -= EggFallInWaterCallback; } catch { }
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
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        scoreText.text = "<color=#00FF00>" + hostScore.ToString() + "</color> - <color=#0000FF>" + clientScore.ToString() + "</color>";
    }
}
