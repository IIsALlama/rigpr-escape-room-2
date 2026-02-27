using UnityEngine;
using System.Collections;


public class RatSpawner : MonoBehaviour
{
    [Header("Puzzle Toggle")]
    [SerializeField] private bool puzzleEnabled = false;

    [Header("Rat Spawn Settings")]
    [SerializeField] private GameObject ratPrefab;
    [SerializeField] private int ratCount = 10;
    [SerializeField] private Transform ratSpawnPoint;
    [SerializeField] private Vector2 spawnDelayRange = new Vector2(3f, 5f);
    [SerializeField] private RatWorldRefs worldRefs;
    private Coroutine spawnRoutine;

    private void Update()
    {
        if (puzzleEnabled && spawnRoutine == null)
        {

            StartSpawning();
        }

    }


    public void StartSpawning()
    {
        if (spawnRoutine != null)
            StopCoroutine(spawnRoutine);
        spawnRoutine = StartCoroutine(SpawnRats());
    }

    public void StopSpawning()
    {
        if (spawnRoutine != null)
            StopCoroutine(spawnRoutine);
        spawnRoutine = null;
    }

    private IEnumerator SpawnRats()
    {
        Vector3 pos = ratSpawnPoint ? ratSpawnPoint.position : transform.position;
        Quaternion rot = ratSpawnPoint ? ratSpawnPoint.rotation : transform.rotation;

        for (int i = 0; i < ratCount; i++)
        {
            var ratGo = Instantiate(ratPrefab, ratSpawnPoint.position, ratSpawnPoint.rotation);
            var ai = ratGo.GetComponent<RatAI>();
            ai.Init(worldRefs);

            Instantiate(ratPrefab, pos, rot);

            if (i < ratCount - 1)
                yield return new WaitForSeconds(Random.Range(spawnDelayRange.x, spawnDelayRange.y));
        }
    }

    public void SetPuzzleEnabled(bool enabled) => puzzleEnabled = enabled;

}
