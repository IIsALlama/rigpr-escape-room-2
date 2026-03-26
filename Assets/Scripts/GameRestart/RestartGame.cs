using SimpleTwineDialogue;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    [SerializeField] private Canvas menu;


    private void Awake()
    {
        menu.enabled = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !menu.enabled)
        {
            menu.enabled = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && menu.enabled)
        {
            menu.enabled = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnApplicationQuit()
    {
        Application.Quit();

    }
}
