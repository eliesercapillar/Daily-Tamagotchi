using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    public class Script_NPCActionsManager : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Script_NPCLineOfSight _los;

        [Header("Action Properties")]
        [SerializeField] private float _susIncrementAmount;
        [SerializeField] private float _susDecrementAmount;
        [SerializeField] private float _susCDSeconds;

        // Meters
        private float _susAmount = 0.0f;

        // State Flags
        private bool _isInteracting = false;
        private bool _isSus = false;
        private bool _onSusCDTimer = false;
    
        // Getters/Setters
        public float SusAmount    { get { return _susAmount; } }
        public bool IsInteracting { get { return _isInteracting; } }
        public bool IsSus         { get { return _isSus; } set { _isSus = value; }}

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            //PollForSus();
        }

        private void PollForSus()
        {
            if (!_onSusCDTimer)
            {
                if (_isSus)
                {
                    IncrementSUS();
                }
                else
                {
                    DecrementSus();
                }
                //Debug.Log("Hmm im kinda sus rn... " + _susAmount);
                StartCoroutine(CooldownSus());
            }
        }

        public void IncrementSUS()
        {
            _susAmount += _susIncrementAmount;
            _susAmount = Mathf.Clamp(_susAmount, 0.0f, 100.0f);
        }

        private void DecrementSus()
        {
            _susAmount -= _susDecrementAmount;
            _susAmount = Mathf.Clamp(_susAmount, 0.0f, 100.0f);
        }

        private IEnumerator CooldownSus()
        {
            _onSusCDTimer = true;
            yield return new WaitForSeconds(_susCDSeconds);
            _onSusCDTimer = false;
        }
        
    }
}
