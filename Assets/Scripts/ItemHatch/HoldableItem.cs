using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HoldableItem : MonoBehaviour
{
    private Rigidbody rb;

    public bool isHeld;
    public bool isStored;

    public ItemType itemType;

    public ContainerSlot storedInContainer;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();    
    }

    public void PickUp(Transform holdPoint)
    {
        isHeld = true;
        isStored = false;
        storedInContainer = null;

        rb.isKinematic = true;
        rb.useGravity = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        gameObject.SetActive(true);
    }

    public void Drop()
    {
        isHeld = false;
        storedInContainer = null;

        transform.SetParent(null);

        rb.isKinematic = false;
        rb.useGravity = true;

        gameObject.SetActive(true);
    }

    public void Store(ContainerSlot container)
    {
        isHeld = false;
        isStored = true;
        storedInContainer = container;

        transform.SetParent(null);

        rb.isKinematic = true;
        rb.useGravity = false;

        gameObject.SetActive(false);
    }

    public void DeleteItem()
    {
        storedInContainer = null;
        Destroy(gameObject);
    }
}
