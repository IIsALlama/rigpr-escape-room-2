using UnityEngine;

public class StartCam : MonoBehaviour
{
    [Header("Orbit Target")]
    [Tooltip("If null, uses world origin (0,0,0)")]
    [SerializeField] private Transform target;

    [Header("Orbit Settings")]
    [SerializeField] private float radius = 25f;
    [SerializeField] private float height = 12f;

    [Tooltip("Degrees per second")]
    [SerializeField] private float degreesPerSecond = 20f;

    [Header("Look")]
    [SerializeField] private bool lookAtTarget = true;
    [SerializeField] private Vector3 lookOffset = Vector3.zero;

    private float _angleDeg;

    private void Update()
    {
        Vector3 center = (target != null) ? target.position : Vector3.zero;

        _angleDeg += degreesPerSecond * Time.deltaTime;
        if (_angleDeg > 360f) _angleDeg -= 360f;

        float rad = _angleDeg * Mathf.Deg2Rad;

        Vector3 pos = new Vector3(
            center.x + Mathf.Cos(rad) * radius,
            center.y + height,
            center.z + Mathf.Sin(rad) * radius
        );

        transform.position = pos;

        if (lookAtTarget)
            transform.LookAt(center + lookOffset);
    }
}