using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

namespace Player
{
    public class Script_PlayerHitboxManager : MonoBehaviour
    {
        #region Global Variables

        private const int ATTACK_LAYER  = 6;
        private const int TERRAIN_LAYER = 7;
        private const int PLAYER_LAYER  = 8;

        [Header("Hitboxes")]
        [SerializeField] private Collider2D _playerNormalHitbox;
        [SerializeField] private Collider2D _playerStrongHitbox;
        [SerializeField] private Collider2D _playerGigaHitbox;
        [Space(10)]
        [SerializeField] private Collider2D _attackStrongHitbox;
        [SerializeField] private Collider2D _attackGigaHitbox;

        private Collider2D _currentActiveHitbox;

        #endregion Global Variables

        private void Start()
        {
            _currentActiveHitbox = _playerNormalHitbox;
            _playerNormalHitbox.gameObject.SetActive(true);
            _playerStrongHitbox.gameObject.SetActive(false);
            _playerGigaHitbox.gameObject.SetActive(false);
            SetupCollisionIgnores();
        }

        public void SetupCollisionIgnores()
        {
            //Physics2D.IgnoreLayerCollision(ATTACK_LAYER, TERRAIN_LAYER);
            //Physics2D.IgnoreLayerCollision(ATTACK_LAYER, PLAYER_LAYER);
        }

        public void EnablePlayerHitbox(Transformation transformation)
        {
            _currentActiveHitbox.gameObject.SetActive(false);
            switch (transformation)
            {
                case Transformation.Normal:
                    _playerNormalHitbox.gameObject.SetActive(true);
                    _currentActiveHitbox = _playerNormalHitbox;
                    break;
                case Transformation.Strong:
                    _playerStrongHitbox.gameObject.SetActive(true);
                    _currentActiveHitbox = _playerStrongHitbox;
                    break;
                case Transformation.Gigachad:
                    _playerGigaHitbox.gameObject.SetActive(true);
                    _currentActiveHitbox = _playerGigaHitbox;
                    break;
            }
        }

        // This is a hacky way of moving the hitbox a bit to ensure that there is the change in the overlap of colliders needed to trigger OnTriggerEnter2D.
        public void ShakeHitbox()
        {
            
        }
    }
}
