using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

namespace Player
{
    public enum Transformation
    {
        Normal,
        Strong,
        Gigachad
    }

    public class Script_PlayerActions : MonoBehaviour
    {
        #region Global Variables

        [Header("Managers")]
        [SerializeField] Script_PlayerLocomotion _locomotionManager;

        [Space(5)]
        [Header("Action Properties")]
        [SerializeField] private float _rageIncrementAmount;
        [SerializeField] private float _rageDecrementAmount;

        // Action Variables
        private float _rageMeter = 0.0f;

        // State Flags
        private Transformation _currentState;

        private bool _hasTransformationStarted;
        private bool _isEnraged;
        private bool _isTransforming;
        private bool _isTransformed;

        // Setters/Getters
        public bool HasTransformationStarted { get { return _hasTransformationStarted; } }
        public bool IsEnraged                { get { return _isEnraged;                } }
        public bool IsTransforming           { get { return _isTransforming;           } }
        public bool IsTransformed            { get { return _isTransformed;            } }
        public Transformation CurrentState   { get { return _currentState;             } }

        #endregion Global Variables

        private void Start()
        {
            _currentState = Transformation.Normal;
            _isEnraged = false;
            _hasTransformationStarted = false;
            _isTransforming = false;
            _isTransformed = false;
        }

        private void Update()
        {
            HandleInputs();
            HandleRage();
        }

        private void HandleInputs()
        {
            _hasTransformationStarted = Input.GetKeyDown(KeyCode.F);
            if (Input.GetKeyDown(KeyCode.Space)) { HandleAttack(); }
        }

        private void HandleAttack()
        {

        }

        private void HandleRage()
        {
            if (_isEnraged)
            {
                if (_rageMeter <= 0.0f)   { _isEnraged = false; }
            }
            else
            {
                if (_rageMeter >= 100.0f) { _isEnraged = true;  }
            }
        }

        public void IncrementRage()
        {
            _rageMeter += _rageIncrementAmount;
            _rageMeter = Mathf.Clamp(_rageMeter, 0.0f, 100.0f);
        }

        public void DecrementRage()
        {
            _rageMeter -= _rageDecrementAmount;
            _rageMeter = Mathf.Clamp(_rageMeter, 0.0f, 100.0f);
        }

        // Called from the Animator when transfomation animation starts.
        public void OnTransformationStartEvent()
        {
            _isTransforming = true;
        }

        // Called from the Animator after transfomation animation has finished.
        public void OnTransformationEndEvent()
        {
            _isTransformed = true;
            _isTransforming = false;

            UpdateCurrentState();
        }

        // Called from the Animator after revert transformation has finished.
        public void OnRevertTransformationEndEvent()
        {
            _isTransformed = false;
            _isTransforming = false;

            UpdateCurrentState();
        }

        private void UpdateCurrentState()
        {
            if (_isTransformed)
            {
                if (_isEnraged) { _currentState = Transformation.Gigachad; }
                else            { _currentState = Transformation.Strong;   }
            }
            else                { _currentState = Transformation.Normal; }

            _locomotionManager.UpdateMovementSpeed(_currentState);
        }
    }
}
