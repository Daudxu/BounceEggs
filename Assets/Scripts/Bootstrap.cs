using UnityEngine;
using UnityEngine.SceneManagement;
public class Bootstrap : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Application.targetFrameRate = 60;  // 设置目标帧率为 60 FPS
        SceneManager.LoadScene(1);  // 加载索引为 1 的 主场景
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
