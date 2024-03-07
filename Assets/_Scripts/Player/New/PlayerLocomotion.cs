using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerLocomotion : MonoBehaviour
    {
        #region Global Variables

        [Header("Managers")]
        [SerializeField] Player _playerProperties;
        [SerializeField] Script_PlayerAnimationManager _animationManager;

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
        private bool _hasTransformationStarted;

        // Setters/Getters
        public bool IsMoving { get { return _isMoving; } }
        public float MovementSpeed { get { return _movementSpeed; }  set { _movementSpeed = value; }}
        
        #endregion Global Variables

        private void Start()
        {
            if (_playerRB == null) _playerRB = GetComponent<Rigidbody2D>();
            UpdateMovementSpeed(_playerProperties.CurrentState);
        }

        private void Update()
        {
            HandlePlayerInput();
        }

        private void FixedUpdate()
        {
            if (_animationManager.IsTransforming)   { HaltVelocity();         }
            else if (_animationManager.IsAttacking) { HandleAttackMovement(); }
            else                                    { HandleNormalMovement(); }
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

        private void HandleAttackMovement()
        {
            bool isChad = _playerProperties.CurrentState == Transformation.Gigachad;
            if (isChad) { HaltVelocity(); }
        }

        private void HandleNormalMovement()
        {
            _playerRB.velocity = _movementDirection * _movementSpeed;
        }

        public void UpdateMovementSpeed(Transformation currentState)
        {
            switch (currentState)
            {
                case Transformation.Normal:
                    _movementSpeed = _movementSpeedNormal;
                    break;
                case Transformation.Strong:
                    _movementSpeed = _movementSpeedStrong;
                    break;
                case Transformation.Gigachad:
                    _movementSpeed = _movementSpeedGiga;
                    break;
            }
        }
    
        public void ApplyForce(float forceStrength)
        {
            Vector2 direction = _lookingRight ? Vector2.right : Vector2.left;
            _playerRB.AddForce(direction * forceStrength, ForceMode2D.Impulse);
        }

        public void HaltVelocity()
        {
            _playerRB.velocity = Vector2.zero;
        }
    }
}
