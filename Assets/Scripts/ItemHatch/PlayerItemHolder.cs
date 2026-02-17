using UnityEngine;

public class PlayerItemHolder : MonoBehaviour
{
    public Transform holdPoint;
    public HoldableItem heldItem;

    public void PickUpItem(HoldableItem item)
    {
        if (heldItem != null) return;

        heldItem = item;
        item.PickUp(holdPoint);
    }

    public void DropItem()
    {
        if (heldItem == null) return;

        heldItem.Drop();
        heldItem = null;
    }

    public void ClearHeldItem()
    {
        heldItem = null;
    }
}
