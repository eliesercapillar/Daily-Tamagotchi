using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Player
{
    public enum Transformation
    {
        Normal,
        Strong,
        Gigachad
    }

    public class Player : MonoBehaviour
    {
        public static Player _instance;
        [SerializeField] private WaypointIndicator _indicator;

        [SerializeField] private List<Waypoint> _waypoints;
        private Dictionary<Waypoint, TextMeshProUGUI> _waypointUIDict;

        private Waypoint _currentWaypoint;
        private Transformation _currentState;

        [Header("Canvas Elements")]
        [SerializeField] private Transform _parent;
        [SerializeField] private TextMeshProUGUI _goalsPrefab;
        [SerializeField] private TextMeshProUGUI _countText;
        [SerializeField] private Color _completedTaskColor;
        [SerializeField] private Color _incompleteTaskColor;

        private int _numCompletedTasks;
        private int _numTasks;

        // Getters/Setters
        public Transformation CurrentState { get { return _currentState; } }
        public Waypoint CurrentWaypoint    { get { return _currentWaypoint; } }

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
        }

        private void Start()
        {
            _waypointUIDict = new Dictionary<Waypoint, TextMeshProUGUI>();
            _numCompletedTasks = 0;
            _numTasks = _waypoints.Count;
            InitializeTaskList();
            UpdateCountText();
            GetNewWaypoint();
        }

        private void UpdateCountText()
        {
            _countText.text = $" {_numCompletedTasks}/{_numTasks}";
            _countText.color = Color.Lerp(_incompleteTaskColor, _completedTaskColor, (float) _numCompletedTasks/_numTasks);
        }

        private void InitializeTaskList()
        {
            foreach (Waypoint waypoint in _waypoints)
            {
                var text = Instantiate(_goalsPrefab, _parent);
                _waypointUIDict[waypoint] = text;
                text.text = waypoint.Description;
                text.gameObject.SetActive(false);
            }
        }

        private void GetNewWaypoint()
        {
            _currentWaypoint = SelectRandomWaypoint();
            _indicator.GetNewTarget(_currentWaypoint.transform.position);
            _waypointUIDict[_currentWaypoint].gameObject.SetActive(true);
        }

        private Waypoint SelectRandomWaypoint()
        {
            _waypoints.RandomElement();

            return _waypoints.RandomElement();
        }

        public void InteractAtWaypoint()
        {
            //_waypointUIDict[_currentWaypoint].color = _completedTaskColor;
            _numCompletedTasks++;

            //TODO: play SFX
            _waypointUIDict[_currentWaypoint].gameObject.SetActive(false);
            if (_waypoints.Contains(_currentWaypoint))  _waypoints.Remove(_currentWaypoint);
            if (_waypoints.Count > 0) GetNewWaypoint();
            else 
            {
                _indicator.gameObject.SetActive(false);
                _currentWaypoint = null;
                GameManager._instance.GameOverSuccess();
            }
            UpdateCountText();
        }
    }
}

