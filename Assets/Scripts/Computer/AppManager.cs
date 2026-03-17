using SimpleTwineDialogue;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppManager : MonoBehaviour
{
    public static AppManager instance;
    
    [SerializeField] List<App> apps = new List<App>();
    private App currentApp;
    [SerializeField] private GameObject cctvCanvas;
    [SerializeField] private GameObject staticOverlay;

    public bool completedHatchPuzzle = false;

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

        for (int i = 0; i < apps.Count; i++)
        {
            apps[i].icon.color = apps[i].disabled ? new Color(0.2f, 0.2f, 0.2f) : Color.white;
        }
    }

    public void OpenApp(int id)
    {
        CloseApp();

        if (apps[id].disabled)
        {
            return;
        }

        currentApp = apps[id];
        currentApp.appPanel.SetActive(true);

        if (cctvCanvas != null)
            cctvCanvas.SetActive(currentApp.enablesCCTV);
        if (staticOverlay != null)
            staticOverlay.SetActive(currentApp.enablesCCTV);

        if (currentApp.name == "Message")
        {
            TextAdventure.instance.newMessageIcon.SetActive(false);
            //TextAdventure.instance.newMessageIconComputer.SetActive(false);

            apps[0].disabled = false;
            apps[2].disabled = false;

            if (completedHatchPuzzle)
            {
                FindFirstObjectByType<RatSpawner>().ratPuzzleCanStart = true;
            }
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

    public void OpenCCTVToCamera(int cameraIndex, CameraManager camManager, int cctvAppId)
    {
        OpenApp(cctvAppId);
        camManager.SetCamera(cameraIndex);
    }
}

[System.Serializable]
public class App
{
    public string name;
    public Image icon; 
    public GameObject appPanel;
    public bool enablesCCTV;

    public bool disabled;
}