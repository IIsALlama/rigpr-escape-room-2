using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Interaction")]
    public float interactRange = 3f;
    public LayerMask interactLayer;


    public GameObject interactHelper;
    [SerializeField] private ComputerTerminal computerTerminal;
    [SerializeField] private BluePrintInteractor bluePrintInteractor;

    private Camera cam;
    private PlayerItemHolder itemHolder;

    void Awake()
    {
        cam = Camera.main;
        itemHolder = GetComponent<PlayerItemHolder>();
        computerTerminal = FindAnyObjectByType<ComputerTerminal>();
        bluePrintInteractor = FindAnyObjectByType<BluePrintInteractor>();
    }

    void Update()
    {
        Debug.DrawRay(
            cam.transform.position,
            cam.transform.forward * interactRange,
            Color.green
        );

        if (Input.GetMouseButtonDown(0))
        {
            TryInteract();
        }

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (!Physics.Raycast(ray, out RaycastHit hit, interactRange, interactLayer))
        {
            interactHelper.SetActive(false);
        }
        else
        {
            interactHelper.SetActive(true);
        }

    }


    void TryInteract()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (!Physics.Raycast(ray, out RaycastHit hit, interactRange, interactLayer))
        {

            // If holding item and looking at nothing → drop
            if (itemHolder.heldItem != null)
            {
                itemHolder.DropItem();
            }
            return;
        }


        // 1️⃣ Pick up item
        HoldableItem item = hit.collider.GetComponent<HoldableItem>();
        if (item != null && itemHolder.heldItem == null)
        {
            itemHolder.PickUpItem(item);
            return;
        }

        // 2️⃣ Interact with container
        ContainerSlot container = hit.collider.GetComponent<ContainerSlot>();
        if (container != null)
        {
            // Place item into MAIN container
            if (itemHolder.heldItem != null && container.isMainContainer)
            {
                bool stored = container.TryStoreItem(itemHolder.heldItem);
                if (stored)
                {
                    itemHolder.ClearHeldItem();
                }
                return;
            }



            //// Take item from container
            //if (itemHolder.heldItem == null && container.HasItem())
            //{
            //    HoldableItem takenItem = container.RemoveItem();
            //    itemHolder.PickUpItem(takenItem);
            //    return;
            //}
        }

        //Interact with computer
        var computer = hit.collider.GetComponent<ComputerTerminal>();
        if(computer != null && computer.isOpen == false)
        {
            computer.Open();
            return;
        }
        else if(computer != null && computer.isOpen == true)
        {
            computer.Close();
            return;
        }

        //Interact with blueprints
        var blueprint = hit.collider.GetComponent<BluePrintInteractor>();
        if(blueprint != null && blueprint.isOpen == false)
        {
            blueprint.Open();
            return;
        }
        else if (blueprint != null && blueprint.isOpen == true)
        {
            blueprint.Close();
            return;
        }
    }
}
