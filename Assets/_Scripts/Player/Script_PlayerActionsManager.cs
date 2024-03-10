using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class Script_PlayerActionsManager : MonoBehaviour
    {
        #region Global Variables

        [Header("Managers")]
        [SerializeField] Script_PlayerLocomotionManager _locomotionManager;
        [SerializeField] Script_PlayerAnimationManager _animationManager;

        [Space(5)]
        [Header("Action Properties")]
        [Tooltip("The number of seconds needed to pass before rage begins to increment/decrement again.")]
        [SerializeField] private float _rageWaitTimeSeconds;
        [SerializeField] private float _passiveRageIncrementAmount;
        [SerializeField] private float _passiveRageDecrementAmount;

        [Space(5)]
        [Header("Canvas Elements")]
        [SerializeField] private Slider _rageSlider;

        // Action Variables
        private float _rageMeter = 0.0f;

        // State Flags
        private Transformation _currentState;
        private bool _isEnraged;
        private bool _coroutineRunning;

        // Setters/Getters
        public Transformation CurrentState   { get { return _currentState; } }
        public bool IsEnraged                { get { return _isEnraged;    } }

        #endregion Global Variables

        private void Start()
        {
            _currentState = Transformation.Strong;
            _isEnraged = false;
        }

        private void Update()
        {
            //HandleRage();
        }

        private void HandleRage()
        {
            if (!_coroutineRunning)   StartCoroutine(ManageRage());
            IEnumerator ManageRage()
            {
                _coroutineRunning = true;
                yield return new WaitForSeconds(_rageWaitTimeSeconds);

                bool isChad = _currentState == Transformation.Gigachad;
                
                if      (!isChad && !_isEnraged)  IncrementRage(_passiveRageIncrementAmount);
                else if (isChad  && _isEnraged)   DecrementRage(_passiveRageDecrementAmount);

                _coroutineRunning = false;
            }
        }

        private void UpdateRage(float value)
        {
            _rageMeter = value;
            _rageMeter = Mathf.Clamp(_rageMeter, 0.0f, 100.0f);

            // TODO: Lerp values for smooth progress bar
            _rageSlider.value = _rageMeter;

            if (_rageMeter <= 0.0f)
            {
                _isEnraged = false;
                _animationManager.PlayTransformationAnimation();
            }
            else if (_rageMeter >= 100.0f)
            {
                _isEnraged = true;
            }
        }

        public void IncrementRage(float amount)
        {
            UpdateRage(_rageMeter + amount);
        }

        public void DecrementRage(float amount)
        {
            UpdateRage(_rageMeter - amount);
        }

        public void UpdateCurrentState(string state)
        {
            switch (state)
            {
                case "Normal":
                    _currentState = Transformation.Normal;
                    break;
                case "Strong":
                    _currentState = Transformation.Strong;
                    break;
                case "Giga":
                    _currentState = Transformation.Gigachad;
                    break;
            }
            _locomotionManager.UpdateMovementSpeed(_currentState);
        }
    }
}
