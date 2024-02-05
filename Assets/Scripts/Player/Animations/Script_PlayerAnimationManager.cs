using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Animator))]
    public class Script_PlayerAnimationManager : MonoBehaviour
    {
        #region Global Variables

        [Header("Managers")]
        [SerializeField] Script_PlayerLocomotion _locomotionManager;
        [SerializeField] Script_PlayerActions    _actionsManager;

        [Space(5)]
        [Header("Player Components")]
        [SerializeField] private Animator _playerAnimator;

        [Space(5)]
        [Header("Animation Properties")]
        [SerializeField] private float _coffeeWaitTimeMin;
        [SerializeField] private float _coffeeWaitTimeMax;
        private bool _isWaitingForCoffee = false;

        // State Variables
        private bool _hasTransformationStarted;
        private bool _isAttacking;
        private bool _isTransforming;
        private bool _isTransformed;

        // Getters/Setters
        public bool IsAttacking    { get { return _isAttacking;    } }
        public bool IsTransforming { get { return _isTransforming; } }
        public bool IsTransformed  { get { return _isTransformed;  } }
        
        #endregion Global Variables

        private void Start()
        {
            if (_playerAnimator == null) _playerAnimator = GetComponent<Animator>();
        }

        private void Update()
        {
            HandleInputs();
            UpdateAnimations();
        }

        private void HandleInputs()
        {
            if (!_isTransforming) _hasTransformationStarted = Input.GetKeyDown(KeyCode.F);
            if (_isTransformed)   _isAttacking              = Input.GetKeyDown(KeyCode.Space);
            //if (Input.GetKeyDown(KeyCode.Space)) { HandleAttack(); }
        }

        private void UpdateAnimations()
        {   
            PlayCoffeeIdleAnimation();
            PlayWalkingAnimation();
            PlayTransformationAnimation();
            PlayAttackAnimation();
        }
    
        #region Animation Event Methods

        // Called from the Animator when transfomation animation starts.
        public void OnTransformationStartEvent(AnimationEvent animEvent)
        {
            _isTransforming = animEvent.stringParameter == "START";
        }

        // Called from the Animator after transfomation animation has finished.
        public void OnTransformationEndEvent(AnimationEvent animEvent)
        {
            _isTransformed = animEvent.stringParameter == "TRANSFORMED";
            _isTransforming = false;

            _actionsManager.UpdateCurrentState(_isTransformed);
        }

        // Called from the Animator during the hit frame of the an attack
        public void OnAttackEvent(AnimationEvent animEvent)
        {
            int damage = animEvent.intParameter;
            float movementAmount = animEvent.floatParameter;

            Vector2 magnitude = new Vector2(movementAmount, 0);
            _locomotionManager.ApplyForce(magnitude);

        }

        #endregion Animation Event Methods

        #region Play Animation Methods

        public void PlayCoffeeIdleAnimation()
        {
            if (!_isWaitingForCoffee && !_isTransformed) StartCoroutine(WaitForCoffee());

            IEnumerator WaitForCoffee()
            {
                _isWaitingForCoffee = true;
                yield return new WaitForSeconds(Random.Range(_coffeeWaitTimeMin, _coffeeWaitTimeMax));

                if (!_isTransformed) _playerAnimator.SetTrigger("DrinkCoffee");
                _isWaitingForCoffee = false;
            }
        }
    
        public void PlayWalkingAnimation()
        {
            _playerAnimator.SetBool("IsMoving", _locomotionManager.IsMoving);
        }
   
        public void PlayTransformationAnimation()
        {
            if (_hasTransformationStarted) 
            {
                switch (_actionsManager.CurrentState)
                {
                    case Transformation.Normal:
                        if (_actionsManager.IsEnraged) { _playerAnimator.SetTrigger("GIGATransform");   }
                        else                           { _playerAnimator.SetTrigger("StrongTransform"); }
                        return;
                    default:
                        _playerAnimator.SetTrigger("RevertTransform");
                        return;
                }
            }
        }
    
        public void PlayAttackAnimation()
        {
            if (_isAttacking)
            {
                _playerAnimator.SetTrigger("IsAttacking");
            }
        }
    
        #endregion Play Animation Methods
    }
}
