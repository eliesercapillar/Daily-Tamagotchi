using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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

    [Header("Buttons")]
    [SerializeField] private Button _menuButton;
    [SerializeField] private Button _restartButton;
    [Space(5)]
    [SerializeField] private RectTransform _buttonsTransform;
    [SerializeField] private Vector2 _buttonEndPos;
    [SerializeField] private float _buttonTweenDuration;

    private IEnumerator Start()
    {
        yield return PlayOpener();
        TweenImage();
        yield return TweenButtons();
    }

    private IEnumerator PlayOpener()
    {
        _animator.Play("Opener", 0);
        AnimatorStateInfo info = _animator.GetCurrentAnimatorStateInfo(0);

        yield return new WaitForSeconds(info.length);
    }

    private void TweenImage()
    {
        _imageTransform.DOAnchorPos(_imageEndPos, _imageTweenDuration);
        _imageTransform.DOScale(_imageEndScale, _imageTweenDuration);
    }

    private IEnumerator TweenButtons()
    {
        _buttonsTransform.DOAnchorPos(_buttonEndPos, _buttonTweenDuration);
        yield return new WaitForSeconds(_buttonTweenDuration);
        _menuButton.interactable = true;
        _restartButton.interactable = true;
    }
}
