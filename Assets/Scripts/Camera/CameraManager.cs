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

    private List<string> roomNumbers = new List<string>()
    { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "12", "13", "14", "15", "16", "17", "18" };

    private List<int> cameraLabels;
    private List<RenderTexture> renderTextures;

    private int currentCamera = 0;
    public int CurrentCameraIndex => currentCamera;

    void Start()
    {
        renderTextures = new List<RenderTexture>(cameras.Count);
        for (int i = 0; i < cameras.Count; i++)
        {
            RenderTexture tex = new RenderTexture(1280, 720, 0);
            renderTextures.Add(tex);
            cameras[i].targetTexture = tex;
        }

        roomNumberDropdown.ClearOptions();
        roomNumberDropdown.AddOptions(roomNumbers);

        cameraLabels = new List<int>(cameras.Count);
        for (int i = 0; i < cameras.Count; i++)
            cameraLabels.Add(0);

        textureView.gameObject.SetActive(true);
        ApplyCamera(currentCamera);
    }

    void Update()
    {
        if (cameras == null || cameras.Count == 0) return;

        if (Input.GetKeyDown(KeyCode.RightArrow))
            StepCamera(+1);

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            StepCamera(-1);
    }

    private void StepCamera(int direction)
    {
        currentCamera = (currentCamera + direction) % cameras.Count;
        if (currentCamera < 0) currentCamera += cameras.Count;

        ApplyCamera(currentCamera);
    }

    private void ApplyCamera(int index)
    {
        textureView.texture = renderTextures[index];
        camNumberText.text = "Camera " + (index + 1);

        roomNumberDropdown.value = cameraLabels[index];
    }

    public void OnDropdownChange(TMP_Dropdown change)
    {
        cameraLabels[currentCamera] = change.value;
    }

    public void SetCamera(int index)
    {
        if (cameras == null || cameras.Count == 0) return;

        index = Mathf.Clamp(index, 0, cameras.Count - 1);
        currentCamera = index;

        ApplyCamera(currentCamera);
    }
}