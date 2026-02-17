using UnityEngine;

public class ComputerTerminal : MonoBehaviour
{
    public GameObject appCanvas;
    public GameObject player;
    public bool isOpen = false;



    public void Open()
    {
        if(!isOpen)
        {
            Debug.Log("Opening computer terminal...");
            isOpen = true;
            appCanvas.SetActive(true);
            player.GetComponent<FirstPersonController>().enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void Close()
    {
        if(isOpen)
        {
            Debug.Log("Closing computer terminal...");
            isOpen = false;
            appCanvas.SetActive(false);
            player.GetComponent<FirstPersonController>().enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            AppManager.instance.CloseApp();

        }
    }
}
