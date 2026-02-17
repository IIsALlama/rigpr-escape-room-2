using UnityEngine;

public class RoutingApp : MonoBehaviour
{
    public int mainContainerID = 0;
    //For switching the hatch system
    public bool manualDrop = false;
    public int lastDestinationID = -1;


    public void SendItemToContainer(int destinationID)
    {
        lastDestinationID = destinationID;
        ContainerSlot main = ContainerManager.Instance.GetContainer(mainContainerID);
        ContainerSlot target = ContainerManager.Instance.GetContainer(destinationID);

        if (main == null || target == null) return;
        if (!main.HasItem()) return;

        HoldableItem item = main.RemoveItem();
        target.ReceiveItem(item);
    }

    public void DropLast()
    {
        if(lastDestinationID < 0) return;

        ContainerSlot target = ContainerManager.Instance.GetContainer(lastDestinationID);
        if(target == null) return;

        target.EjectItem();
    }
}
