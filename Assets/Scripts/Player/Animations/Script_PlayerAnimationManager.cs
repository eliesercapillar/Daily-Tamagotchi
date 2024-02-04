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

        // Start is called before the first frame update
        private void Start()
        {
            if (_playerAnimator == null) _playerAnimator = GetComponent<Animator>();
        }

        // Update is called once per frame
        private void Update()
        {
            UpdateAnimations();
        }

        private void UpdateAnimations()
        {   
            // Coffee Idle Animation
            if (!_isWaitingForCoffee) StartCoroutine("WaitForCoffee");

            // Walking Animation
            _playerAnimator.SetBool("IsMoving", _locomotionManager.IsMoving);
            
            // Transformation Animation
            if (_actionsManager.HasTransformationStarted) 
            {
                if (_actionsManager.IsEnraged) _playerAnimator.SetTrigger("GIGATransform");
                else                           _playerAnimator.SetTrigger("StrongTransform");
            }
        }

        private IEnumerator WaitForCoffee()
        {
            _isWaitingForCoffee = true;
            yield return new WaitForSeconds(Random.Range(_coffeeWaitTimeMin, _coffeeWaitTimeMax));

            _playerAnimator.SetTrigger("DrinkCoffee");
            _isWaitingForCoffee = false;
        }
    }
}
