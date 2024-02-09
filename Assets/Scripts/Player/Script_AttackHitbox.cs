using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Script_AttackHitbox : MonoBehaviour
{
    [Header("Object Properties")]
    [SerializeField] BoxCollider2D _hitboxCollider;

    private void Start()
    {
        if (_hitboxCollider == null) _hitboxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Attackable"))
        {
            Debug.Log("HIT!");
        }
        else
        {
            Debug.Log("MISS!");
        }
    }
}
