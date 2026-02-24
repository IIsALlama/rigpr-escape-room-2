using UnityEngine;

[RequireComponent(typeof(Collider))]
public class RatAI : MonoBehaviour
{
    public enum AfterCheeseBehaviour
    {
        ReturnToHoleAndDespawn,
        ContinueToDoorAndDespawn,
        StopAndWait
    }

    [Header("Puzzle Toggle")]
    [SerializeField] private bool puzzleEnabled = false;

    [Header("Targets")]
    [SerializeField] private Transform holeTarget;
    [SerializeField] private Transform pausePoint;
    [SerializeField] private Transform doorTarget;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2.5f;
    [SerializeField] private bool lockY = true;

    [Header("Facing")]
    [Tooltip("THIS rotates, root never rotates. Must have localPosition (0,0,0).")]
    [SerializeField] private Transform rotPivot;
    [Tooltip("Yaw offset for the pivot so the mesh faces forward. Try 0/90/-90/180.")]
    [SerializeField] private float pivotYawOffset = 0f;
    [SerializeField] private float turnSpeed = 12f;

    [Header("Pause")]
    [SerializeField] private bool doPause = true;
    [SerializeField] private float pauseDuration = 1.0f;

    [Header("Cheese Interaction")]
    [SerializeField] private ItemType requiredItemType = ItemType.Cheese;
    [SerializeField] private Transform carryAnchor;
    [SerializeField]
    private AfterCheeseBehaviour afterCheeseBehaviour =
        AfterCheeseBehaviour.ReturnToHoleAndDespawn;
    [SerializeField] private bool despawnCheeseWithRat = true;

    [Header("Optional Rules")]
    [SerializeField] private bool onlyAcceptCheeseWhilePaused = false;

    private enum State { ToPause, Paused, ToDoor, ToHole, ToDoorAfterCheese, Stopped }
    private State state;

    private bool hasCheese;
    private HoldableItem carriedItem;
    private float pauseTimer;

    private void OnEnable()
    {
        // ensure trigger
        Collider c = GetComponent<Collider>();
        if (c != null && !c.isTrigger) c.isTrigger = true;
        Debug.Log($"[RatAI] pausePoint = {(pausePoint ? pausePoint.name : "NULL")} @ {pausePoint?.position}", this);

        ResetRat();
    }

    private void ResetRat()
    {
        hasCheese = false;
        carriedItem = null;
        pauseTimer = 0f;

        if (doPause && pausePoint != null)
            state = State.ToPause;
        else
            state = State.ToDoor;
    }

    private void Update()
    {
        if (!puzzleEnabled) return;
        if (holeTarget == null || doorTarget == null) return;

        switch (state)
        {
            case State.ToPause:
                if (pausePoint == null)
                {
                    state = State.ToDoor;
                    break;
                }

                if (MoveAndFaceExact(pausePoint.position))
                {
                    state = State.Paused;
                    pauseTimer = pauseDuration;
                    Debug.Log($"[RatAI] PAUSED. RatPos={transform.position} PausePos={pausePoint.position} Delta={(pausePoint.position - transform.position)}");
                    Debug.Log("[RatAI] Reached PAUSE point exactly -> Paused");

                }
                break;

            case State.Paused:
                pauseTimer -= Time.deltaTime;
                FaceOnly(doorTarget.position);

                if (pauseTimer <= 0f)
                    state = State.ToDoor;
                break;

            case State.ToDoor:
                if (MoveAndFaceExact(doorTarget.position))
                {
                    Debug.Log("[RatAI] Reached DOOR point exactly -> Despawn", this);
                    DespawnRatAndMaybeCheese();
                }
                break;

            case State.ToHole:
                if (MoveAndFaceExact(holeTarget.position))
                {
                    Debug.Log("[RatAI] Reached HOLE point exactly -> Despawn", this);
                    DespawnRatAndMaybeCheese();
                }
                break;

            case State.ToDoorAfterCheese:
                if (MoveAndFaceExact(doorTarget.position))
                {
                    Debug.Log("[RatAI] Reached DOOR (after cheese) exactly -> Despawn", this);
                    DespawnRatAndMaybeCheese();
                }
                break;

            case State.Stopped:
                break;
        }
    }

