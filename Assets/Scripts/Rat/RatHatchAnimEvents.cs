using UnityEngine;

public class RatHatchAnimEvents : MonoBehaviour
{
    [SerializeField] private RatHatch hatch;

    public void SpawnNow()
    {
        if (hatch != null)
            hatch.SpawnNow();
    }
}