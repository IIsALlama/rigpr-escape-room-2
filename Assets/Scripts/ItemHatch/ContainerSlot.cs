using System.Collections;
using UnityEngine;

public class ContainerSlot : MonoBehaviour
{
    [Header("Container Settings")]
    public int containerID;
    public bool isMainContainer;

    public Transform ejectPoint;
    private bool ejectPending;
    private float ejectTimer;
    public float ejectdelay;
    private Coroutine ejectRoutine;

    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private int cameraIndexForThisHatch = -1;
    [SerializeField] private int cctvAppId = 0;

    [Header("Force")]
    public float force;
    public bool useForce;

    private RoutingApp routingApp;
    private HoldableItem storedItem;

    void Start()
    {
        ContainerManager.Instance.Register(this);
        routingApp = FindFirstObjectByType<RoutingApp>(FindObjectsInactive.Include);

        if (cameraManager == null)      
            cameraManager = FindFirstObjectByType<CameraManager>(FindObjectsInactive.Include);


    }


    void Update()
    {
        if (ejectPending && storedItem != null)
        {
            ejectTimer -= Time.deltaTime;
            if (ejectTimer <= 0f)
            {
                EjectItem();
                ejectPending = false;
            }
        }
    }

    public bool HasItem() => storedItem != null;

    public bool TryStoreItem(HoldableItem item)
    {
        if (!isMainContainer || storedItem != null)
            return false;

        storedItem = item;
        item.Store(this);
        return true;
    }


    public void ReceiveItem(HoldableItem item)
    {
        storedItem = item;
        item.Store(this);

        if (isMainContainer) return;

        if (routingApp != null && routingApp.manualDrop) return;

        if (ejectRoutine != null)
            StopCoroutine(ejectRoutine);

        ejectRoutine = StartCoroutine(AutoEjectAfterDelay());
    }



    public HoldableItem RemoveItem()
    {
        HoldableItem item = storedItem;
        storedItem = null;

        if (item != null)
            item.storedInContainer = null;

        return item;
    }

    public void EjectItem()
    {

        if (storedItem == null) return;
        Debug.Log("Ejecting Item from ContainerSlot ID: " + containerID);

        HoldableItem item = storedItem;
        storedItem = null;

        //Set item active and to ejectpoint pos
        item.gameObject.SetActive(true);
        item.transform.position = ejectPoint.transform.position;
        item.transform.rotation = ejectPoint.transform.rotation;

        //Remove item from being stored
        item.isStored = false;
        item.storedInContainer = null;

        //Turn item physics back on
        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        ////Add force to eject item
        rb = item.GetComponent<Rigidbody>();
        if (rb != null && useForce == true)
        {
            rb.AddForce(ejectPoint.transform.up * force, ForceMode.VelocityChange);
        }

        if (cameraIndexForThisHatch >= 0 && AppManager.instance != null && cameraManager != null)
        {
            AppManager.instance.OpenApp(cctvAppId);
            cameraManager.SetCamera(cameraIndexForThisHatch);
        }
    }

    private IEnumerator AutoEjectAfterDelay()
    {
        yield return new WaitForSeconds(ejectdelay);

        if (storedItem == null)
        {
            ejectRoutine = null;
            yield break;
        }
        Debug.Log("Starting Auto Eject Coroutine");

        EjectItem();
        ejectRoutine = null;
    }
}