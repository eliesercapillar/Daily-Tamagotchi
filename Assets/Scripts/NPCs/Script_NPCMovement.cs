using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace NPC
{
    public class Script_NPCMovement : MonoBehaviour
    {
        private Transform _destination;
        private NavMeshAgent _agent;

        // Getters/Setters
        public Transform Destination { get { return _destination; } set { _destination = value; }}

        void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _agent.updateRotation = false;
            _agent.updateUpAxis = false;
        }

        void Update()
        {
            _agent.SetDestination(_destination.position);
        }
    }
}
