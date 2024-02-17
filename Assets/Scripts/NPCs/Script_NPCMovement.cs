using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    public class Script_NPCMovement : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody;

        //[SerializeField] private Path _paths;
        [SerializeField] private GameObject[] _waypoints;
        [SerializeField] private float _moveSpeed;

        [SerializeField] private int currentWaypoint = 0;
        private bool _moveToNextWaypoint = true;

        void Start()
        {

        }

        void Update()
        {
            MoveToWaypoint();
        }

        private void MoveToWaypoint()
        {
            Vector3 destination = _waypoints[currentWaypoint].transform.position;
            Vector3 newPos = Vector3.MoveTowards(transform.position, destination, _moveSpeed * Time.deltaTime);
            
            if (_moveToNextWaypoint)
            {
                transform.position = newPos;
                Debug.Log("Velocity is: " + _rigidbody.velocity);
            }

            float distance = Vector3.Distance(transform.position, destination);
            if (distance <= 0.05)
            {
                currentWaypoint++;
                currentWaypoint %= _waypoints.Length;
                Debug.Log("Moving to waypoint: " + _waypoints[currentWaypoint].name);
                StartCoroutine(HoldAtWaypoint());
            }

            IEnumerator HoldAtWaypoint()
            {
                _moveToNextWaypoint = false;
                yield return new WaitForSeconds(5);
                _moveToNextWaypoint = true;
            }
        }
    }
}
