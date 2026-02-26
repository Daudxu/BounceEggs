using UnityEngine;
using Unity.Netcode;

public class UlManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject connectionPanel;
    [SerializeField] private GameObject waitingPanel;
    [SerializeField] private GameObject gamePanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ShowConnectionPanel(GameObject panel)
    {
        connectionPanel.SetActive(true);
        waitingPanel.SetActive(false);
        gamePanel.SetActive(false);
    }

    private void ShowWaitingPanel(GameObject panel)
    {
        connectionPanel.SetActive(false);
        waitingPanel.SetActive(true);
        gamePanel.SetActive(false);
    }

    private void ShowGamePanel(GameObject panel)
    {
        connectionPanel.SetActive(false);
        waitingPanel.SetActive(false);
        gamePanel.SetActive(true);
    }

    public void HostButtonCallback() 
    {
        if (NetworkManager.Singleton == null)
        {
            Debug.LogError("NetworkManager not found. Ensure Bootstrap scene loads first.");
            return;
        }
        NetworkManager.Singleton.StartHost();
        ShowWaitingPanel(waitingPanel);
    }
    
    public void ClientButtonCallback() 
    {
        if (NetworkManager.Singleton == null)
        {
            Debug.LogError("NetworkManager not found. Ensure Bootstrap scene loads first.");
            return;
        }
        NetworkManager.Singleton.StartClient();
        ShowWaitingPanel(waitingPanel);
    }
}
