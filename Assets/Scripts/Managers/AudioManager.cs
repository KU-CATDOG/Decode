using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioType
{
    player_attack,
    player_hit,
    player_jump_down,
    player_jump_up,
    player_roll,
    player_slide,
    player_walk,
    stage1,
    hpboss,
    mpboss,
    speedboss,
    Length
}

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioSource loopAudioSource;
    [SerializeField] private AudioSource bgmAudioSource;
    [SerializeField] private AudioSource[] audioSources;
    private int sourceIndex;

    Dictionary<AudioType, AudioClip> audioDictionary = new Dictionary<AudioType, AudioClip>();
    private bool bgmPlaying = false;
    private bool loopPlaying = false;

    private void Awake()
    {
        if (AudioManager.Instance != this)
            Destroy(this.gameObject);

        for (int i = 0; i < (int)AudioType.Length; i++)
        {
            audioDictionary.Add((AudioType)i, Resources.Load<AudioClip>($"Audio/{((AudioType)i).ToString()}"));
        }

        transform.parent = null;
        DontDestroyOnLoad(this.gameObject);
    }

    public void PlayAudio(AudioType type)
    {
        AudioSource source = audioSources[sourceIndex];
        source.loop = false;
        source.clip = audioDictionary[type];
        source.Play();

        sourceIndex++;
        sourceIndex %= audioSources.Length;
    }

    public void PlayLoop(AudioType type)
    {
        if (loopPlaying) return;
        loopPlaying = true;
        Debug.Log(type);
        AudioSource source = loopAudioSource;
        source.loop = true;
        source.clip = audioDictionary[type];
        source.Play();
    }

    public void StopLoop()
    {
        AudioSource source = loopAudioSource;
        source.loop = false;
        source.Stop();
        loopPlaying = false;
    }

    public void PlayBGM(AudioType type)
    {
        if (bgmPlaying) return;
        bgmPlaying = true;
        Debug.Log(type);
        AudioSource source = loopAudioSource;
        source.loop = true;
        source.clip = audioDictionary[type];
        source.Play();
    }
}
