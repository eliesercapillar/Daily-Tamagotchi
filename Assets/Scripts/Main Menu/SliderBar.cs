using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderBar : MonoBehaviour
{
    private IAudioManager _audioManager;
    private Slider _slider;
    [SerializeField] private bool _isBGMSlider;
    [SerializeField] private AudioClip _sliderAudio;
    private bool _isOnCD;
    private WaitForSeconds _cooldown;

    public void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    private void OnEnable()
    {
        _slider.onValueChanged.AddListener( delegate { ChangeBGMVolume(); } );
        _slider.onValueChanged.AddListener( delegate { PlaySFX(); } );
    }

    private void OnDisable()
    {
        _slider.onValueChanged.RemoveAllListeners();
    }

    private void Start()
    {
        if (_isBGMSlider) _audioManager = BGMManager._instance;
        else              _audioManager = SFXManager._instance; 
        Debug.Log("Setting slider value");
        //_slider.value = _audioManager.GetVolumeLevel();
        _isOnCD = false;
        _cooldown = new WaitForSeconds(0.05f);
    }

    private void ChangeBGMVolume()
    {
        _audioManager.SetVolumeLevel(_slider.value);
    }

    private void PlaySFX()
    {
        if (!_isOnCD) StartCoroutine(PutOnCD());

        IEnumerator PutOnCD()
        {
            _isOnCD = true;
            SFXManager._instance.PlayAudio(_sliderAudio);
            yield return _cooldown;
            _isOnCD = false;
        }
    }
}
