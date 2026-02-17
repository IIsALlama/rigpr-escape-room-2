using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private List<Camera> cameras;
    [SerializeField] private RawImage textureView;
    [SerializeField] private TMP_Text camNumberText;
    [SerializeField] private TMP_Dropdown roomNumberDropdown;
    private List<string> roomNumbers = new List<string>() { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "12", "13", "14", "15", "16", "17", "18" };

    private List<int> cameraLabels;
    private List<RenderTexture> renderTextures;
    private int currentCamera = -1;
    public int CurrentCameraIndex => currentCamera;

    void Start()
    {
        renderTextures = new List<RenderTexture>();
        for (int i = 0; i < cameras.Count; i++)
        {
            RenderTexture tex = new RenderTexture(1280, 720, 0);
            renderTextures.Add(tex);
            
            cameras[i].targetTexture = tex;
        }

        cameraLabels = new List<int>();
        roomNumberDropdown.AddOptions(roomNumbers);

        textureView.gameObject.SetActive(true);
        ChangeCamera();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeCamera();
        }
    }

    private void ChangeCamera()
    {
        currentCamera++;
        if (currentCamera == cameras.Count){
            currentCamera = 0;
        }

        textureView.texture = renderTextures[currentCamera];
        camNumberText.text = "Camera " + (currentCamera + 1).ToString();
        roomNumberDropdown.value = cameraLabels[currentCamera];
    }

    public void OnDropdownChange(TMP_Dropdown change)
    {
        cameraLabels[currentCamera] = Convert.ToInt32(change.options[change.value].text);
    }

}