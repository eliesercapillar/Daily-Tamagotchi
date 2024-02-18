using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Toolbox;

namespace NPC
{
    public class Script_NPC : MonoBehaviour
    {
        private GameManager _gameManager;

        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private float _moveSpeed;

        [SerializeField] private List<GameObject> _waypoints;   // A list of key waypoints this NPC will go to.
        private List<Vector3> _pathToWaypoint; // A list of tiles that indicate a path to travel to get to a waypoint.
        private GameObject _currentWaypoint;   // The current selected target waypoint to travel to.
        private bool _waypointReached = true;
        private bool _isInteracting = false;

        // Getters/Setters
        public List<GameObject> Waypoints { get { return _waypoints; } set { _waypoints = value; }}
        public bool IsInteracting         { get { return _isInteracting; } set { _isInteracting = value; }}

        private void Start()
        {
            _gameManager = GameManager._instance;
        }

        public IEnumerator StartPatrolling()
        {
            while (true)
            {
                if (!_isInteracting)
                {
                    if (_waypointReached) 
                    {
                        GetNewWaypoint();
                        GetPath();
                    }
                    yield return TraversePath();
                }
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
        }

        private void GetPath()
        {
            _pathToWaypoint = AStar.FindPath(_gameManager.Tilemap, transform.position, _currentWaypoint.transform.position);
        }

        private IEnumerator TraversePath()
        {
            foreach (Vector3 destination in _pathToWaypoint)
            {
                yield return MoveToWaypoint(destination);
            }
            _waypointReached = true;
        }

        private IEnumerator MoveToWaypoint(Vector3 destination)
        {
            float distance = Vector3.Distance(transform.position, destination);
            while (distance > 0.05)
            {
                transform.position = Vector3.MoveTowards(transform.position, destination, _moveSpeed * Time.deltaTime);
                distance = Vector3.Distance(transform.position, destination);
                yield return null;
            }
        }
    }
}
