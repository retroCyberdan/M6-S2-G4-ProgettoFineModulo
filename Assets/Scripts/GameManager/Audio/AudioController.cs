using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static void Play(AudioClip clip, Vector2 position, float volume)
    {
        if (clip == null) return; // <- se non viene assegnata una clip audio non riproduce nulla

        GameObject audioObject = new GameObject("Sound");
        audioObject.transform.position = position;

        // assegno la componente AudioSource, quindi riproduco la clip audio con un certo volume
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();

        Object.Destroy(audioObject, clip.length); // <- si distrugge quando finisce la clip audio
    }
    public static AudioSource PlayBGM(AudioClip clip, Transform parent, float volume)
    {
        if (clip == null) return null;

        GameObject bgmObj = new GameObject("BGM_" + clip.name);
        bgmObj.transform.SetParent(parent);
        bgmObj.transform.localPosition = Vector2.zero;

        AudioSource source = bgmObj.AddComponent<AudioSource>();
        source.clip = clip;
        source.loop = true;
        source.volume = volume;
        source.Play();

        return source;
    }
}
