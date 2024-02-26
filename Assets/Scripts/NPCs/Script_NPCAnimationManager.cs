using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    public enum NPCState
    {
        Idle_Up = 0,
        Idle_Down = 1,
        Idle_Left = 2,
        Idle_Right = 3,
        Walk_Up = 4,
        Walk_Down = 5,
        Walk_Left = 6,
        Walk_Right = 7,
        Idle_Book = 8,
        Idle_Phone = 9
    }

    [RequireComponent(typeof(Animator))]
    public class Script_NPCAnimationManager : MonoBehaviour
    {
        [Header("Managers")]
        [SerializeField] private Script_NPCActionsManager _actionsManager;
        [SerializeField] private Script_NPCMovementManager _movementManager;

        [Header("Components")]
        [SerializeField] private Animator _animator;

        [Header("Animation Clips")]
        [SerializeField] private List<string> _animationNames;

        // Animation Flags
        private NPCState _currentState = NPCState.Idle_Down;

        // Special Animation Flags
        private bool _isReading;



        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if (!_actionsManager.IsInteracting)
            {
                if (_movementManager.IsMoving) PlayMovementAnimations();
                else PlayIdleAnimations();
            }
        }

        private void PlayMovementAnimations()
        {
            // Priorities direction of greater influence
            if (_movementManager.IsHorizontalGreater)
            {
                Debug.Log("Horizontal is greater");
                if (_movementManager.IsWalkingRight)
                {
                    PlayAnimation(NPCState.Walk_Right);
                }
                else
                {
                    PlayAnimation(NPCState.Walk_Left);
                }
            }
            else
            {
                Debug.Log("Vertical is greater");
                if (_movementManager.IsWalkingUp)
                {
                    PlayAnimation(NPCState.Walk_Up);
                }
                else
                {
                    PlayAnimation(NPCState.Walk_Down);
                }
            }
        }

        private void PlayIdleAnimations()
        {
            //_animator.SetBool("isMoving", _movementManager.IsMoving);
        }

        private void PlayAnimation(NPCState toState)
        {
            if (toState == _currentState) return;

            Debug.Log("State " + toState + " resolves to " + (int) toState);
            _animator.Play(_animationNames[(int) toState], 0);
            _currentState = toState;
        }
    }
}
