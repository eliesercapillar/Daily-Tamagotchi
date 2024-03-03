using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Toolbox;
using System;
using NPC;

namespace MainMenu
{
    public class NPCWander : MonoBehaviour
    {
        [Header("Managers")]
        private GameManager _gameManager;
        [SerializeField] private Script_NPCAnimationManager _animationManager;

        [Header("NPC Properties")]
        [SerializeField] private float _moveSpeed;

        [Header("Pathfinding")]
        [SerializeField] private List<GameObject> _waypoints;   // A list of key waypoints this NPC will go to.
        [SerializeField] private List<Vector3> _pathToWaypoint; // A list of tiles that indicate a path to travel to get to a waypoint.
        [SerializeField] private GameObject _currentWaypoint;   // The current selected target waypoint to travel to.
        
        // State Flags
        private bool _waypointReached = true;
        private bool _isMoving = false;
        private bool _isWalkingRight = false;
        private bool _isWalkingUp = false;
        private bool _isHorizontalGreater = false;

        // Getters/Setters
        public List<GameObject> Waypoints { get { return _waypoints; } set { _waypoints = value; }}
        public bool WaypointReached       { get { return _waypointReached; } }
        public bool IsMoving              { get { return _isMoving; } }
        public bool IsWalkingRight        { get { return _isWalkingRight; } }
        public bool IsWalkingUp           { get { return _isWalkingUp; } }
        public bool IsHorizontalGreater   { get { return _isHorizontalGreater; } }

        private void Start()
        {
            _gameManager = GameManager._instance;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.name == _currentWaypoint.name)
            {
                Debug.Log("NPC " + gameObject.name + " has REACHEDs waypoint: " + _currentWaypoint.name);
                _waypointReached = true;
                _isMoving = false;
                _animationManager.InteractAtWaypoint(other.gameObject);
            }
        }

        public IEnumerator StartPatrolling()
        {
            while (true)
            {
                if (_waypointReached)
                {
                    GetNewWaypoint();
                }
                yield return TraversePath();
            }
        }

        private void GetNewWaypoint()
        {
            GameObject newWaypoint = _waypoints.RandomElement();
            while (newWaypoint == _currentWaypoint)
            {
                newWaypoint = _waypoints.RandomElement();
            }
            _currentWaypoint = newWaypoint;
            _waypointReached = false;
            _pathToWaypoint = AStar.FindPath(_gameManager.Tilemap, transform.position, _currentWaypoint.transform.position);
        }

        private IEnumerator TraversePath()
        {
            foreach (Vector3 destination in _pathToWaypoint)
            {
                yield return MoveToWaypoint(destination);
            }
        }

        private IEnumerator MoveToWaypoint(Vector3 destination)
        {
            float distance = Vector3.Distance(transform.position, destination);
            while (distance > 0.05)
            {
                if (_waypointReached) break;
                transform.position = Vector3.MoveTowards(transform.position, destination, _moveSpeed * Time.deltaTime);
                distance = Vector3.Distance(transform.position, destination);
                _isMoving = true;   
                SetDirectionVariables(transform.position, destination);
                yield return null;
            }
        }
        
        private void SetDirectionVariables(Vector3 npcPos, Vector3 tilePos)
        {
            float deltaX = tilePos.x - npcPos.x;
            float deltaY = tilePos.y - npcPos.y;
            _isWalkingRight = deltaX > 0.00f;
            _isWalkingUp    = deltaY > 0.00f;

            _isHorizontalGreater = Math.Abs(deltaX) > Math.Abs(deltaY);
        }

    }
}
