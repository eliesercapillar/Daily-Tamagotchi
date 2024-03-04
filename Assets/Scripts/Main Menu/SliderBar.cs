using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderBar : MonoBehaviour
{
    private IAudioManager _audioManager;
    private Slider _slider;
    [SerializeField] private bool _isBGMSlider;

    public void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    private void OnEnable()
    {
        _slider.onValueChanged.AddListener( delegate { ChangeBGMVolume(); } );
    }

    private void Start()
    {
        if (_isBGMSlider) _audioManager = BGMManager._instance;
        else              _audioManager = SFXManager._instance; 
        Debug.Log("Setting slider value");
        _slider.value = _audioManager.GetVolumeLevel();
    }

    private void ChangeBGMVolume()
    {
        _audioManager.SetVolumeLevel(_slider.value);
    }
}
