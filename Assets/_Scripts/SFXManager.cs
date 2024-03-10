using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour, IAudioManager
{
    public static SFXManager _instance;
    private bool isMuted = false;

    [Header("Components")]
    [SerializeField] private AudioSource _audioSource;

    [Header("Clips")]
    [SerializeField] private List<AudioClip> _ambientOfficeClips;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Update()
    {
        if (_audioSource.isPlaying) return;
        PlayRandomAmbience();
    }

    public void PlayRandomAmbience()
    {
        _audioSource.clip = _ambientOfficeClips.RandomElement();;
        _audioSource.Play();
    }

    public void SetAudioAndLoop(AudioClip clip, bool turnOn)
    {
        _audioSource.clip = clip;
        _audioSource.loop = turnOn;
        _audioSource.Play();
    }

    public float GetVolumeLevel()
    {
        return _audioSource.volume;
    }

    public void SetVolumeLevel(float volume)
    {
        _audioSource.volume = volume;
    }

    public void FlipMute()
    {
        isMuted = !isMuted;
        _audioSource.mute = isMuted;
    }

    public void PlayAudio(AudioClip clip)
    {
        Debug.Log($"Playing clip {clip}");
        _audioSource.PlayOneShot(clip);
    }
}
