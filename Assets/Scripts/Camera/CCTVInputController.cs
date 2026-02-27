using UnityEngine;
using UnityEngine.EventSystems;

public class CCTVInputController : MonoBehaviour
{
    public CameraManager cameraManager;
    public RoutingApp routingApp;

    [Header("Rat puzzle hatch")]
    [SerializeField] private int ratPuzzleCameraIndex = 3; // 0-based
    [SerializeField] private RatHatch ratHatch;
    [SerializeField] private bool ratHatchUsesLeftClick = true;

    void Update()
    {
        if (cameraManager == null) { Debug.LogWarning("[CCTV] cameraManager NULL", this); return; }
        if (!Input.GetMouseButtonDown(0)) return;

        int camIndex = cameraManager.CurrentCameraIndex;
        Debug.Log($"[CCTV] Click camIndex={camIndex}", this);

        if (ratHatchUsesLeftClick && camIndex == ratPuzzleCameraIndex)
        {
            Debug.Log($"[CCTV] Rat cam click. ratHatch={(ratHatch ? ratHatch.name : "NULL")}", this);
            ratHatch?.Dispense();
            return;
        }

        if (routingApp == null) { Debug.LogWarning("[CCTV] routingApp NULL", this); return; }
        if (!routingApp.manualDrop) { Debug.Log("[CCTV] manualDrop is FALSE", this); return; }

        DropFromCurrentCamera();
    }

    public void DropFromCurrentCamera()
    {
        int camIndex = cameraManager.CurrentCameraIndex;
        int containerID = camIndex + 1;

        ContainerSlot target = ContainerManager.Instance.GetContainer(containerID);
        if (target == null)
        {
            Debug.LogWarning($"[CCTV] No ContainerSlot for containerID={containerID}. Check IDs.", this);
            return;
        }

        Debug.Log($"[CCTV] Eject from containerID={containerID} hasItem={target.HasItem()}", target);
        target.EjectItem();
    }

}