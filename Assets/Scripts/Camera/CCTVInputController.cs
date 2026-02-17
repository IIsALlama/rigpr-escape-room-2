using UnityEngine;

public class CCTVInputController : MonoBehaviour
{
    public CameraManager cameraManager;
    public RoutingApp routingApp;

    void Update()
    {
        if (routingApp == null || cameraManager == null) return;

        if (!routingApp.manualDrop) return;

        if (routingApp.manualDrop == true && Input.GetMouseButtonDown(0))
        {
            DropFromCurrentCamera();
        }
    }

    public void DropFromCurrentCamera()
    {
        int camIndex = cameraManager.CurrentCameraIndex;
        int containerID = camIndex + 1;

        ContainerSlot target = ContainerManager.Instance.GetContainer(containerID);
        if (target == null) return;

        target.EjectItem();
    }
}
