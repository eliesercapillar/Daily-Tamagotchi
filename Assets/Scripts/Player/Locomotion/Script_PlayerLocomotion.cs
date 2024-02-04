using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Script_PlayerLocomotion : MonoBehaviour
    {
        #region Global Variables

        [Header("Managers")]
        [SerializeField] Script_PlayerActions _actionManager;

        [Space(5)]
        [Header("Player Components")]
        [SerializeField] private Rigidbody2D _playerRB;

        [Space(5)]
        [Header("Movement Properties")]
        [SerializeField] private float _movementSpeed;

        // Movement Variables
        private Vector2 _movementDirection;

        // State Flags
        private bool _isMoving;

        // Setters/Getters
        public bool IsMoving { get { return _isMoving; } }
        
        #endregion Global Variables

        private void Start()
        {
            if (_playerRB == null) _playerRB = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            HandlePlayerInput();
        }

        private void FixedUpdate()
        {
            HandlePlayerMovement();
        }

        private void HandlePlayerInput()
        {
            float moveAmountX = Input.GetAxisRaw("Horizontal");
            float moveAmountY = Input.GetAxisRaw("Vertical");

            _movementDirection = new Vector2(moveAmountX, moveAmountY);
            _movementDirection = _movementDirection.normalized; // Prevent faster speeds when moving diagonally

            if (_movementDirection.magnitude > 0.0f) { _isMoving = true; }
            else                                     { _isMoving = false;}
        }

        private void HandlePlayerMovement()
        {
            if (_actionManager.IsTransforming) 
            {
                _playerRB.velocity = Vector2.zero;
                return;
            }
            _playerRB.velocity = _movementDirection * _movementSpeed;
        }
    }
}
