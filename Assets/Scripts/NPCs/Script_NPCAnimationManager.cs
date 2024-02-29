using System;
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
        [SerializeField] private GameManager _gameManager; 
        [SerializeField] private Script_NPCActionsManager _actionsManager;
        [SerializeField] private Script_NPCMovementManager _movementManager;

        [Header("Components")]
        [SerializeField] private Animator _animator;
        [SerializeField] private Script_NPCLineOfSight _los;

        [Header("Animation Clip Names")]
        [SerializeField] private List<string> _animationNames;

        [Header("Interactions")]
        [SerializeField] private float _minInteractTime;
        [SerializeField] private float _maxInteractTime;
        Waypoint _waypointProperties = null;

        // State
        private NPCState _currentState = NPCState.Idle_Down;



        // Start is called before the first frame update
        void Start()
        {
            _gameManager = GameManager._instance;
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
                if (_movementManager.IsWalkingRight)
                {
                    PlayAnimation(NPCState.Walk_Right);
                    Debug.Log("Setting Ray Direction Right");
                    _los.SetRayDirection(Vector3.right);
                }
                else
                {
                    PlayAnimation(NPCState.Walk_Left);
                    Debug.Log("Setting Ray Direction Left");
                    _los.SetRayDirection(Vector3.left);
                }
            }
            else
            {
                if (_movementManager.IsWalkingUp)
                {
                    PlayAnimation(NPCState.Walk_Up);
                    Debug.Log("Setting Ray Direction Up");
                    _los.SetRayDirection(Vector3.up);
                }
                else
                {
                    PlayAnimation(NPCState.Walk_Down);
                    Debug.Log("Setting Ray Direction Down");
                    _los.SetRayDirection(Vector3.down);
                }
            }
        }

        private void PlayIdleAnimations()
        {
            switch (_waypointProperties.NPCBehaviour)
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
            PlayAnimation(_waypointProperties.NPCBehaviour);
        }

        private void PlayAnimation(NPCState toState)
        {
            if (toState == _currentState) return;

            //Debug.Log("State " + toState + " resolves to " + (int) toState);
            _animator.Play(_animationNames[(int) toState], 0);
            _currentState = toState;
        }

        public void InteractAtWaypoint(GameObject waypoint)
        {
            _waypointProperties = waypoint.GetComponent<Waypoint>();
            StartCoroutine(HoldPosition());
            if (_waypointProperties.ShouldInteract)
            {
                _los.ShrinkLOS();
            }

            IEnumerator HoldPosition()
            {
                yield return new WaitForSeconds(UnityEngine.Random.Range(_minInteractTime, _maxInteractTime));
                
                _gameManager.LetNPCWalk(_movementManager);
                if (_waypointProperties.ShouldInteract) _los.ExpandLOS();
            }
        }
    }
}
