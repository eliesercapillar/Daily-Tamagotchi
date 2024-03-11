using UnityEngine;
using DG.Tweening;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class WinCanvasManager : MonoBehaviour
{
    [Header("Win Text")]
    [SerializeField] private TextMeshProUGUI _winText;
    [SerializeField] private float _fadeTweenTime;
    [SerializeField] private Vector2 _textEndPos;
    [SerializeField] private float _moveTweenTime;

    [Header("Player")]
    [SerializeField] private RectTransform _player;
    [SerializeField] private Animator _animator;

    [Header("Button")]
    [SerializeField] private TextMeshProUGUI _menuButton;
    [SerializeField] private float _menuFadeTweenTime;
    
    private IEnumerator Start()
    {
        yield return FadeInWinText();
        yield return PlayerWalkIn();
    }

    private IEnumerator FadeInWinText()
    {
        _winText.DOFade(1, _fadeTweenTime);
        yield return new WaitForSeconds(_fadeTweenTime);
        _winText.rectTransform.DOAnchorPos(_textEndPos, _moveTweenTime);
    }

    private IEnumerator PlayerWalkIn()
    {
        _animator.Play("Walk and Transform");
        yield return new WaitForSeconds(9f);
        _animator.Play("Normal Idle");
        _menuButton.DOFade(1, _menuFadeTweenTime);
        yield return new WaitForSeconds(_menuFadeTweenTime);
        _menuButton.GetComponent<Button>().interactable = true;
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Scene_MainMenu");
    }
}
