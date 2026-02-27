using System.Collections.Generic;
using UnityEngine;

public class ScareZone : MonoBehaviour
{
    [SerializeField] private ItemType scareItemType;
    [SerializeField] private bool debugLogs = true;

    private readonly HashSet<RatAI> ratsInside = new HashSet<RatAI>();
    private readonly HashSet<HoldableItem> scaryItemsInside = new HashSet<HoldableItem>();

    private void OnTriggerEnter(Collider other)
    {
        var rat = other.GetComponentInParent<RatAI>();
        if (rat != null)
        {
            ratsInside.Add(rat);
            if (debugLogs)
                Debug.Log($"[ScareZone] Rat ENTER: {rat.name}. RatsInside={ratsInside.Count} ScaryItemsInside={scaryItemsInside.Count}", this);
            return;
        }

        if (!other.TryGetComponent<HoldableItem>(out var item)) return;
        if (item.itemType != scareItemType) return;

        bool isNew = scaryItemsInside.Add(item);

        if (debugLogs)
            Debug.Log($"[ScareZone] Scary item ENTER: {item.name} (new={isNew}). ScaryItemsInside={scaryItemsInside.Count}", this);

        if (!isNew) return;

        if (debugLogs)
            Debug.Log($"[ScareZone] TRIGGER by {item.name}. Scaring {ratsInside.Count} rats currently inside.", this);

        foreach (var r in ratsInside)
        {
            if (r != null) r.Scare();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var rat = other.GetComponentInParent<RatAI>();
        if (rat != null)
        {
            ratsInside.Remove(rat);
            if (debugLogs)
                Debug.Log($"[ScareZone] Rat EXIT: {rat.name}. RatsInside={ratsInside.Count}", this);
            return;
        }

        if (!other.TryGetComponent<HoldableItem>(out var item)) return;
        if (item.itemType != scareItemType) return;

        bool removed = scaryItemsInside.Remove(item);

        if (debugLogs)
            Debug.Log($"[ScareZone] Scary item EXIT: {item.name} (removed={removed}). ScaryItemsInside={scaryItemsInside.Count}", this);
    }
}