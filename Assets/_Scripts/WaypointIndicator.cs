using UnityEngine;
using UnityEngine.UI;
using Utilities;

public class WaypointIndicator : MonoBehaviour
{
    [SerializeField] private Vector3 _targetPos;
    [SerializeField] private RectTransform _pointerTransform;
    [SerializeField] private Camera _uiCamera;
    [SerializeField] private float _border;

    [Header("Image")]
    [SerializeField] private Image _pointerImage;
    [SerializeField] private Sprite _offScreenSprite;
    [SerializeField] private Sprite _onScreenSprite;

    void Start()
    {
        if (_pointerTransform == null) _pointerTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        MoveTowardsWaypoint();
    }

    private void MoveTowardsWaypoint()
    {
        Vector3 targetWorldPos = Camera.main.WorldToScreenPoint(_targetPos);
        bool isOffScreen = targetWorldPos.x <= _border ||
                           targetWorldPos.x >= Screen.width - _border ||
                           targetWorldPos.y <= _border ||
                           targetWorldPos.y >= Screen.height - _border;

        Vector3 targetScreenPos;
        if (isOffScreen)
        {
            RotateToWaypoint();
            Vector3 cappedPos = targetWorldPos;
            cappedPos.x = Mathf.Clamp(cappedPos.x, _border, Screen.width - _border);
            cappedPos.y = Mathf.Clamp(cappedPos.y, _border, Screen.height - _border);

            targetScreenPos = _uiCamera.ScreenToWorldPoint(cappedPos);
            _pointerImage.sprite = _offScreenSprite;
        }
        else
        {
            targetScreenPos = _uiCamera.ScreenToWorldPoint(targetWorldPos);
            _pointerImage.sprite = _onScreenSprite;
            transform.localEulerAngles = Vector3.zero;
        }

        _pointerTransform.position = targetScreenPos;
        Vector3 local = _pointerTransform.localPosition;
        local.z = 0f;
        _pointerTransform.localPosition = local;
    }

    private void RotateToWaypoint()
    {
        Vector3 currPos = Camera.main.transform.position;
        currPos.z = 0f;

        Vector3 direction = (_targetPos - currPos).normalized;
        float angle = HelperMethods.VectorToFloatAngle(direction);

        _pointerTransform.localEulerAngles = new Vector3(0,0, angle);
    }

    public void GetNewTarget(Vector3 target)
    {
        _targetPos = target;
    }
}
