using System.Collections;
using UnityEngine;
using TMPro;

public class CutsceneText : MonoBehaviour
{
    [Header("Boss")]
    [SerializeField] private TextMeshProUGUI _bossText;
    [SerializeField] private string _bossString;
    [SerializeField] private string _bossString2;
    
    [Header("Player")]
    [SerializeField] private TextMeshProUGUI _playerText;
    [SerializeField] private string _playerString;
    [SerializeField] private string _playerString2;

    [Header("Text Settings")]
    [SerializeField] private float _typeDelay;
    [SerializeField] private float _transformationDuration;
    [SerializeField] private AudioClip _typingSFX;

    [SerializeField] private bool _isBoss;

    public void OnEnable()
    {
        if (_isBoss) StartCoroutine(PlayBossText());
        else         StartCoroutine(PlayPlayerText());
    }

    public IEnumerator PlayBossText()
    {
        string curr = "";

        int index = 0;
        while (curr != _bossString)
        {
            curr += _bossString[index];
            index++;
            _bossText.text = curr;
            SFXManager._instance.PlayAudio(_typingSFX);
            yield return new WaitForSeconds(_typeDelay);
        }
        yield return new WaitForSeconds(1.5f);

        curr = "";
        _bossText.text = curr;
        index = 0;
        while (curr != _bossString2)
        {
            curr += _bossString2[index];
            index++;
            _bossText.text = curr;
            SFXManager._instance.PlayAudio(_typingSFX);
            yield return new WaitForSeconds(_typeDelay);
        }
    }

    public IEnumerator PlayPlayerText()
    {
        string curr = "";

        int index = 0;
        while (curr != _playerString)
        {
            curr += _playerString[index];
            index++;
            _playerText.text = curr;
            SFXManager._instance.PlayAudio(_typingSFX);
            yield return new WaitForSeconds(_typeDelay);
        }
        yield return new WaitForSeconds(_transformationDuration);

        curr = "";
        _playerText.text = curr;
        index = 0;
        while (curr != _playerString2)
        {
            curr += _playerString2[index];
            index++;
            _playerText.text = curr;
            SFXManager._instance.PlayAudio(_typingSFX);
            yield return new WaitForSeconds(_typeDelay);
        }
    }
}
