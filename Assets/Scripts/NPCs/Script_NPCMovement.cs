using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Toolbox;
using System.Threading.Tasks;

namespace NPC
{
    public class Script_NPCMovement : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody;

        //[SerializeField] private Path _paths;
        [SerializeField] private List<Vector3> _pathToWaypoint;
        [SerializeField] private GameObject _specificWaypoint;
        [SerializeField] private float _moveSpeed;

        [SerializeField] private Tilemap _floorTilemap;

        [SerializeField] private int currentWaypoint = 0;
        private bool _moveToNextWaypoint = true;

        void Start()
        {
            GetPath();
            TraversePath();
        }

        private void GetPath()
        {
            Debug.Log("Getting Path");
            _pathToWaypoint = AStar.FindPath(_floorTilemap, transform.position, _specificWaypoint.transform.position);
        }

        private async void TraversePath()
        {
            foreach (Vector3 destination in _pathToWaypoint)
            {
                Debug.Log("Moving through path");
                await MoveToWaypoint(destination);
            }
        }

        private async Task MoveToWaypoint(Vector3 destination)
        {
            float distance = Vector3.Distance(transform.position, destination);
            while (distance > 0.05)
            {
                transform.position = Vector3.MoveTowards(transform.position, destination, _moveSpeed * Time.deltaTime);
                distance = Vector3.Distance(transform.position, destination);
                await Task.Yield();
            }

            // Vector3 destination = _waypoints[currentWaypoint].transform.position;
            // Vector3 newPos = Vector3.MoveTowards(transform.position, destination, _moveSpeed * Time.deltaTime);
            
            // if (_moveToNextWaypoint)
            // {
            //     transform.position = newPos;
            //     Debug.Log("Velocity is: " + _rigidbody.velocity);
            // }

            // float distance = Vector3.Distance(transform.position, destination);
            // if (distance <= 0.05)
            // {
            //     currentWaypoint++;
            //     currentWaypoint %= _waypoints.Length;
            //     Debug.Log("Moving to waypoint: " + _waypoints[currentWaypoint].name);
            //     StartCoroutine(HoldAtWaypoint());
            // }

            // IEnumerator HoldAtWaypoint()
            // {
            //     _moveToNextWaypoint = false;
            //     yield return new WaitForSeconds(5);
            //     _moveToNextWaypoint = true;
            // }
        }
    }
}
