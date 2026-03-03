using UnityEngine;

public class CanDestoyer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Can dead");
        Destroy(other.gameObject);
    }
}
