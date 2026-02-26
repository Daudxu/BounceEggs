using UnityEngine;
using UnityEngine.SceneManagement;
public class Bootstrap : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Application.targetFrameRate = 60;  // ÉèÖÃÄ¿±êÖ¡ÂÊÎª 60 FPS
        SceneManager.LoadScene(1);  // ¼ÓÔØË÷ÒýÎª 1 µÄ Ö÷³¡¾°
    }
}
