using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    public enum NPCMood
    {
        Neutral,
        Suspicious1,
        Suspicious2,
        Suspicious3,
        Angry1,
        Angry2,
        Angry3,
        GameOver
    }

    [RequireComponent(typeof(SpriteRenderer))]
    public class Script_NPCMoodManager : MonoBehaviour
    {
        [Header("Managers")]
        [SerializeField] private Script_NPCSusManager _susManager;

        [Header("Mood")]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Sprite[] _moods;
        private int numMoods;

        [SerializeField] private int _currentMood;

        private void Start()
        {
            _currentMood = 0;
            numMoods = _moods.Length;
        }
        
        private void Update()
        {
            UpdateSprite();
        }

        private void UpdateSprite()
        {
            _currentMood = (int) Mathf.Floor(_susManager.SusAmount * numMoods / 100.0f);
            if (_currentMood == numMoods)
            {
                Debug.Log("Game Over");
            }
            else
            {
                _spriteRenderer.sprite = _moods[_currentMood];
            }

        }
    }

}
