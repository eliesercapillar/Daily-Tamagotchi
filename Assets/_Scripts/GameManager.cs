using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using NPC;
using System.Linq;
using Player;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;

    [Header("Scene")]
    [SerializeField] private string _nextScene;

    [Header("NPCs / Enemies / Player")]
    [SerializeField] private Player.Player _player;
    [Tooltip("A list of NPC's the player must avoid.")]
    [SerializeField] private List<Script_NPCMovementManager> _npcs;
    private Dictionary<Script_NPCMovementManager, IEnumerator> _npcCoroutines;

    [Header("Environment")]
    [Tooltip("A Tilemap only containing tiles where an NPC MAY NOT walk through.")]
    [SerializeField] private Tilemap _unwalkableTilemap;
    [Tooltip("A list of key waypoints NPC's may travel to.")]
    [SerializeField] private List<GameObject> _NPCWaypoints;
    [Tooltip("A number of key waypoints an NPC will rotate between.")]
    [SerializeField] private int _numWaypoints;
    [Tooltip("A list of key waypoints in the player can interact with.")]
    [SerializeField] private List<GameObject> _playerWaypoints;
    [Tooltip("A number of key waypoints the player will need to interact with.")]
    [SerializeField] private int _numPlayerWaypoints;

    [Header("Game Over Settings")]
    [SerializeField] private Script_PlayerLocomotionManager _playerLocomotion;
    [SerializeField] Image _blackOutPanel;
    [SerializeField] private Camera _camera;
    [SerializeField] private float _zoomSpeed;
    private Color _currentPanelColor;
    private bool _endInProgress;

    [Header("Start Game Settings")]
    [SerializeField] private Image _startingScreen;
    [SerializeField] private float _fadeInTime;
    [SerializeField] private TextMeshProUGUI _dayTextTMP;
    [SerializeField] private string _dayText;
    [SerializeField] private float _charPauseTime;
    [SerializeField] private AudioClip _typingClip;

    public Tilemap Tilemap { get { return _unwalkableTilemap; } set { _unwalkableTilemap = value; }}

    private void Awake()
    {
        if (_instance == null) _instance = this;
        _npcCoroutines = new Dictionary<Script_NPCMovementManager, IEnumerator>();
        _blackOutPanel.gameObject.SetActive(false);
    }

    private IEnumerator Start()
    {
        if (_player == null) _player = Player.Player._instance;
        AssignNPCWaypoints();
        AssignPlayerWaypoints();
        yield return ShowOpening();
        StartNPCBehaviour();
    }

    // void Update()
    // {
    //     PollForNPCInteractions();
    // }

    private void AssignNPCWaypoints()
    {
        foreach (Script_NPCMovementManager npc in _npcs)
        {
            List<GameObject> waypoints = GetRandomWaypoints(_numWaypoints, _NPCWaypoints);
            npc.Waypoints = waypoints;
        }
    }

    private void AssignPlayerWaypoints()
    {
        List<GameObject> waypointGO = GetRandomWaypoints(_numPlayerWaypoints, _playerWaypoints);
        List<Waypoint> waypoints = new List<Waypoint>();

        foreach (GameObject go in waypointGO)
        {
            waypoints.Add(go.GetComponent<Waypoint>());
        }
        _player.Waypoints = waypoints;
    }

    private List<GameObject> GetRandomWaypoints(int numWaypoints, IList<GameObject> waypointList)
    {
        List<GameObject> waypoints = new List<GameObject>();
        int count = 0;
        
        while (count < numWaypoints)
        {
            GameObject waypoint = waypointList.RandomElement();
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

    private void LetNPCWalk(Script_NPCMovementManager npc)
    {
        IEnumerator co = npc.StartPatrolling();
        StartCoroutine(co);
        _npcCoroutines[npc] = co;
    }

    // private void StopNPCWalk(Script_NPCMovementManager npc)
    // {
    //     StopCoroutine(_npcCoroutines[npc]);
    //     _npcCoroutines.Remove(npc);
    // }

    // private void PollForNPCInteractions()
    // {
    //     foreach (Script_NPCMovementManager npc in _npcs)
    //     {
    //         if (npc.WaypointReached && _npcCoroutines.ContainsKey(npc))
    //         {
    //             StopNPCWalk(npc);
    //         }
    //     }
    // }

    #region Game Over
    public void GameOver()
    {
        if (!_endInProgress)
        {
            _endInProgress = true;
            _playerLocomotion.HaltVelocity();
            _playerLocomotion.enabled = false;
            HaltAllNPCs();
            StartCoroutine(WonGame(false));
        }
    }

    public void GameOverSuccess()
    {
        Debug.Log("WOOHOO GAME OVER");
        if (!_endInProgress)
        {
            _endInProgress = true;
            _playerLocomotion.HaltVelocity();
            _playerLocomotion.enabled = false;
            HaltAllNPCs();
            StartCoroutine(WonGame(true));
        }
    }

    private void HaltAllNPCs()
    {
        foreach (var value in _npcCoroutines.Values.ToList())
        {
            StopCoroutine(value);
        }
    }

    private IEnumerator WonGame(bool wonGame)
    {
        if (wonGame)
        {
            yield return BlackOutScreen();
            SceneManager.LoadScene(_nextScene);
        }
        else
        {
            yield return ZoomCamera();
            yield return BlackOutScreen();
            SceneManager.LoadScene("Scene_GameOver");
        }
    }

    private IEnumerator ZoomCamera()
    {
        float progress = 0f;
        
        while (progress < 1f)
        {
            progress += Time.deltaTime * _zoomSpeed;
            _camera.orthographicSize = Mathf.Lerp(5, 1, progress);
            yield return null;
        }
    }

    private IEnumerator BlackOutScreen()
    {
        _blackOutPanel.gameObject.SetActive(true);
        _currentPanelColor = _blackOutPanel.color;

        while (_currentPanelColor.a < 1f)
        {
            _currentPanelColor.a += Time.deltaTime;
            _blackOutPanel.color = _currentPanelColor;
            yield return null;
        }
    }

    #endregion Game Over

    private IEnumerator ShowOpening()
    {
        _startingScreen.gameObject.SetActive(true);
        yield return ShowText();
        _dayTextTMP.DOFade(0, _fadeInTime);
        _startingScreen.DOFade(0, _fadeInTime);
        yield return new WaitForSeconds(_fadeInTime);
        _startingScreen.gameObject.SetActive(false);
    }

    private IEnumerator ShowText()
    {
        string text = "";

        int index = 0;
        while (text != _dayText)
        {
            text += _dayText[index];
            _dayTextTMP.text = text;
            index++;
            SFXManager._instance.PlayAudio(_typingClip);
            yield return new WaitForSeconds(_charPauseTime);
        }
        yield return new WaitForSeconds(1f);
    }
}
