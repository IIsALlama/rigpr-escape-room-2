using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class Browser : MonoBehaviour
{
    public static Browser instance;
    [SerializeField] List<Webpage> pages = new List<Webpage>();
    [SerializeField] TMP_InputField searchBar;
    private Webpage currentPage;
    
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
        
    }

    public void CloseWebsite()
    {
        if (currentPage == null) return;

        currentPage.websitePanel.SetActive(false);
    }

    public void OpenWebsite(string name)
    {
        for (int i = 0; i < pages.Count; i++)
        {
            if (pages[i].name == name)
            {
                CloseWebsite();

                currentPage = pages[i];
                pages[i].websitePanel.SetActive(true);
                return;
            }
        }
    }
}

[System.Serializable]
public class Webpage
{
    public string name;
    public GameObject websitePanel;
}