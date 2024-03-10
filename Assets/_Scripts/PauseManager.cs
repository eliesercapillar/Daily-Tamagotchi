using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PauseManager : MonoBehaviour
{
    private bool _isGamePaused;

    [Header("Canvases")]
    [SerializeField] private GameObject _pauseCanvas;
    [SerializeField] private GameObject _gameCanvas;

    [Header("UI Elements")]
    [SerializeField] private RectTransform _pausePanel;
    

    // Start is called before the first frame update
    void Start()
    {
        _isGamePaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isGamePaused) ResumeGame();
            else PauseGame();
        }
    }

    public void PauseGame()
    {
        Debug.Log("Pausing Game");

        Time.timeScale = 0;
        _isGamePaused = true;

        _pauseCanvas.SetActive(true);
        _gameCanvas.SetActive(false);
    }

    public void ResumeGame()
    {
        Debug.Log("Resuming Game");

        Time.timeScale = 1;
        _isGamePaused = false;

        _pauseCanvas.SetActive(false);
        _gameCanvas.SetActive(true);
    }

    // private IEnumerator TweenOpenPanel()
    // {
    //     _optionsMenu.gameObject.SetActive(true);
    //     _optionsMenu.DOScaleX(1, _optionsTweenTime);
    //     yield return new WaitForSeconds(_optionsTweenTime);
    //     _optionsMenu.DOScaleY(1, _optionsTweenTime);
    // }

    // private IEnumerator TweenClosePanel()
    // {
    //     _optionsMenu.DOScaleY(0.01f, _optionsTweenTime);
    //     yield return new WaitForSeconds(_optionsTweenTime);
    //     _optionsMenu.DOScaleX(0.01f, _optionsTweenTime);
    //     yield return new WaitForSeconds(_optionsTweenTime);
    //     _optionsMenu.gameObject.SetActive(false);
    // }
}
