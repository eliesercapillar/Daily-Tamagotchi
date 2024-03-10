using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

namespace Player
{
    public class Script_PlayerHitboxManager : MonoBehaviour
    {
        #region Global Variables

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
            _playerNormalHitbox.gameObject.SetActive(false);
            _playerStrongHitbox.gameObject.SetActive(true);
            _playerGigaHitbox.gameObject.SetActive(false);
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
    }
}
