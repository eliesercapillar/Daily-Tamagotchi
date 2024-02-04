using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
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
        [Tooltip("The player movement speed when in Normal mode.")]
        [SerializeField] private float _movementSpeedNormal;
        [Tooltip("The player movement speed when in Strong mode.")]
        [SerializeField] private float _movementSpeedStrong;
        [Tooltip("The player movement speed when in Gigachad mode.")]
        [SerializeField] private float _movementSpeedGiga;

        // Movement Variables
        private Vector2 _movementDirection;
        private float _movementSpeed;

        // State Flags
        private bool _isMoving;
        private bool _lookingRight;

        // Setters/Getters
        public bool IsMoving { get { return _isMoving; } }
        public float MovementSpeed { get { return _movementSpeed; }  set { _movementSpeed = value; }}
        
        #endregion Global Variables

        private void Start()
        {
            if (_playerRB == null) _playerRB = GetComponent<Rigidbody2D>();
            UpdateMovementSpeed(_actionManager.CurrentState);
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

            if (moveAmountX != 0)
            {
                transform.localScale = new Vector3(moveAmountX, 1, 1); // Turn towards direction of horizontal movement.
                _lookingRight = moveAmountX == 1;
            }

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

        public void UpdateMovementSpeed(Transformation currentState)
        {
            switch (currentState)
            {
                case Transformation.Normal:
                    _movementSpeed = _movementSpeedNormal;
                    return;
                case Transformation.Strong:
                    _movementSpeed = _movementSpeedStrong;
                    return;
                case Transformation.Gigachad:
                    _movementSpeed = _movementSpeedGiga;
                    return;
            }
        }
    
        public void ApplyForce(Vector2 magnitude)
        {
            //Vector2 direction = _lookingRight ? Vector2.right : Vector2.left;
            Vector2 direction = transform.forward;
            Debug.Log($"Applying force: {magnitude * direction}");
            //_playerRB.AddForce(magnitude * direction * 10, ForceMode2D.Impulse);

            Vector3 currentPos = transform.position;
            Vector3 toPos      = new Vector3(currentPos.x + magnitude.x, currentPos.y + magnitude.y, 0);
        
            transform.position = Vector3.MoveTowards(transform.position, toPos * direction, 100);
        }
    }
}
