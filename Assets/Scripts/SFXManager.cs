using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour, IAudioManager
{
    public static SFXManager _instance;
    private bool isMuted = false;

    [Header("Components")]
    [SerializeField] private AudioSource _audioSource;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
        //SetVolumeLevel(0.5f);
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
