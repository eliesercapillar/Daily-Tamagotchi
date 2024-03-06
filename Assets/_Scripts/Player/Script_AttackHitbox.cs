using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Script_AttackHitbox : MonoBehaviour
    {
        #region Global Variables

        private const int ATTACK_LAYER  = 6;
        private const int TERRAIN_LAYER = 7;
        private const int PLAYER_LAYER  = 8;

        [Header("Object Properties")]
        [SerializeField] BoxCollider2D _hitboxCollider;

        #endregion Global Variables

        private void Awake()
        {
            //SetupCollisionIgnores();
        }

        private void Start()
        {
            if (_hitboxCollider == null) _hitboxCollider = GetComponent<BoxCollider2D>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("TAG_Attackable"))
            {
                Debug.Log("HIT!");
            }
            else
            {
                Debug.Log("MISS!");
            }
        }

        private void SetupCollisionIgnores()
        {
            Physics2D.IgnoreLayerCollision(ATTACK_LAYER, TERRAIN_LAYER);
            Physics2D.IgnoreLayerCollision(ATTACK_LAYER, PLAYER_LAYER);
        }
    }
}
