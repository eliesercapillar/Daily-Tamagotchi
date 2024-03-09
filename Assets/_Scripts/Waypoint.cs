using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NPC;
using DG.Tweening;

public class Waypoint : MonoBehaviour
{
    [SerializeField] private bool _shouldInteract = false;
    [SerializeField] private NPCState _npcBehaviour;
    [SerializeField] private string _description;

    [Header("Progress Bar")]
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Slider _progressBar;
    [SerializeField] private Image _progressBarBG;
    [SerializeField] private Image _progressBarFill;
    [SerializeField] private float _progressIncrementAmount;
    [SerializeField] private float _progressIncrementCD;
    private WaitForSeconds _cooldown;
    private bool _onCD;

    private Player.Player _player;
    private bool _playerInCollider;

    public bool ShouldInteract    { get { return _shouldInteract; } }
    public NPCState NPCBehaviour  { get { return _npcBehaviour; } }
    public string Description  { get { return _description; } }

    private void Awake()
    {
        _canvas = transform.Find("Waypoint Progress Bar").GetComponent<Canvas>();
        _progressBar = _canvas.transform.Find("Slider").GetComponent<Slider>();
        _progressBarBG = _progressBar.transform.Find("Background").GetComponent<Image>();
        _progressBarFill = _progressBar.transform.Find("Fill Area").GetComponentInChildren<Image>();
    }

    private void Start()
    {
        _canvas.worldCamera = Camera.main;
        _cooldown = new WaitForSeconds(_progressIncrementCD);
        _onCD = false;
        _playerInCollider = false;
        _player = Player.Player._instance;
    }

    private void Update()
    {
        if (!_playerInCollider) return;

        if (Input.GetKey(KeyCode.Space))
        {
            if (_player.CurrentWaypoint != this) return;
            if (!_canvas.enabled) TurnOnOffProgressBar(true);
            if (_onCD) return;
            StartCoroutine(IncrementProgressBar());
            if (_progressBar.value >= 1f) 
            {
                TurnOnOffProgressBar(false);
                _player.InteractAtWaypoint();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "TAG_Player")
        {
            if (_player.CurrentWaypoint != this) return;
            _playerInCollider = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "TAG_Player")
        {
            if (_player.CurrentWaypoint != this) return;
            _playerInCollider = false;
            if (_progressBar.value < 1f) 
            {
                TurnOnOffProgressBar(false);
            }
        }
    }

    private void TurnOnOffProgressBar(bool turnOn)
    {
        if (turnOn) _canvas.enabled = turnOn;
        StartCoroutine(FadeProgressBar(turnOn));

        IEnumerator FadeProgressBar(bool fadeIn)
        {
            if (fadeIn)
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
                _canvas.enabled = fadeIn;
            }
        }
    }

    private IEnumerator IncrementProgressBar()
    {
        _progressBar.value = Mathf.Clamp(_progressBar.value + _progressIncrementAmount, 0f, 1f);
        Debug.Log($"Value is: {_progressBar.value}");
        _onCD = true;
        yield return _cooldown;
        _onCD = false;
    }
}
