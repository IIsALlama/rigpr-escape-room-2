using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatHatch : MonoBehaviour
{
    [Header("Spawn")]
    [SerializeField] private HoldableItem itemPrefab;
    [SerializeField] private Transform ejectPoint;

    [Header("Cooldown")]
    [SerializeField] private float spawnCooldown = 1.0f;
    private bool canSpawn = true;

    [Header("Spawn Rotation")]
    [SerializeField] private Vector3 spawnEulerOffset;
    [SerializeField] private bool randomYaw = true;

    [Header("Optional force")]
    [SerializeField] private bool useForce = true;

    [Header("Random Force")]
    [SerializeField] private Vector2 forceRange = new Vector2(2f, 5f);
    [SerializeField] private float sidewaysSpread = 0.5f;

    [Header("Optional limit (prevents item spam)")]
    [SerializeField] private int maxAlive = 3;
    private readonly List<HoldableItem> alive = new List<HoldableItem>();

    [Header("Animator")]
    [SerializeField] private Animator anim;

    private bool pendingDispense;

    public void Dispense()
    {
        CleanupList();

        if (!canSpawn) return;
        if (itemPrefab == null || ejectPoint == null) return;
        if (maxAlive > 0 && alive.Count >= maxAlive) return;

        pendingDispense = true;
        anim.SetTrigger("Dispense");
    }

    public void SpawnNow()
    {
        if (!pendingDispense) return;
        if (!canSpawn) return;
        pendingDispense = false;

        StartCoroutine(DispenseRoutine());
    }

    private IEnumerator DispenseRoutine()
    {
        canSpawn = false;

        Quaternion rot = ejectPoint.rotation * Quaternion.Euler(spawnEulerOffset);
        if (randomYaw)
            rot *= Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

        var item = Instantiate(itemPrefab, ejectPoint.position, rot);
        alive.Add(item);

        item.isHeld = false;
        item.isStored = false;
        item.storedInContainer = null;

        item.gameObject.SetActive(true);
        foreach (var t in item.GetComponentsInChildren<Transform>(true))
            t.gameObject.SetActive(true);
        foreach (var r in item.GetComponentsInChildren<Renderer>(true))
            r.enabled = true;

        var rb = item.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            if (useForce)
            {
                float f = Random.Range(forceRange.x, forceRange.y);

                Vector3 dir = ejectPoint.up;

                Vector2 s = Random.insideUnitCircle * sidewaysSpread;
                dir = (dir + ejectPoint.right * s.x + ejectPoint.forward * s.y).normalized;

                rb.AddForce(dir * f, ForceMode.VelocityChange);
            }
        }

        yield return new WaitForSeconds(spawnCooldown);
        canSpawn = true;
    }

    private void CleanupList()
    {
        for (int i = alive.Count - 1; i >= 0; i--)
            if (alive[i] == null) alive.RemoveAt(i);
    }
}