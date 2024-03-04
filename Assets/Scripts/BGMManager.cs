using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour, IAudioManager
{
    public static BGMManager _instance;
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
}
