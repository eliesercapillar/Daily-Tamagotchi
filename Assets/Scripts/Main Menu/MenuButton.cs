using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    private BGMManager _bgmManager;
    private SFXManager _sfxManager;
    private Button _button;
    [SerializeField] private AudioClip _sfxClip;
    [SerializeField] private bool _isSFXMuter;
    [SerializeField] private bool _playSFXonly;

    public void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener( delegate { PlaySFX(); } );
        if (!_playSFXonly)
        {
            if (_isSFXMuter) _button.onClick.AddListener( delegate { MuteSFX(); } );
            else             _button.onClick.AddListener( delegate { MuteBGM(); } );
        }
    }

    private void OnDisable()
    {
        _button.onClick.RemoveAllListeners();
    }

    private void Start()
    {
        _bgmManager = BGMManager._instance;
        _sfxManager = SFXManager._instance; 
    }

    private void PlaySFX()
    {
        _sfxManager.PlayAudio(_sfxClip);
    }

    private void MuteSFX()
    {
        _sfxManager.FlipMute();
    }
    
    private void MuteBGM()
    {
        _bgmManager.FlipMute();
    }
}
