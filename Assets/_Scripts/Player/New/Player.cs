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
        [SerializeField] private Color _completedTaskColor;

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
            InitializeTaskList();
            GetNewWaypoint();
        }

        private void InitializeTaskList()
        {
            foreach (Waypoint waypoint in _waypoints)
            {
                var text = Instantiate(_goalsPrefab, _parent);
                _waypointUIDict[waypoint] = text;
                text.text = waypoint.Description;
            }
        }

        private void GetNewWaypoint()
        {
            _currentWaypoint = SelectRandomWaypoint();
            _indicator.GetNewTarget(_currentWaypoint.transform.position);
        }

        private Waypoint SelectRandomWaypoint()
        {
            _waypoints.RandomElement();

            return _waypoints.RandomElement();
        }

        public void InteractAtWaypoint()
        {
            _waypointUIDict[_currentWaypoint].color = _completedTaskColor;
            _waypoints.Remove(_currentWaypoint);
            if (_waypoints.Count > 0) GetNewWaypoint();
            else 
            {
                _indicator.gameObject.SetActive(false);
                GameManager._instance.GameOverSuccess();
            }
        }
    }
}

