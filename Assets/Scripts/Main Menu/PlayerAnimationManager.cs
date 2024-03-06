using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private string[] _clipNames;
    private string _previousClip = "";
    private WaitForSeconds _waitTime;

    private void Awake()
    {
        _waitTime = new WaitForSeconds(15f);
    }

    IEnumerator Start()
    {
        while (true)
        {
            string clip;
            do
            {
                clip = _clipNames.RandomElement();
            } while (clip == _previousClip);

            _previousClip = clip;
            _animator.Play(clip);
            yield return _waitTime;
        }
    }
}
