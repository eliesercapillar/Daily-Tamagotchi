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
        [SerializeField] Script_PlayerLocomotionManager _locomotionManager;
        [SerializeField] Script_PlayerActionsManager    _actionsManager;
        [SerializeField] Script_PlayerHitboxManager     _hitboxManager;

        [Space(5)]
        [Header("Player Components")]
        [SerializeField] private Animator _playerAnimator;

        // State Variables
        private bool _isTransforming;
        private bool _isAttacking;

        // Getters/Setters
        public bool IsTransforming { get { return _isTransforming; } }
        public bool IsAttacking    { get { return _isAttacking;    } }
        
        #endregion Global Variables

        private void Awake()
        {
            if (_playerAnimator == null) _playerAnimator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (_locomotionManager.IsMoving) PlayWalkingAnimation();
            else                             PlayIdleAnimation();
        }
    
        #region Play Animation Methods

        public void PlayWalkingAnimation()
        {
            switch (_actionsManager.CurrentState)
            {
                case Transformation.Normal:
                    PlayAnimation("Normal Walk");
                    break;
                case Transformation.Strong:
                    PlayAnimation("Strong Walk");
                    break;
                case Transformation.Gigachad:
                    PlayAnimation("Giga Walk");
                    break;  
            }
        }

        public void PlayIdleAnimation()
        {
            switch (_actionsManager.CurrentState)
            {
                case Transformation.Normal:
                    PlayAnimation("Normal Idle");
                    break;
                case Transformation.Strong:
                    PlayAnimation("Strong Idle");
                    break;
                case Transformation.Gigachad:
                    PlayAnimation("Giga Idle");
                    break;  
            }
        }
 
        public void PlayTransformationAnimation()
        {
            switch (_actionsManager.CurrentState)
            {
                case Transformation.Normal:
                    if (_actionsManager.IsEnraged) 
                    {
                        PlayAnimation("Normal Transform Giga");
                        _hitboxManager.EnablePlayerHitbox(Transformation.Gigachad);
                    }
                    else
                    {
                        PlayAnimation("Normal Transform Strong");
                        _hitboxManager.EnablePlayerHitbox(Transformation.Strong);
                    }
                    break;
                case Transformation.Strong:
                    PlayAnimation("Strong Transform Normal");
                    _hitboxManager.EnablePlayerHitbox(Transformation.Normal);
                    break;
                case Transformation.Gigachad:
                    PlayAnimation("Giga Transform Normal");
                    _hitboxManager.EnablePlayerHitbox(Transformation.Normal);
                    break;
            }
        }
    
        public void PlayAttackAnimation()
        {
            switch (_actionsManager.CurrentState)
            {
                case Transformation.Normal:
                    break;
                case Transformation.Strong:
                    PlayAnimation("Strong Attack");
                    break;
                case Transformation.Gigachad:
                    PlayAnimation("Giga Attack");
                    break;
            }
        }

        public void PlayAnimation(string clipName)
        {
            _playerAnimator.Play(clipName, 0);
        }
    
        #endregion Play Animation Methods
    }
}
