using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("BGM Settings")]
    public AudioClip menuMusic;
    public AudioClip gameMusic;
    [Range(0f, 1f)] public float musicVolume = 0.5f;

    [Header("Player Audio Settings")]
    public AudioClip[] footstepSounds;
    [Range(0f, 1f)] public float footstepVolume = 0.7f;
    public AudioClip jumpSound;
    [Range(0f, 1f)] public float jumpVolume = 0.8f;

    private AudioSource _currentBGM;

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

        // ferma la musica precedente se presente
        if (_currentBGM != null)
        {
            _currentBGM.Stop();
            Destroy(_currentBGM.gameObject);
        }

        // crea nuovo oggetto per la BGM
        GameObject bgmObj = new GameObject("BGM_" + clip.name);
        bgmObj.transform.SetParent(transform);
        bgmObj.transform.localPosition = Vector3.zero;

        _currentBGM = bgmObj.AddComponent<AudioSource>();
        _currentBGM.clip = clip;
        _currentBGM.loop = true;
        _currentBGM.volume = musicVolume;
        _currentBGM.Play();
    }

    public void PlayMenuMusic() // <- riproduce musica del menu
    {
        PlayBGM(menuMusic);
    }

    public void PlayGameMusic() // <- riproduce musica del gioco
    {
        PlayBGM(gameMusic);
    }

    public void PlayFootstep(Vector2 position) // <- riproduce suono dei passi del player (randomico)
    {
        if (footstepSounds == null || footstepSounds.Length == 0) return;

        // seleziona un suono casuale dall'array
        AudioClip randomFootstep = footstepSounds[Random.Range(0, footstepSounds.Length)];
        PlaySoundEffect(randomFootstep, position, footstepVolume);
    }

    public void PlayJump(Vector2 position) // <- riproduce suono del salto del player
    {
        PlaySoundEffect(jumpSound, position, jumpVolume);
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

    public void StopBGM() // <- ferma la musica di sottofondo
    {
        if (_currentBGM != null)
        {
            _currentBGM.Stop();
            Destroy(_currentBGM.gameObject);
            _currentBGM = null;
        }
    }

    public void SetMusicVolume(float volume) // <- cambia il volume della musica
    {
        musicVolume = Mathf.Clamp01(volume);

        if (_currentBGM != null) _currentBGM.volume = musicVolume;
    }
}