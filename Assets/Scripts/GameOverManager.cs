using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class GameOverManager : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] private Animator _animator;

    [Header("Image")]
    [SerializeField] private RectTransform _imageTransform;
    [SerializeField] private Vector2 _imageStartPos;
    [SerializeField] private Vector2 _imageEndPos;
    [Space(5)]
    [SerializeField] private Vector3 _imageStartScale;
    [SerializeField] private Vector3 _imageEndScale;
    [SerializeField] private float _imageTweenDuration;
    [SerializeField] private float _fadeTweenDuration;

    [Header("Buttons")]
    [SerializeField] private Button _menuButton;
    [SerializeField] private Button _restartButton;
    [Space(5)]
    [SerializeField] private RectTransform _buttonsTransform;
    [SerializeField] private Vector2 _buttonStartPos;
    [SerializeField] private Vector2 _buttonEndPos;
    [SerializeField] private float _buttonTweenDuration;

    private IEnumerator Start()
    {
        yield return PlayOpener();
        TweenImage(true);
        yield return TweenButtons(true);
    }

    private IEnumerator PlayOpener()
    {
        _animator.Play("Opener", 0);
        yield return new WaitForSeconds(2.5f);
    }

    private IEnumerator PlayRestart()
    {
        _animator.Play("Restart", 0);
        yield return new WaitForSeconds(6.5f);
    }

    private void TweenImage(bool isOpener)
    {
        if (isOpener)
        {
            _imageTransform.DOAnchorPos(_imageEndPos, _imageTweenDuration);
            _imageTransform.DOScale(_imageEndScale, _imageTweenDuration);
        }
        else
        {
            _imageTransform.DOAnchorPos(_imageStartPos, _imageTweenDuration);
            _imageTransform.DOScale(_imageStartScale, _imageTweenDuration);
        }
    }

    private IEnumerator TweenButtons(bool isOpener)
    {
        if (isOpener)
        {
            _buttonsTransform.DOAnchorPos(_buttonEndPos, _buttonTweenDuration);
            yield return new WaitForSeconds(_buttonTweenDuration);
            _menuButton.interactable = true;
            _restartButton.interactable = true;
        }
        else
        {
            _menuButton.interactable = false;
            _restartButton.interactable = false;
            _buttonsTransform.DOAnchorPos(_buttonStartPos, _buttonTweenDuration);
            yield return null;
        }
    }

    public void RestartGame()
    {
        StartCoroutine(Restart());

        IEnumerator Restart()
        {
            yield return TweenButtons(false);
            TweenImage(false);
            yield return PlayRestart();
            yield return FadeToBlack();
            SceneManager.LoadScene("Scene_Game");
        }
    }

    private IEnumerator FadeToBlack()
    {
        _imageTransform.GetComponent<Image>().DOFade(0, _fadeTweenDuration);
        yield return new WaitForSeconds(_fadeTweenDuration);
    }
}
