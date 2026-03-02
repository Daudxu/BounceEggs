using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using Unity.Netcode.Transports.UTP;
/// <summary>
/// UI 管理器：负责连接面板、等待面板、游戏面板的切换，以及 Host/Client 按钮的网络连接逻辑。
/// </summary>
public class UlManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject connectionPanel;   // 连接面板：显示 Host/Client 按钮
    [SerializeField] private GameObject waitingPanel;   // 等待面板：连接中或等待其他玩家
    [SerializeField] private GameObject gamePanel;      // 游戏面板：主游戏界面
    [SerializeField] private GameObject winPanel;      // 胜利面板：显示胜利信息
    [SerializeField] private GameObject losePanel;      // 失败面板：显示失败信息


    void Start()
    {
        ShowConnectionPanel();
        GameManager.onGameStateChanged += GameStateChangedCallback;
    }
    
    private void OnDestroy()
    {
        GameManager.onGameStateChanged -= GameStateChangedCallback;
    }

    private void GameStateChangedCallback(GameManager.State gameState)
    {
        switch (gameState)
        {
            case GameManager.State.Game:
                ShowGamePanel();
                break;
            case GameManager.State.Win:
                ShowWinPanel();
                break;
            case GameManager.State.Lose:
                ShowLosePanel();
                break;
        }
    }

    /// <summary>
    /// 显示连接面板，隐藏其他面板。
    /// </summary>
    private void ShowConnectionPanel()
    {
        connectionPanel.SetActive(true);
        waitingPanel.SetActive(false);
        gamePanel.SetActive(false);
        winPanel.SetActive(false);
        losePanel.SetActive(false);
    }

    private void ShowWinPanel()
    {
        winPanel.SetActive(true);
    }

    private void ShowLosePanel()
    {
        losePanel.SetActive(true);
    }

    /// <summary>
    /// 显示等待面板，隐藏其他面板。
    /// </summary>
    private void ShowWaitingPanel()
    {
        connectionPanel.SetActive(false);
        waitingPanel.SetActive(true);
        gamePanel.SetActive(false);
    }

    /// <summary>
    /// 显示游戏面板，隐藏其他面板。
    /// </summary>
    private void ShowGamePanel()
    {
        connectionPanel.SetActive(false);
        waitingPanel.SetActive(false);
        gamePanel.SetActive(true);
    }

    /// <summary>
    /// Host 按钮回调：创建主机（服务器+客户端），并切换到等待面板。
    /// </summary>
    public void HostButtonCallback()
    {
        NetworkManager.Singleton.StartHost();
        ShowWaitingPanel();
    }

    /// <summary>
    /// Client 按钮回调：作为客户端连接到主机，并切换到等待面板。
    /// </summary>
    public void ClientButtonCallback()
    {
        string ipAddress = IPManager.instance.GetInputIP();
        UnityTransport utp = NetworkManager.Singleton.GetComponent<UnityTransport>();
        utp.SetConnectionData(ipAddress, 7777);

        NetworkManager.Singleton.StartClient();
        ShowWaitingPanel();
    }

    public void NextButtonCallback()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        NetworkManager.Singleton.Shutdown();
        // NetworkManager.Singleton.Shutdown();
        // ShowConnectionPanel();
    }
}
