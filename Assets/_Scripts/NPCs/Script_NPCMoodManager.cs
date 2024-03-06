using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace NPC
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Script_NPCMoodManager : MonoBehaviour
    {
        private GameManager _gameManager;

        [Header("Mood Sprites")]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Sprite[] _moods;
        [SerializeField] private Sprite _questionMarkSprite;
        [SerializeField] private int _currentMood;
        private int numMoods;

        [Header("Sus Properties")]
        [SerializeField] private float _susIncrementAmount;
        [SerializeField] private float _susDecrementAmount;
        [SerializeField] private float _susCDSeconds;
        private WaitForSeconds _susCDTime;
        private float _currentSusAmount;
        private bool _justAlteredSus;

        [Header("LOS")]
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private SpriteRenderer _auraSpriteRenderer;
        [SerializeField] private Color _startColor;
        [SerializeField] private Color _endColor;

        private bool _isInLOS;
        private bool _wasInLOS;

        // Getters/Setters
        public float SusAmount { get { return _currentSusAmount; } set { _currentSusAmount = value; }}
        public bool IsInLOS    { get { return _isInLOS; } set { _isInLOS = value; }}

        private void Start()
        {
            _gameManager = GameManager._instance;

            numMoods = _moods.Length;
            _currentMood = 0;
            _justAlteredSus = false;
            _isInLOS = false;
            _susCDTime = new WaitForSeconds(_susCDSeconds);
        }
        
        private void Update()
        {
            PollForSus();
        }

        private void PollForSus()
        {
            if (_isInLOS)
            {
                IncrementSUS();
                UpdateMood();
            }
            else
            {
                if (_wasInLOS) ResetMood();
                DecrementSus();
            }
            UpdateLOSColor();
        }

        public void UpdateMood()
        {
            if (_currentSusAmount > 0) _spriteRenderer.enabled = true;

            _currentMood = (int) Mathf.Floor(_currentSusAmount * numMoods / 100.0f);
            if (_currentMood == numMoods)
            {
                _spriteRenderer.sprite = _moods[numMoods - 1];
                _gameManager.GameOver();
            }
            else
            {
                _spriteRenderer.sprite = _moods[_currentMood];
            }
        }

        private void ResetMood()
        {
            if (_wasInLOS) _spriteRenderer.sprite = _questionMarkSprite;
            _wasInLOS = false;
        }

        private void IncrementSUS()
        {
            _wasInLOS = true;
            if (_justAlteredSus) return;
            StartCoroutine(AlterSus(_susIncrementAmount));
        }

        private void DecrementSus()
        {
            if (_justAlteredSus) return;
            StartCoroutine(AlterSus(_susDecrementAmount * -1));
            if (_currentSusAmount == 0) _spriteRenderer.enabled = false;
        }

        private IEnumerator AlterSus(float amount)
        {
            _currentSusAmount += amount;
            _currentSusAmount = Mathf.Clamp(_currentSusAmount, 0.0f, 100.0f);

            _justAlteredSus = true;
            yield return _susCDTime;
            _justAlteredSus = false;
        }

        private void UpdateLOSColor()
        {
            Color c = Color.Lerp(_startColor, _endColor, _currentSusAmount / 100.0f);
            _auraSpriteRenderer.color = c;
            _meshRenderer.material.color = c;
        }

    }

}
