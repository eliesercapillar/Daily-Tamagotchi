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
        private bool _isTransforming;
        private bool _isAttacking;

        // Getters/Setters
        public bool IsTransforming { get { return _isTransforming; } }
        public bool IsAttacking    { get { return _isAttacking;    } }
        
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
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (!_isTransforming) PlayTransformationAnimation();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                PlayAttackAnimation();
            }
        }

        private void UpdateAnimations()
        {   
            PlayCoffeeIdleAnimation();
            PlayWalkingAnimation();
        }
    
        #region Animation Event Methods

        // Called from the Animator twice during transfomation animation.
        public void OnTransformationEvent(AnimationEvent animEvent)
        {
            _isTransforming = animEvent.intParameter == 1;
            string currentState = animEvent.stringParameter;

            _actionsManager.UpdateCurrentState(currentState);
        }

        // Called from the Animator during the hit frame of the an attack
        public void OnAttackEvent(AnimationEvent animEvent)
        {
            int damage = animEvent.intParameter;
            float forceStrength = animEvent.floatParameter;
            //float forceStrength = 0.1f;

            _isAttacking = animEvent.stringParameter == "";
            _locomotionManager.ApplyForce(forceStrength);

        }

        public void OnAttackStartEvent(AnimationEvent animEvent)
        {
            Debug.Log("Asking to Halt Velocity");
            _isAttacking = animEvent.stringParameter == "";
            _locomotionManager.HaltVelocity();
        }

        #endregion Animation Event Methods

        #region Play Animation Methods

        public void PlayCoffeeIdleAnimation()
        {
            bool isNormal = _actionsManager.CurrentState == Transformation.Normal;

            if (isNormal && !_isWaitingForCoffee) StartCoroutine(WaitForCoffee());
            IEnumerator WaitForCoffee()
            {
                _isWaitingForCoffee = true;
                yield return new WaitForSeconds(Random.Range(_coffeeWaitTimeMin, _coffeeWaitTimeMax));

                if (isNormal) _playerAnimator.SetTrigger("DrinkCoffee");
                _isWaitingForCoffee = false;
            }
        }
    
        public void PlayWalkingAnimation()
        {
            _playerAnimator.SetBool("IsMoving", _locomotionManager.IsMoving);
        }
   
        public void PlayTransformationAnimation()
        {
            switch (_actionsManager.CurrentState)
            {
                case Transformation.Normal:
                    if (_actionsManager.IsEnraged) { _playerAnimator.SetTrigger("GIGATransform");   }
                    else                           { _playerAnimator.SetTrigger("StrongTransform"); }
                    break;
                case Transformation.Strong:
                    _playerAnimator.SetTrigger("RevertTransform");
                    break;
                case Transformation.Gigachad:
                    _playerAnimator.SetTrigger("RevertTransform");
                    break;
            }
        }
    
        public void PlayAttackAnimation()
        {
            bool isNormal = _actionsManager.CurrentState == Transformation.Normal;
            if (!isNormal)  _playerAnimator.SetTrigger("IsAttacking");
        }
    
        #endregion Play Animation Methods
    }
}
