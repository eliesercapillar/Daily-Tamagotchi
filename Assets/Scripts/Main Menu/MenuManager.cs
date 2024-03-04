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
    [SerializeField] private Vector2 _optionsFromPos;
    [SerializeField] private Vector2 _optionsToPos;
    [SerializeField] private float _optionsTweenTime;

    [Header("Buttons")]
    [SerializeField] private List<Button> _menuButtons;

    [Header("Buttons")]
    [SerializeField] private Image _bgmImage;
    [SerializeField] private Image _bgmButtonImage;
    [SerializeField] private Sprite _bgmButtonOffSprite;
    [SerializeField] private Sprite _bgmButtonOnSprite;
    [SerializeField] private Sprite _bgmUnmutedSprite;
    [SerializeField] private Sprite _bgmMutedSprite;
    [Space(5)]
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
        SceneManager.LoadScene("Scene_Game");
    }

    public void ShowOptionsMenu()
    {
        Debug.Log("Showing Options Menu");
        foreach (Button button in _menuButtons)
        {
            button.enabled = false;
        }

        StartCoroutine(TweenOptions());

        IEnumerator TweenOptions()
        {
            _optionsMenu.gameObject.SetActive(true);
            _optionsMenu.DOScaleX(1, _optionsTweenTime);
            yield return new WaitForSeconds(_optionsTweenTime);
            _optionsMenu.DOScaleY(1, _optionsTweenTime);
        }
    }

    public void CloseOptionsMenu()
    {
        Debug.Log("Closing Options Menu");
        StartCoroutine(TweenOptions());
        foreach (Button button in _menuButtons)
        {
            button.enabled = true;
        }

        IEnumerator TweenOptions()
        {
            _optionsMenu.DOScaleY(0.01f, _optionsTweenTime);
            yield return new WaitForSeconds(_optionsTweenTime);
            _optionsMenu.DOScaleX(0.01f, _optionsTweenTime);
            yield return new WaitForSeconds(_optionsTweenTime);
            _optionsMenu.gameObject.SetActive(false);
        }
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
