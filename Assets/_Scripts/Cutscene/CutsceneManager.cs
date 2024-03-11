using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private string _sceneName;
    [SerializeField] private float _cutsceneDuration;

    [SerializeField] private Image _blackImage;
    [SerializeField] private Button _skipButton;

    private void Update()
    {
        _cutsceneDuration -= Time.deltaTime;
        if (_cutsceneDuration <= 5) _skipButton.interactable = false;
        if (_cutsceneDuration <= 0) SceneManager.LoadScene(_sceneName);
    }

    public void SkipCutscene()
    {
        _skipButton.interactable = false;
        StartCoroutine(Skip());

        IEnumerator Skip()
        {
            _blackImage.DOFade(1, 2f);
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene(_sceneName);
        }
    }
}
