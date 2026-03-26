using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public void GamePlay()
    {
        SceneManager.LoadScene("SecurityCameraScene");
    }
}
