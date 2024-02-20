using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using NPC;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;

    [Header("NPCs / Enemies")]
    [Tooltip("A list of NPC's the player must avoid.")]
    [SerializeField] private List<Script_NPCMovementManager> _npcs;
    private Dictionary<Script_NPCMovementManager, IEnumerator> _npcCoroutines;

    [Header("Environment")]
    [Tooltip("A Tilemap only containing tiles where an NPC MAY NOT walk through.")]
    [SerializeField] private Tilemap _unwalkableTilemap;
    [Tooltip("A list of key waypoints NPC's may travel to.")]
    [SerializeField] private List<GameObject> _waypoints;
    [Tooltip("A list of key waypoints in the player office NPC's may travel to.")]
    [SerializeField] private List<GameObject> _playerOfficeWaypoints;
    [Tooltip("A number of key waypoints an NPC will rotate between.")]
    [SerializeField] private int _numWaypoints;

    public Tilemap Tilemap { get { return _unwalkableTilemap; } set { _unwalkableTilemap = value; }}

    private void Awake()
    {
        if (_instance == null) _instance = this;
        _npcCoroutines = new Dictionary<Script_NPCMovementManager, IEnumerator>();
    }

    private void Start()
    {
        AssignNPCWaypoints();
        StartNPCBehaviour();
    }

    void Update()
    {
        PollForNPCInteractions();
    }

    private void AssignNPCWaypoints()
    {
        foreach (Script_NPCMovementManager npc in _npcs)
        {
            List<GameObject> waypoints = GetRandomWaypoints();
            npc.Waypoints = waypoints;
        }
    }

    private List<GameObject> GetRandomWaypoints()
    {
        List<GameObject> waypoints = new List<GameObject>();
        int count = 0;
        
        while (count < _numWaypoints)
        {
            GameObject waypoint = _waypoints.RandomElement();
            if (!waypoints.Contains(waypoint))
            {
                waypoints.Add(waypoint);
                count++;
            }
        }
        return waypoints;
    }

    private void StartNPCBehaviour()
    {
        foreach (Script_NPCMovementManager npc in _npcs)
        {
            LetNPCWalk(npc);
        }
    }

    public void LetNPCWalk(Script_NPCMovementManager npc)
    {
        IEnumerator co = npc.StartPatrolling();
        StartCoroutine(co);
        _npcCoroutines.Add(npc, co);
    }

    public void StopNPCWalk(Script_NPCMovementManager npc)
    {
        Debug.Log("Killing Coroutine");
        StopCoroutine(_npcCoroutines[npc]);
        _npcCoroutines.Remove(npc);
    }

    private void PollForNPCInteractions()
    {
        foreach (Script_NPCMovementManager npc in _npcs)
        {
            if (npc.WaypointReached && _npcCoroutines.ContainsKey(npc))
            {
                StopNPCWalk(npc);
            }
        }
    }
}
