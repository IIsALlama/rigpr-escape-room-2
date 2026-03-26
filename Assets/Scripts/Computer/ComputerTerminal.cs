using UnityEngine;

public class ComputerTerminal : MonoBehaviour
{
    public GameObject appCanvas;
    public GameObject player;
    public bool isOpen = false;

    [SerializeField] private StaticScam staticScam;
 

    public void Open()
    {
        if(!isOpen)
        {
            Debug.Log("Opening computer terminal...");
            staticScam.isStatic = true;
            AudioManager.instance.Play("Static");
            AudioManager.instance.Play("LightHum");

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
            staticScam.isStatic = false;
            AudioManager.instance.Stop("Static");
            AudioManager.instance.Stop("LightHum");

            isOpen = false;
            appCanvas.SetActive(false);
            player.GetComponent<FirstPersonController>().enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            AppManager.instance.CloseApp();

        }
    }
}
