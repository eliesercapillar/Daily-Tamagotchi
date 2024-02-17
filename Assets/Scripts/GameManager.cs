using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Toolbox;

public class GameManager : MonoBehaviour
{
    [Header("NPCs / Enemies")]
    [Tooltip("A list of NPC's the player must avoid.")]
    [SerializeField] private List<GameObject> _npcs;
    [SerializeField] private GameObject _adamNPC;

    [Header("Environment")]
    [SerializeField] private Tilemap _floorTilemap;
    [Tooltip("A list of waypoints NPC's may travel to.")]
    [SerializeField] private List<GameObject> _waypoints;
    [SerializeField] private GameObject _specificWaypoint;
    private List<Vector3> _pathToWaypoint;

    [Header("Test")]
    [SerializeField] private Transform _startPos;
    [SerializeField] private Transform _endPos;



    void Start()
    {
        _pathToWaypoint = new List<Vector3>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Calculating Path from " + _adamNPC.transform.position + " to " + _specificWaypoint.transform.position);
            Debug.Log("The closest starting cell is " + _floorTilemap.WorldToCell(_adamNPC.transform.position) + ", and the closest ending cell is " + _floorTilemap.WorldToCell(_specificWaypoint.transform.position));
            //_pathToWaypoint = AStar.FindPathClosest(_floorTilemap, _adamNPC.transform.position, _specificWaypoint.transform.position);
            _pathToWaypoint = AStar.FindPath(_floorTilemap, _startPos.position, _endPos.position);


            if (_pathToWaypoint != null)
            {
                foreach (Vector3 vector in _pathToWaypoint)
                {
                    Debug.Log("Path: " + vector);
                }
            }
            else
            {
                Debug.Log("Path list is null");
            }
        }
    }
}
