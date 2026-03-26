using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private List<Camera> cameras;
    [SerializeField] private Camera ratPuzzleCamera;
    [SerializeField] private RawImage textureView;
    [SerializeField] private TMP_Text camNumberText;
    [SerializeField] private TMP_Dropdown roomNumberDropdown;
    [SerializeField] private GameObject ratButtonEnabled;
    public bool ratCameraEnabled;

    [SerializeField] private RatSpawner ratSpawner;

    private List<string> roomNumbers = new List<string>()
    { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "12", "13", "14", "15", "16", "17", "18" };

    private List<int> cameraLabels;
    private List<RenderTexture> renderTextures;

    private int currentCamera = 0;
    public int CurrentCameraIndex => currentCamera;


    void Start()
    {

        // Rat cam should not be in the cycle at boot
        ratCameraEnabled = false;
        if (ratButtonEnabled != null) ratButtonEnabled.SetActive(false);

        if (ratPuzzleCamera != null)
        {
            // Remove if assigned in the inspector list
            cameras.Remove(ratPuzzleCamera);

            // Ensure it doesnt render anywhere until activated
            ratPuzzleCamera.targetTexture = null;
            ratPuzzleCamera.enabled = false;
            ratPuzzleCamera.gameObject.SetActive(false);
        }

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

        if (Input.GetKeyDown(KeyCode.R))
        {
            ActivateRatPuzzleCamera();
            Debug.Log($"After activate: cameras={cameras.Count}, renderTextures={renderTextures.Count}, labels={cameraLabels.Count}");
        }

    }

    private void StepCamera(int direction)
    {
        currentCamera = (currentCamera + direction) % cameras.Count;
        if (currentCamera < 0) currentCamera += cameras.Count;

        ApplyCamera(currentCamera);
    }

    private void ApplyCamera(int index)
    {
        if (index < 0 || index >= renderTextures.Count)
        {
            Debug.LogError($"ApplyCamera index out of range. index={index}, renderTextures={renderTextures.Count}");
            return;
        }

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
        Debug.Log($"StepCamera() on CameraManager: {gameObject.name} (instanceID {GetInstanceID()}) cameras.Count={cameras.Count}");
        if (cameras == null || cameras.Count == 0) return;

        index = Mathf.Clamp(index, 0, cameras.Count - 1);
        currentCamera = index;

        ApplyCamera(currentCamera);
    }

    public void ActivateRatPuzzleCamera()
    {
        Debug.Log($"ActivateRatPuzzleCamera() on CameraManager: {gameObject.name} (instanceID {GetInstanceID()})");

        if (ratPuzzleCamera == null)
        {
            Debug.LogWarning("Rat Puzzle Camera not assigned in inspector!");
            return;
        }

        Debug.Log($"ActivateRatPuzzleCamera called on {gameObject.name}");

        if (!cameras.Contains(ratPuzzleCamera))
        {
            cameras.Add(ratPuzzleCamera);
            cameraLabels.Add(0);

            var tex = new RenderTexture(1280, 720, 0);
            renderTextures.Add(tex);

            ratPuzzleCamera.gameObject.SetActive(true);
            ratPuzzleCamera.enabled = true;
            ratPuzzleCamera.targetTexture = tex;

            Debug.Log($"Added rat cam at index {cameras.Count - 1}");
        }

        ratCameraEnabled = true;
        if (ratButtonEnabled != null) ratButtonEnabled.SetActive(true);

        StartCoroutine(SwitchToRatCameraNextFrame());
    }


    private IEnumerator SwitchToRatCameraNextFrame()
    {
        yield return null;
        SetCamera(cameras.Count - 1);
        ratSpawner.puzzleEnabled = true;
    }

    public void NextCamButton()
    {
        StepCamera(+1);
    }

    public void PreviousCamButton()
    {
        StepCamera(-1);
    }
}