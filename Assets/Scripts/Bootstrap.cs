using UnityEngine;
using UnityEngine.SceneManagement;
public class Bootstrap : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Application.targetFrameRate = 60;
        // Additive load so Bootstrap (with NetworkManager) stays in memory
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
