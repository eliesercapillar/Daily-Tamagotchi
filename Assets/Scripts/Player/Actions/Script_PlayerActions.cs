using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.UI;

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

        [Space(5)]
        [Header("Canvas Elements")]
        [SerializeField] private Slider _rageSlider;

        // Action Variables
        private float _rageMeter = 0.0f;

        // State Flags
        [SerializeField] private Transformation _currentState;
        private bool _isEnraged;
        private bool _coroutineRunning;

        // Setters/Getters
        public Transformation CurrentState   { get { return _currentState; } }
        public bool IsEnraged                { get { return _isEnraged;    } }

        #endregion Global Variables

        private void Start()
        {
            _currentState = Transformation.Normal;
            _isEnraged = false;
        }

        private void Update()
        {
            HandleRage();
        }

        private void HandleRage()
        {
            if (!_coroutineRunning)  StartCoroutine(ManageRage());
            if (_currentState == Transformation.Gigachad)
            {
                if (_rageMeter <= 0.0f)   { _isEnraged = false; }
            }
            else
            {
                if (_rageMeter >= 100.0f) { _isEnraged = true; }
            }

            IEnumerator ManageRage()
            {
                _coroutineRunning = true;
                yield return new WaitForSeconds(2f);

                if (!(_currentState == Transformation.Gigachad))  IncrementRage();
                else                                              DecrementRage();
                _coroutineRunning = false;
            }
        }

        public void IncrementRage()
        {
            _rageMeter += _rageIncrementAmount;
            _rageMeter = Mathf.Clamp(_rageMeter, 0.0f, 100.0f);

            _rageSlider.value = _rageMeter;
        }

        public void DecrementRage()
        {
            _rageMeter -= _rageDecrementAmount;
            _rageMeter = Mathf.Clamp(_rageMeter, 0.0f, 100.0f);

            _rageSlider.value = _rageMeter;
        }

        public void UpdateCurrentState(bool isTransformed)
        {
            if (isTransformed)
            {
                if (_isEnraged) { _currentState = Transformation.Gigachad; }
                else            { _currentState = Transformation.Strong;   }
            }
            else                { _currentState = Transformation.Normal;   }

            _locomotionManager.UpdateMovementSpeed(_currentState);
        }

    }
}
