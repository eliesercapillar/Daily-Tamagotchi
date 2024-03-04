using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private AudioSource _audioSource;



    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        ChangeAudioVolume(0.5f);
    }

    private void Update()
    {
        
    }

    public void ChangeAudioVolume(float volume)
    {
        _audioSource.volume = volume;
    }
}
