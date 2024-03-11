using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NPC;
using DG.Tweening;

public class Waypoint : MonoBehaviour
{
    private SFXManager _sfxManager;

    [Header("NPC Settings")]
    [SerializeField] private bool _shouldInteract = false;
    [SerializeField] private NPCState _npcBehaviour;

    [Header("Waypoint Properties")]
    [SerializeField] private string _description;
    [SerializeField] private AudioSource _waypointAudioSource;
    [SerializeField] private AudioClip _successChime;

    [Header("Progress Bar")]
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Slider _progressBar;
    [SerializeField] private Image _progressBarBG;
    [SerializeField] private Image _progressBarFill;
    private float _progressIncrementAmount = 0.02f;
    private float _progressCD = 0.2f;
    private float _progressDecrementAmount = 0.04f;
    private WaitForSeconds _cooldown;
    private bool _onIncrementCD;
    private bool _onDecrementCD;

    private Player.Player _player;
    private bool _playerInCollider;
    private Image _indicatorImg;

    public bool ShouldInteract    { get { return _shouldInteract; } }
    public NPCState NPCBehaviour  { get { return _npcBehaviour; } }
    public string Description  { get { return _description; } }

    private void Awake()
    {
        _canvas = transform.Find("Waypoint Progress Bar").GetComponent<Canvas>();
        _progressBar = _canvas.transform.Find("Slider").GetComponent<Slider>();
        _progressBarBG = _progressBar.transform.Find("Background").GetComponent<Image>();
        _progressBarFill = _progressBar.transform.Find("Fill Area").GetComponentInChildren<Image>();
        _waypointAudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _canvas.worldCamera = Camera.main;
        _cooldown = new WaitForSeconds(_progressCD);
        _onIncrementCD = false;
        _onDecrementCD = false;
        _playerInCollider = false;
        _player = Player.Player._instance;
        _indicatorImg = WaypointIndicator._instance. GetComponent<Image>();
        _sfxManager = SFXManager._instance;
    }

    private void Update()
    {
        if (!_playerInCollider) return;
        
        if (Input.GetKey(KeyCode.Space))
        {
            if (_player.CurrentWaypoint != this) return;
            if (_onIncrementCD) return;
            StartCoroutine(IncrementProgressBar());
            if (!_waypointAudioSource.isPlaying) _waypointAudioSource.Play();
            if (_progressBar.value >= 1f) 
            {
                if (_waypointAudioSource.isPlaying) _waypointAudioSource.Stop();
                _waypointAudioSource.PlayOneShot(_successChime);
                _indicatorImg.DOFade(1, 0.5f);
                TurnOnOffProgressBar(false);
                _player.InteractAtWaypoint();
            }
        }
        else if (!_onDecrementCD) 
        {   
            if (_waypointAudioSource.isPlaying) _waypointAudioSource.Pause();
            StartCoroutine(DecrementProgressBar());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "TAG_Player")
        {
            if (_player.CurrentWaypoint != this) return;
            _playerInCollider = true;
            _indicatorImg.DOFade(0, 0.5f);
            TurnOnOffProgressBar(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "TAG_Player")
        {
            if (_player.CurrentWaypoint != this) return;
            if (_waypointAudioSource.isPlaying) _waypointAudioSource.Pause();
            _playerInCollider = false;
            _indicatorImg.DOFade(1, 0.5f);
            if (_progressBar.value < 1f) 
            {
                TurnOnOffProgressBar(false);
            }
        }
    }

    private void TurnOnOffProgressBar(bool turnOn)
    {
        StartCoroutine(FadeProgressBar());

        IEnumerator FadeProgressBar()
        {
            if (turnOn)
            {
                _progressBarBG.DOFade(1, 0.5f);
                _progressBarFill.DOFade(1, 0.5f);
                yield return null;
            }
            else    // Fade out
            {
                _progressBarBG.DOFade(0, 0.5f);
                _progressBarFill.DOFade(0, 0.5f);
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    private IEnumerator IncrementProgressBar()
    {
        _progressBar.value = Mathf.Clamp(_progressBar.value + _progressIncrementAmount, 0f, 1f);
        _onIncrementCD = true;
        yield return _cooldown;
        _onIncrementCD = false;
    }

    private IEnumerator DecrementProgressBar()
    {
        _progressBar.value = Mathf.Clamp(_progressBar.value - _progressDecrementAmount, 0f, 1f);
        _onDecrementCD = true;
        yield return _cooldown;
        _onDecrementCD = false;
    }
}
