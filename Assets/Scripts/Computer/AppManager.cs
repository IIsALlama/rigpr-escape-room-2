using SimpleTwineDialogue;
using System.Collections.Generic;
using UnityEngine;

public class AppManager : MonoBehaviour
{
    public static AppManager instance;
    
    [SerializeField] List<App> apps = new List<App>();
    private App currentApp;
    [SerializeField] private GameObject cctvCanvas;
    [SerializeField] private GameObject staticOverlay;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CloseApp();
        }

    }

    public void OpenApp(int id)
    {
        CloseApp();

        currentApp = apps[id];
        currentApp.appPanel.SetActive(true);

        if (cctvCanvas != null)
            cctvCanvas.SetActive(currentApp.enablesCCTV);
        if (staticOverlay != null)
            staticOverlay.SetActive(currentApp.enablesCCTV);

        if (currentApp.name == "Message")
        {
            TextAdventure.instance.newMessageIcon.SetActive(false);
            TextAdventure.instance.newMessageIconComputer.SetActive(false);
        }
    }

    public void CloseApp()
    {
        if (currentApp == null) return;

        currentApp.appPanel.SetActive(false);

        if (cctvCanvas != null)
            cctvCanvas.SetActive(false);
        if (staticOverlay != null)
            staticOverlay.SetActive(false);


        currentApp = null;
    }
}

[System.Serializable]
public class App
{
    public string name;
    public GameObject appPanel;
    public bool enablesCCTV;
}