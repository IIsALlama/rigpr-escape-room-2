using UnityEngine;

public class LightHum : MonoBehaviour
{


    private void Start()
    {
        AudioManager.instance.Play("LightHum", this.transform.position);
    }
}