    private bool MoveAndFaceExact(Vector3 worldTarget)
    {
        Vector3 pos = transform.position;
        Vector3 target = worldTarget;

        if (lockY)
            target.y = pos.y;

        Vector3 delta = target - pos;
        float dist = delta.magnitude;

        if (dist <= 0.000001f)
            return true;

        Vector3 dir = delta / dist;
        float moveDist = moveSpeed * Time.deltaTime;

        if (moveDist >= dist)
        {
            transform.position = target; 
            FaceDirection(dir);
            return true;
        }

        transform.position = pos + dir * moveDist;
        FaceDirection(dir);
        return false;
    }

    private void FaceOnly(Vector3 worldTarget)
    {
        Vector3 pos = transform.position;
        Vector3 target = worldTarget;

        if (lockY)
            target.y = pos.y;

        Vector3 delta = target - pos;
        if (delta.sqrMagnitude < 0.00001f) return;

        FaceDirection(delta.normalized);
    }

    private void FaceDirection(Vector3 dir)
    {
        if (rotPivot == null) return;

        Vector3 flatDir = dir;
        flatDir.y = 0f;
        if (flatDir.sqrMagnitude < 0.00001f) return;

        Quaternion worldLook = Quaternion.LookRotation(flatDir, Vector3.up);
        Quaternion desiredWorld = worldLook * Quaternion.Euler(0f, pivotYawOffset, 0f);

        rotPivot.rotation = Quaternion.Slerp(rotPivot.rotation, desiredWorld, turnSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[RatAI] OnTriggerEnter -> {other.name}", this);

        if (!puzzleEnabled)
        {
            Debug.Log("[RatAI] Ignored: puzzleEnabled = false", this);
            return;
        }

        if (hasCheese)
        {
            Debug.Log("[RatAI] Ignored: already hasCheese = true", this);
            return;
        }

        if (onlyAcceptCheeseWhilePaused && state != State.Paused)
        {
            Debug.Log($"[RatAI] Ignored: onlyAcceptCheeseWhilePaused = true but state = {state}", this);
            return;
        }

        HoldableItem item = other.GetComponentInParent<HoldableItem>();
        if (item == null)
        {
            Debug.Log("[RatAI] Ignored: No HoldableItem found on collider or its parents", this);
            return;
        }

        Debug.Log($"[RatAI] HoldableItem found: {item.name} | type={item.itemType} | isHeld={item.isHeld} | isStored={item.isStored}", this);

        if (item.itemType != requiredItemType)
        {
            Debug.Log($"[RatAI] Ignored: itemType {item.itemType} != required {requiredItemType}", this);
            return;
        }

        if (item.isHeld)
        {
            Debug.Log("[RatAI] Ignored: item is still held (isHeld = true)", this);
            return;
        }

        Debug.Log("[RatAI] CHEESE ACCEPTED -> attaching + changing behaviour", this);

        hasCheese = true;
        carriedItem = item;

        AttachItemToRat(item);

        switch (afterCheeseBehaviour)
        {
            case AfterCheeseBehaviour.ReturnToHoleAndDespawn:
                state = State.ToHole;
                Debug.Log("[RatAI] AfterCheeseBehaviour: ReturnToHoleAndDespawn -> state = ToHole", this);
                break;

            case AfterCheeseBehaviour.ContinueToDoorAndDespawn:
                state = State.ToDoorAfterCheese;
                Debug.Log("[RatAI] AfterCheeseBehaviour: ContinueToDoorAndDespawn -> state = ToDoorAfterCheese", this);
                break;

            case AfterCheeseBehaviour.StopAndWait:
                state = State.Stopped;
                Debug.Log("[RatAI] AfterCheeseBehaviour: StopAndWait -> state = Stopped", this);
                break;
        }
    }

    private void AttachItemToRat(HoldableItem item)
    {
        item.isHeld = false;
        item.isStored = false;
        item.storedInContainer = null;

        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        Collider col = item.GetComponent<Collider>();
        if (col != null) col.enabled = false;

        Transform anchor = carryAnchor != null ? carryAnchor : transform;
        item.transform.SetParent(anchor, false);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;
        item.gameObject.SetActive(true);
    }

    private void DespawnRatAndMaybeCheese()
    {
        if (despawnCheeseWithRat && carriedItem != null)
            carriedItem.DeleteItem();

        Destroy(gameObject);
    }

    public void SetPuzzleEnabled(bool enabled) => puzzleEnabled = enabled;
}