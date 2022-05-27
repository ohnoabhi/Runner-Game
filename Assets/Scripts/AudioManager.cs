using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private List<AudioSource> audioSources = new List<AudioSource>();
    [SerializeField] private List<AudioSource> audio3DSources = new List<AudioSource>();
    private GameObject targetGameObject;

    private static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        targetGameObject = Instantiate(new GameObject(), transform);
        targetGameObject.name = "AudioSources";
    }


    public static bool SoundON
    {
        get => PlayerPrefs.GetInt("SoundON", 1) > 0;
        set => PlayerPrefs.SetInt("SoundON", value ? 1 : 0);
    }

    public static bool MusicON
    {
        get => PlayerPrefs.GetInt("MusicON", 1) > 0;
        set => PlayerPrefs.SetInt("MusicON", value ? 1 : 0);
    }

    [SerializeField] Sounds[] sounds;

    public static void Play(string name)
    {
        if (!SoundON) return;
        var s = Array.Find(instance.sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Audio " + name + " not found!");
            return;
        }

        var source = instance.GetAudioSource();
        source.clip = s.audioClip;
        source.Play();
    }

    public static void Play(string name, Vector3 position, float volume = 1)
    {
        Debug.Log(name + ": " + volume);
        if (!SoundON) return;
        var s = Array.Find(instance.sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Audio " + name + " not found!");
            return;
        }

        var source = instance.GetAudio3DSource();
        source.transform.position = position;
        source.clip = s.audioClip;
        source.volume = volume;
        source.Play();
    }

    public static void Play(AudioClip clip, float pitch = 1)
    {
        if (!SoundON) return;
        var source = instance.GetAudioSource();
        source.pitch = pitch;
        source.clip = clip;
        source.Play();
    }

    public static void Play(AudioSource source, AudioClip clip)
    {
        if (!SoundON) return;
        source.clip = clip;
        source.Play();
    }

    public static void Play(AudioSource source)
    {
        if (!SoundON) return;
        source.Play();
    }

    public static void OnButtonClick()
    {
        Play("OnButtonClick");
    }

    private AudioSource GetAudioSource()
    {
        int sourcesAmount = audioSources.Count;
        for (int i = 0; i < sourcesAmount; i++)
        {
            if (!audioSources[i].isPlaying)
            {
                return audioSources[i];
            }
        }

        var createdSource = CreateAudioSourceObject();
        audioSources.Add(createdSource);

        return createdSource;
    }

    private AudioSource GetAudio3DSource()
    {
        int sourcesAmount = audio3DSources.Count;
        for (int i = 0; i < sourcesAmount; i++)
        {
            if (!audio3DSources[i].isPlaying)
            {
                return audio3DSources[i];
            }
        }

        var createdSource = CreateAudioSourceObject();
        createdSource.spatialBlend = 1;
        audio3DSources.Add(createdSource);

        return createdSource;
    }

    private AudioSource CreateAudioSourceObject()
    {
        var audioSource = targetGameObject.AddComponent<AudioSource>();
        SetSourceDefaultSettings(audioSource);

        return audioSource;
    }

    public static void SetSourceDefaultSettings(AudioSource source, AudioType type = AudioType.Sound)
    {
        float volume = 1;

        if (type == AudioType.Sound)
        {
            source.loop = false;
        }
        else if (type == AudioType.Music)
        {
            source.loop = true;
        }

        source.clip = null;

        source.volume = volume;
        source.pitch = 1.0f;
        source.spatialBlend = 0; // 2D Sound
        source.mute = false;
        source.playOnAwake = false;
        source.outputAudioMixerGroup = null;
    }
}

public enum AudioType
{
    Music = 0,
    Sound = 1
}

[System.Serializable]
class Sounds
{
    public string name;

    public AudioClip audioClip;
}