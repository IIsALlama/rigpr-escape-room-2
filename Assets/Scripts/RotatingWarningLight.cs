using UnityEngine;

public class RotatingWarningLight : MonoBehaviour
{
    public float rotationSpeed = 120f;
    public bool isActive = false;

    void Update()
    {
        if (!isActive)
        {
            gameObject.SetActive(false);
        }
        if (isActive)
        {
            gameObject.SetActive(true);
        }

        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    public void Activate()
    {
        isActive = true;
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        isActive = false;
        gameObject.SetActive(false);
    }
}
