using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpinningLOS : MonoBehaviour
{
    [SerializeField] private RectTransform _LOS;
    private Coroutine co;

    private void OnEnable()
    {
        co = StartCoroutine(SpinLOS());
        Debug.Log("SpinLOS coroutine started.");
    }

    private void OnDisable()
    {
        StopCoroutine(co);
        Debug.Log("SpinLOS coroutine stopped.");
    }

    private IEnumerator SpinLOS()
    {
        while (true)
        {
            _LOS.Rotate(Vector3.back);
            yield return null;
        }
    }
}
