using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class CCTVStaticJitteryUI : MonoBehaviour
{
    public float jumpInterval = 0.03f;
    [Range(0.15f, 1f)] public float uvWindow = 0.45f;

    [Range(0f, 1f)] public float minAlpha = 0.18f;
    [Range(0f, 1f)] public float maxAlpha = 0.35f;

    RawImage img;
    float t;

    public bool enabled = false;

    void Awake()
    {
        img = GetComponent<RawImage>();
        img.uvRect = new Rect(0, 0, uvWindow, uvWindow);
    }

    void Update()
    {




        t += Time.deltaTime;
        if (t < jumpInterval) return;
        t = 0f;

        // jump to a random part of the noise texture
        float u = Random.value;
        float v = Random.value;
        img.uvRect = new Rect(u, v, uvWindow, uvWindow);

        // slight flicker
        var c = img.color;
        c.a = Random.Range(minAlpha, maxAlpha);
        img.color = c;


    }
}
