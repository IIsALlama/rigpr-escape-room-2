using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public List<Sound> sounds = new List<Sound>();

    void Awake()
    {
        if (instance == null) instance = this;
        else { Destroy(gameObject); return; }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            GameObject soundObj = new GameObject("Sound_" + s.name);
            soundObj.transform.parent = transform;

            s.source = soundObj.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.spatialBlend = s.is3D ? 1f : 0f; // 3D or 2D sound
        }
    }

    public void Play(string name, Vector3? pos = null)
    {
        Sound s = sounds.Find(sound => sound.name == name);
        if (s == null) { Debug.LogWarning("Sound not found: " + name); return; }

        if (s.is3D && pos.HasValue)
        {
            s.source.transform.position = pos.Value;
        }

        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = sounds.Find(sound => sound.name == name);
        if (s != null && s.source.isPlaying)
            s.source.Stop();
    }
}



// HOW TO USE

//Create audio manager gameobject and attach this script to it
//in inspector set up sound list
//name them assign the clips can set if they are 3d or 2d sounds
//then in the script where you want the sound to play do this for example to play a clip called doorCreak
//AudioManager.instance.Play("doorCreak", transform.position);
//only need to add transform.position if its a 3d

