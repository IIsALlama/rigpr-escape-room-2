using UnityEngine;

public class BluePrintInteractor : MonoBehaviour
{
    public GameObject bluePrintCanvas;
    public GameObject player;

    public bool isOpen = false;

    public void Open()
    {
        if (!isOpen)
        {
            Debug.Log("BluePrint open");
            isOpen = true;
            bluePrintCanvas.SetActive(true);
            player.GetComponent<FirstPersonController>().enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void Close()
    {
        if (isOpen)
        {
            Debug.Log("BluePrint closed");
            isOpen = false;
            bluePrintCanvas.SetActive(false);
            player.GetComponent<FirstPersonController>().enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

        }
    }
}
