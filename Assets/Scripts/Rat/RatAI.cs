using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class RatAI : MonoBehaviour
{
    [Header("Targets")]
    private Transform holeTarget;
    private Transform pauseTarget;
    private Transform doorTarget;

    [Header("Can Detection")]
    public GameObject canDropDetection;

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

    [Header("Shock")]
    [SerializeField] private GameObject shockCanvas;
    [SerializeField] private float hopHeight = 0.15f;
    [SerializeField] private float hopTime = 0.12f;
    private Coroutine hopRoutine;

    private enum State { ToPause, Paused, ToDoor, ToHole, ToDoorAfterCheese, Stopped }
    private State state;

    private bool hasCheese;
    private HoldableItem carriedItem;
    private float pauseTimer;

    public void Init(RatWorldRefs refs)
    {
        holeTarget = refs.holeTarget;
        pauseTarget = refs.pauseTarget;
        doorTarget = refs.doorTarget;
        canDropDetection = refs.canDropZone;

        ResetRat();

    }


    private void OnEnable()
    {
        shockCanvas.SetActive(false);

        // ensure trigger
        Collider c = GetComponent<Collider>();
        if (c != null && !c.isTrigger) c.isTrigger = true;
        Debug.Log($"[RatAI] pausePoint = {(pauseTarget ? pauseTarget.name : "NULL")} @ {pauseTarget?.position}", this);

    }

    private void ResetRat()
    {
        hasCheese = false;
        carriedItem = null;
        pauseTimer = 0f;

        if (doPause && pauseTarget != null)
            state = State.ToPause;
        else
            state = State.ToDoor;
    }

    private void Update()
    {
        if (holeTarget == null || doorTarget == null) return;

        switch (state)
        {
            case State.ToPause:
                if (pauseTarget == null)
                {
                    state = State.ToDoor;
                    break;
                }

                if (MoveAndFaceExact(pauseTarget.position))
                {
                    state = State.Paused;
                    pauseTimer = pauseDuration;
                    Debug.Log($"[RatAI] PAUSED. RatPos={transform.position} PausePos={pauseTarget.position} Delta={(pauseTarget.position - transform.position)}");
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
                    Destroy(gameObject);
                }
                break;

            case State.ToHole:
                if (MoveAndFaceExact(holeTarget.position))
                {
                    Debug.Log("[RatAI] Reached HOLE point exactly -> Despawn", this);
                    Destroy(gameObject);
                }
                break;

            case State.Stopped:
                break;
        }
    }

    public void Scare()
    {
        if (shockCanvas != null) shockCanvas.SetActive(true);

        if (hopRoutine != null) StopCoroutine(hopRoutine);
        hopRoutine = StartCoroutine(Hop());
        AudioManager.instance.Play("Rat"); 
        pauseTimer = 0f;
        moveSpeed = 2f;
        state = State.ToHole;
    }

    private IEnumerator Hop()
    {
        float t = 0f;
        Vector3 start = transform.position;

        while (t < hopTime)
        {
            t += Time.deltaTime;
            float k = t / hopTime;

            float y = Mathf.Sin(k * Mathf.PI) * hopHeight;
            transform.position = new Vector3(start.x, start.y + y, start.z);

            yield return null;
        }

        transform.position = start;
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





}