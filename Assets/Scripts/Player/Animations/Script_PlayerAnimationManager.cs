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
        
        #endregion Global Variables

        private void Start()
        {
            if (_playerAnimator == null) _playerAnimator = GetComponent<Animator>();
        }

        private void Update()
        {
            UpdateAnimations();
        }

        private void UpdateAnimations()
        {   
            PlayCoffeeIdleAnimation();
            PlayWalkingAnimation();
            PlayTransformationAnimation();
            PlayAttackAnimation();
        }
    
        public void PlayCoffeeIdleAnimation()
        {
            if (!_isWaitingForCoffee && !_actionsManager.IsTransformed) StartCoroutine(WaitForCoffee());

            IEnumerator WaitForCoffee()
            {
                _isWaitingForCoffee = true;
                yield return new WaitForSeconds(Random.Range(_coffeeWaitTimeMin, _coffeeWaitTimeMax));

                if (!_actionsManager.IsTransformed) _playerAnimator.SetTrigger("DrinkCoffee");
                _isWaitingForCoffee = false;
            }
        }
    
        public void PlayWalkingAnimation()
        {
            _playerAnimator.SetBool("IsMoving", _locomotionManager.IsMoving);
        }
   
        public void PlayTransformationAnimation()
        {
            if (_actionsManager.HasTransformationStarted) 
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
            if (_actionsManager.IsAttacking)
            {
                _playerAnimator.SetTrigger("IsAttacking");
            }
        }
    }
}
