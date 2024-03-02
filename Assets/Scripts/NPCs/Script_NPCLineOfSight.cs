using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    public class Script_NPCLineOfSight : MonoBehaviour
    {
        [Header("Managers")]
        [SerializeField] private Script_NPCSusManager _susManager;
        [SerializeField] private Script_NPCAnimationManager _animationManager;

        [Header("Player")]
        [SerializeField] private GameObject _player;

        [Header("Components")]
        [SerializeField] private MeshFilter _meshFilter;

        [Header("Field of View Properties")]
        private Vector3[] _meshVertices;
        private Vector2[] _meshUV;
        private int[] _meshTriangles;
        private float _startingAngle;
        private Vector3 _lookDirection;
        [SerializeField] private float _fov;
        [SerializeField] private int _numRays;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private float _minViewDistance;
        [SerializeField] private float _maxViewDistance;
        private float _currViewDistance;

        private bool _isShrinking = false;
        private bool _isExpanding = false;


        public float FOV             {set {_fov = value;}}
        public float MinViewDistance {set {_minViewDistance = value;}}
        public float MaxViewDistance {set {_maxViewDistance = value;}}
    
        private void Start()
        {
            _currViewDistance = _maxViewDistance;
        }

        private void Update()
        {
            PollForPlayerInFOV();
        }

        private void LateUpdate()
        {
            DrawFOVMesh();
        }

        private void PollForPlayerInFOV()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);
            if (distanceToPlayer > _currViewDistance) return;

            Vector3 playerDirection = (_player.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(_lookDirection, playerDirection);
            if (angle > _fov / 2f) return;
            
            RaycastHit2D rayHit = Physics2D.Raycast(transform.position, playerDirection, _currViewDistance);
            if (rayHit.collider == null) return;

            if (rayHit.collider.tag == "")
            {
                _susManager.IsSus = true;
            }
            else
            {
                _susManager.IsSus = false;
            }

        }

        private void DrawFOVMesh()
        {
            float currentAngle = _startingAngle;
            float angleIncrement = _fov / _numRays;

            _meshVertices = new Vector3[_numRays + 1 + 1];
            _meshUV = new Vector2[_numRays + 1 + 1];
            _meshTriangles = new int[_numRays * 3];

            _meshVertices[0] = Vector3.zero;

            int vertexIndex = 1;
            int triangleIndex = 0;
            for (int i = 0; i <= _numRays; i++)
            {
                Vector3 vertex;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, FloatToVectorAngle(currentAngle), _currViewDistance, _layerMask);

                if (hit.collider == null) 
                {
                    //Debug.DrawRay(transform.position, FloatToVectorAngle(currentAngle) * _viewDistance, Color.white);
                    vertex = Vector3.zero + FloatToVectorAngle(currentAngle) * _currViewDistance;
                }
                else if (hit.collider.tag == "TAG_Obstacle")
                {
                    //Debug.DrawRay(transform.position, FloatToVectorAngle(currentAngle) * _viewDistance, Color.red);
                    float distanceToHit = Vector2.Distance(hit.point, transform.position);
                    vertex = Vector3.zero + FloatToVectorAngle(currentAngle) * distanceToHit;
                }
                else if (hit.collider.tag == "TAG_PlayerHitbox")
                {
                    _susManager.IncrementSUS();
                    float distanceToHit = Vector2.Distance(hit.point, transform.position);
                    vertex = Vector3.zero + FloatToVectorAngle(currentAngle) * distanceToHit;
                }
                else
                {
                    Debug.Log("Hit something but not obstacle. What was hit was: " + hit.collider.gameObject.name);
                    vertex = Vector3.zero + FloatToVectorAngle(currentAngle) * _currViewDistance;
                }
                _meshVertices[vertexIndex] = vertex;

                if (i != 0)
                {
                    _meshTriangles[triangleIndex++] = 0;
                    _meshTriangles[triangleIndex++] = vertexIndex - 1;
                    _meshTriangles[triangleIndex++] = vertexIndex;  
                }

                vertexIndex++;
                currentAngle -= angleIncrement; // Clockwise
            }

            _meshFilter.mesh = new Mesh
            {
                vertices = _meshVertices,
                uv = _meshUV,
                triangles = _meshTriangles
            };;
        }

        private Vector3 FloatToVectorAngle(float angle)
        {
            float rad = angle * (Mathf.PI/180f);
            return new Vector3(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;
        }

        private float VectorToFloatAngle(Vector3 direction)
        {
            float angle = Mathf.Atan2(direction.normalized.y, direction.normalized.x) * Mathf.Rad2Deg;
            if (angle < 0) angle += 360;
            return angle;
        }

        public void SetRayDirection(Vector3 direction)
        {
            _lookDirection = direction;
            _startingAngle = VectorToFloatAngle(direction) + _fov / 2;
        }

        public void ShrinkLOS()
        {
            // TODO: Smooth Shrink == Lerp?
            if (!_isShrinking) StartCoroutine(Shrink());

            IEnumerator Shrink()
            {
                _isShrinking = true;
                while (_currViewDistance > _minViewDistance)
                {
                    _currViewDistance--;
                    //Debug.Log($"Shrinking currView, it is {_currViewDistance}");
                    yield return new WaitForSeconds(0.1f);
                }
                _isShrinking = false;
            }
        }

        public void ExpandLOS()
        {
            // TODO: Smooth Expand == Lerp?
            if (!_isExpanding) StartCoroutine(Expand());

            IEnumerator Expand()
            {
                _isExpanding = true;
                while (_currViewDistance < _maxViewDistance)
                {
                    _currViewDistance++;
                    //Debug.Log($"Expanding currView, it is {_currViewDistance}");
                    yield return new WaitForSeconds(0.1f);
                }
                _isExpanding = false;
            }
        }

    
    }
}
