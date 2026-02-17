using UnityEngine;

public class OpenComputer : MonoBehaviour
{
    public GameObject appCanvas;
    public GameObject textHelp;
    public GameObject player;

    public bool isOpen = false;

    public bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && isOpen == false)
        {
            isOpen = true;
            appCanvas.SetActive(true);
            player.GetComponent<FirstPersonController>().enabled = false;
            textHelp.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

        }
        else if (playerInRange && Input.GetKeyDown(KeyCode.E) && isOpen)
        {
            isOpen = false;
            appCanvas.SetActive(false);
            player.GetComponent<FirstPersonController>().enabled = true;
            textHelp.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            AppManager.instance.CloseApp();
        }



    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            textHelp.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            textHelp.SetActive(false);
        }
    }
}