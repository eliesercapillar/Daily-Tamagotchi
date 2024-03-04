using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
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
    [SerializeField] private Button _optionsButton;
    [SerializeField] private Button _creditsButton;

    // Start is called before the first frame update
    void Start()
    {
        _optionsMenu.gameObject.SetActive(false);
        _optionsMenu.localScale = new Vector3(0.01f, 0.01f, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
