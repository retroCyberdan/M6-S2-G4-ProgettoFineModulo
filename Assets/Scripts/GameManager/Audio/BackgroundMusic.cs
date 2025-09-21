using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public AudioClip musicClip;
    [Range(0f, 1f)] public float volume = 0.5f;
    public bool persistOnLoad = true;

    private static AudioSource currentMusic;

    private void Start()
    {
        if (currentMusic != null)
        {
            Destroy(gameObject); // <- evita duplicati
            return;
        }

        currentMusic = AudioController.PlayBGM(musicClip, transform, volume); // <- riproduce la clip audio

        if (persistOnLoad)
        {
            DontDestroyOnLoad(gameObject); // <- non si distrugge quando cambia scena
        }
    }
}
