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
        private bool _hasTransformationStarted;
        private bool _isEnraged;
        private bool _isTransforming;
        private bool _isTransformed;
        private bool _isAttacking;
        private bool _coroutineRunning;

        // Setters/Getters
        public Transformation CurrentState   { get { return _currentState;             } }
        public bool HasTransformationStarted { get { return _hasTransformationStarted; } }
        public bool IsEnraged                { get { return _isEnraged;                } }
        public bool IsTransforming           { get { return _isTransforming;           } }
        public bool IsTransformed            { get { return _isTransformed;            } }
        public bool IsAttacking              { get { return _isAttacking;              } }

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
            if (!_isTransforming) _hasTransformationStarted = Input.GetKeyDown(KeyCode.F);
            if (_isTransformed)   _isAttacking              = Input.GetKeyDown(KeyCode.Space);
            //if (Input.GetKeyDown(KeyCode.Space)) { HandleAttack(); }
        }

        private void AccrueRage()
        {
            if (!_coroutineRunning)  StartCoroutine(Rage());

            IEnumerator Rage()
            {
                _coroutineRunning = true;
                yield return new WaitForSeconds(2f);
                if (!_isEnraged)    IncrementRage();
                _coroutineRunning = false;
            }
        }

        private void HandleAttack()
        {

        }

        private void HandleRage()
        {
            if (_currentState == Transformation.Gigachad)
            {
                DecrementRage();
                if (_rageMeter <= 0.0f){ _isEnraged = false; }
            }
            else
            {
                if (_rageMeter >= 100.0f) { _isEnraged = true;  }
            }

            AccrueRage();
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

        // Called from the Animator during the hit frame of the an attack
        public void OnAttackEvent(AnimationEvent animEvent)
        {
            int damage = animEvent.intParameter;
            float movementAmount = animEvent.floatParameter;

            Vector2 magnitude = new Vector2(movementAmount, 0);
            _locomotionManager.ApplyForce(magnitude);


        }
    }
}
