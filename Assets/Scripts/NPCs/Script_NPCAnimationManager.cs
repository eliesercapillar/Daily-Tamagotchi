using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
        [SerializeField] private GameManager _gameManager; 
        [SerializeField] private Script_NPCMovementManager _movementManager;

        [Header("Components")]
        [SerializeField] private Animator _animator;
        [SerializeField] private Script_NPCLineOfSight _los;

        [Header("Animation Clip Names")]
        [SerializeField] private List<string> _animationNames;

        [Header("Interactions")]
        [SerializeField] private float _minInteractTime;
        [SerializeField] private float _maxInteractTime;
        private WaitForSeconds _interactWaitTime;
        private Waypoint _waypointProperties = null;
        private bool _isInteracting;

        [Header("Idling")]
        [SerializeField] private float _minIdleTime;
        [SerializeField] private float _maxIdleTime;
        private WaitForSeconds _idleWaitTime;

        // State
        private NPCState _currentState = NPCState.Idle_Down;
        private bool _isRandomIdling = false;

        // Start is called before the first frame update
        void Start()
        {
            _gameManager = GameManager._instance;
            _interactWaitTime = new WaitForSeconds(UnityEngine.Random.Range(_minInteractTime, _maxInteractTime));
            _idleWaitTime = new WaitForSeconds(UnityEngine.Random.Range(_minIdleTime, _maxIdleTime));
        }

        // Update is called once per frame
        void Update()
        {
            if (!_isRandomIdling && !_isInteracting)
            {
                PlayMovementAnimations();
            }
        }

        private void PlayMovementAnimations()
        {
            // Priorities direction of greater influence
            Debug.Log($"NPC is moving is {_movementManager.IsMoving}");
            if (_movementManager.IsHorizontalGreater)
            {
                if (_movementManager.IsWalkingRight)
                {
                    if (_movementManager.IsMoving) PlayAnimation(NPCState.Walk_Right);
                    else                           PlayAnimation(NPCState.Idle_Right);
                    _los.SetRayDirection(Vector3.right);
                }
                else
                {
                    if (_movementManager.IsMoving) PlayAnimation(NPCState.Walk_Left);
                    else                           PlayAnimation(NPCState.Idle_Left);
                    _los.SetRayDirection(Vector3.left);
                }
            }
            else
            {
                if (_movementManager.IsWalkingUp)
                {
                    if (_movementManager.IsMoving) PlayAnimation(NPCState.Walk_Up);
                    else                           PlayAnimation(NPCState.Idle_Up);
                    _los.SetRayDirection(Vector3.up);
                }
                else
                {
                    PlayAnimation(NPCState.Walk_Down);
                    if (_movementManager.IsMoving) PlayAnimation(NPCState.Walk_Down);
                    else                           PlayAnimation(NPCState.Idle_Down);
                    _los.SetRayDirection(Vector3.down);
                }
            }
        }

        public IEnumerator RandomlyIdle(NPCState behaviour)
        {
            _isRandomIdling = true;
            _los.SetRayDirection(Vector3.down);
            PlayAnimation(behaviour);

            _los.ShrinkLOS();

            bool waitForIdle = true;
            while (waitForIdle)
            {
                yield return _idleWaitTime;
                waitForIdle = false;
                _isRandomIdling = false;
            }
            _los.ExpandLOS();
        }

        public void PlayAnimation(NPCState toState)
        {
            if (toState == _currentState) return;

            _animator.Play(_animationNames[(int) toState], 0);
            _currentState = toState;
        }

        public void InteractAtWaypoint(GameObject waypoint)
        {
            _isInteracting = true;
            _waypointProperties = waypoint.GetComponent<Waypoint>();

            PlayWaypointAnimations(_waypointProperties.NPCBehaviour);
            StartCoroutine(HoldPosition());
            if (_waypointProperties.ShouldInteract)
            {
                _los.ShrinkLOS();
            }

            _isInteracting = false;

            IEnumerator HoldPosition()
            {
                yield return _interactWaitTime;
                
                _gameManager.LetNPCWalk(_movementManager);
                if (_waypointProperties.ShouldInteract) _los.ExpandLOS();
            }
        }

        private void PlayWaypointAnimations(NPCState behaviour)
        {
            switch (behaviour)
            {
                case NPCState.Idle_Up:
                    _los.SetRayDirection(Vector3.up);
                    break;
                case NPCState.Idle_Down:
                    _los.SetRayDirection(Vector3.down);
                    break;
                case NPCState.Idle_Left:
                    _los.SetRayDirection(Vector3.left);
                    break;
                case NPCState.Idle_Right:
                    _los.SetRayDirection(Vector3.right);
                    break;
                case NPCState.Idle_Phone:
                    _los.SetRayDirection(Vector3.down);
                    break;
                case NPCState.Idle_Book:
                    _los.SetRayDirection(Vector3.down);
                    break;
            }
            PlayAnimation(behaviour);
        }
    }
}
