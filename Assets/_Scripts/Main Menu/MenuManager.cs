using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Canvas Elements")]
    [SerializeField] private RectTransform _optionsMenu;
    [SerializeField] private float _optionsTweenTime;
    [SerializeField] private GameObject _optionsPanel;
    [SerializeField] private GameObject _instructionsPanel;
    [SerializeField] private Image _blackOutPanel;
    [SerializeField] private float _fadeTime;

    [Header("Buttons")]
    [SerializeField] private List<Button> _menuButtons;
    [SerializeField] private string _playSceneName;

    [Header("Options UI Elements")]
    [SerializeField] private Image _bgmImage;
    [SerializeField] private Image _bgmButtonImage;
    [SerializeField] private Sprite _bgmButtonOffSprite;
    [SerializeField] private Sprite _bgmButtonOnSprite;
    [SerializeField] private Sprite _bgmUnmutedSprite;
    [SerializeField] private Sprite _bgmMutedSprite;

    [Header("Instructions UI Elements")]
    [SerializeField] private Image _sfxImage;
    [SerializeField] private Image _sfxButtonImage;
    [SerializeField] private Sprite _sfxButtonOffSprite;
    [SerializeField] private Sprite _sfxButtonOnSprite;
    [SerializeField] private Sprite _sfxUnmutedSprite;
    [SerializeField] private Sprite _sfxMutedSprite;
    private bool _muteBGM;
    private bool _muteSFX;

    // Start is called before the first frame update
    void Start()
    {
        _optionsMenu.gameObject.SetActive(false);
        _optionsMenu.localScale = new Vector3(0.01f, 0.01f, 1);

        _muteBGM = false;
        _muteSFX = false;
    }

    public void PlayGame()
    {
        StartCoroutine(TweenFade());

        IEnumerator TweenFade()
        {
            _blackOutPanel.gameObject.SetActive(true);
            _blackOutPanel.DOFade(1, _fadeTime);
            yield return new WaitForSeconds(_fadeTime);
            SceneManager.LoadScene(_playSceneName);
        }
    }

    public void ShowOptionsMenu()
    {
        Debug.Log("Showing Options Menu");
        foreach (Button button in _menuButtons)
        {
            button.enabled = false;
        }
        StartCoroutine(TweenOpenPanel());
        _optionsPanel.SetActive(true);
    }

    public void CloseOptionsMenu()
    {
        Debug.Log("Closing Options Menu");
        StartCoroutine(TweenClosePanel());
        _optionsPanel.SetActive(false);
        foreach (Button button in _menuButtons)
        {
            button.enabled = true;
        }
    }

    public void ShowInstructionsMenu()
    {
        Debug.Log("Showing Instructions");
        foreach (Button button in _menuButtons)
        {
            button.enabled = false;
        }
        StartCoroutine(TweenOpenPanel());
        _instructionsPanel.SetActive(true);
    }

    public void CloseInstructionsMenu()
    {
        Debug.Log("Closing Instructions");
        StartCoroutine(TweenClosePanel());
        _instructionsPanel.SetActive(false);
        foreach (Button button in _menuButtons)
        {
            button.enabled = true;
        }
    }

    private IEnumerator TweenOpenPanel()
    {
        _optionsMenu.gameObject.SetActive(true);
        _optionsMenu.DOScaleX(1, _optionsTweenTime);
        yield return new WaitForSeconds(_optionsTweenTime);
        _optionsMenu.DOScaleY(1, _optionsTweenTime);
    }

    private IEnumerator TweenClosePanel()
    {
        _optionsMenu.DOScaleY(0.01f, _optionsTweenTime);
        yield return new WaitForSeconds(_optionsTweenTime);
        _optionsMenu.DOScaleX(0.01f, _optionsTweenTime);
        yield return new WaitForSeconds(_optionsTweenTime);
        _optionsMenu.gameObject.SetActive(false);
    }

    public void MuteBGM()
    {
        _muteBGM = !_muteBGM;

        if (_muteBGM) 
        {
            _bgmImage.sprite = _bgmMutedSprite;
            _bgmButtonImage.sprite = _bgmButtonOffSprite;
        }
        else 
        {
            _bgmImage.sprite = _bgmUnmutedSprite;
            _bgmButtonImage.sprite = _bgmButtonOnSprite;
        }
    }

    public void MuteSFX()
    {
        _muteSFX = !_muteSFX;

        if (_muteSFX) 
        {
            _sfxImage.sprite = _sfxMutedSprite;
            _sfxButtonImage.sprite = _sfxButtonOffSprite;
        }
        else 
        {
            _sfxImage.sprite = _sfxUnmutedSprite;
            _sfxButtonImage.sprite = _sfxButtonOnSprite;
        }
    }
}
