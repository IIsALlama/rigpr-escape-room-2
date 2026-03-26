using UnityEngine;

public class StaticScam : MonoBehaviour
{
    public GameObject cctvCanvas;

    public bool isStatic;




    // Update is called once per frame
    void Update()
    {

        if (isStatic)
        {
            cctvCanvas.SetActive(true);
        }
         else
        {
            cctvCanvas.SetActive(false);
        }
    }
}
