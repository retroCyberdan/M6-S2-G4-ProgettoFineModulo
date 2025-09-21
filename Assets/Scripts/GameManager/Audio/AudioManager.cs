using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Background Music Settings")]
    public AudioClip menuMusic;
    public AudioClip gameMusic;
    [Range(0f, 1f)] public float musicVolume = 0.5f;

    [Header("Player Audio Settings")]
    public AudioClip footstepSound;
    [Range(0f, 1f)] public float footstepVolume = 0.7f;

    private AudioSource currentBGM;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void PlayBGM(AudioClip clip) // <- riproduce musica di sottofondo (menu o gioco)
    {
        if (clip == null) return;

        if (currentBGM != null)
        {
            currentBGM.Stop();
            Destroy(currentBGM.gameObject);
        }

        GameObject bgmObj = new GameObject("BGM_" + clip.name);
        bgmObj.transform.SetParent(transform);
        bgmObj.transform.localPosition = Vector3.zero;

        currentBGM = bgmObj.AddComponent<AudioSource>();
        currentBGM.clip = clip;
        currentBGM.loop = true;
        currentBGM.volume = musicVolume;
        currentBGM.Play();
    }

    public void PlayMenuMusic() // <- riproduce musica del menu
    {
        PlayBGM(menuMusic);
    }

    public void PlayGameMusic() // <- riproduce musica del gioco
    {
        PlayBGM(gameMusic);
    }

    public void PlayFootstep(Vector2 position) // <- riproduce suono dei passi del player
    {
        PlaySoundEffect(footstepSound, position, footstepVolume);
    }

    public void PlaySoundEffect(AudioClip clip, Vector2 position, float volume) // <- funzione generica per riprodurre effetti sonori
    {
        if (clip == null) return;

        GameObject audioObject = new GameObject("Sound_" + clip.name);
        audioObject.transform.position = position;

        AudioSource audioSource = audioObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();

        Destroy(audioObject, clip.length);
    }

    public void StopBGM()
    {
        if (currentBGM != null)
        {
            currentBGM.Stop();
            Destroy(currentBGM.gameObject);
            currentBGM = null;
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        if (currentBGM != null)
        {
            currentBGM.volume = musicVolume;
        }
    }
}