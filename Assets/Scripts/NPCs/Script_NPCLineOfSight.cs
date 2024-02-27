using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

namespace NPC
{
    public class Script_NPCLineOfSight : MonoBehaviour
    {
        [Header("Managers")]
        [SerializeField] private Script_NPCActionsManager _actionsManager;
        [SerializeField] private Script_NPCAnimationManager _animationManager;

        [Header("Components")]
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private MeshRenderer _meshRenderer;

        [Header("Field of View Properties")]
        [SerializeField] private Vector3[] _meshVertices;
        [SerializeField] private Vector2[] _meshUV;
        [SerializeField] private int[] _meshTriangles;
        [Space(20)]
        [SerializeField] private Transform _parent;
        [SerializeField] private Vector3 _origin = Vector3.zero;
        [SerializeField] private float _fov;
        [SerializeField] private int _numRays;
        [SerializeField] private float _viewDistance;
        [SerializeField] private float _startingAngle;
        [SerializeField] private LayerMask _layerMask;

        public Vector3 Origin {set {_origin = value;}}

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.tag == "TAG_PlayerHitbox")
            {
                _actionsManager.IsSus = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.tag == "TAG_PlayerHitbox")
            {
                _actionsManager.IsSus = false;
            }
        }
    
        private void Start()
        {
            // Debug.Log("Creating FOV Mesh");
            // CreateFOVMesh();
        }

        private void Update()
        {
            Debug.Log("Creating FOV Mesh");
            CreateFOVMesh();
            //RotateLOS();
        }

        private void CreateFOVMesh()
        {
            float currentAngle = _startingAngle - VectorToFloatAngle(Vector3.down);
            Debug.Log($"Current angle at FOV mesh start is {currentAngle}");
            float angleIncrement = _fov / _numRays;

            _meshVertices = new Vector3[_numRays + 1 + 1];
            _meshUV = new Vector2[_numRays + 1 + 1];
            _meshTriangles = new int[_numRays * 3];

            _meshVertices[0] = _origin;

            int vertexIndex = 1;
            int triangleIndex = 0;
            for (int i = 0; i <= _numRays; i++)
            {
                Vector3 vertex = _origin + FloatToVectorAngle(currentAngle) * _viewDistance;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, FloatToVectorAngle(currentAngle), _viewDistance, _layerMask);

                if (hit.collider == null) 
                {
                    Debug.Log("Hit Nothing.");
                    //Debug.DrawRay(transform.position, FloatToVectorAngle(currentAngle) * _viewDistance, Color.white);
                    vertex = _origin + FloatToVectorAngle(currentAngle) * _viewDistance;
                }
                else if (hit.collider.tag == "TAG_Obstacle")
                {
                    //Debug.Log($"Hit Obstacle at point: {hit.point}");
                    Debug.Log($"Hit Obstacle {hit.collider.gameObject.name}.");
                    //Debug.DrawRay(transform.position, FloatToVectorAngle(currentAngle) * _viewDistance, Color.red);
                    float distance = Vector2.Distance(hit.point, transform.position);
                    vertex = _origin + FloatToVectorAngle(currentAngle) * distance;
                }
                else
                {
                    //vertex = _origin + FloatToVectorAngle(currentAngle) * _viewDistance;
                    Debug.Log("Hit something but not obstacle. What was hit was: " + hit.collider.gameObject.name);
                }

                _meshVertices[vertexIndex] = vertex;

                if (i == 0) continue;
                _meshTriangles[triangleIndex++] = 0;
                _meshTriangles[triangleIndex++] = vertexIndex - 1;
                _meshTriangles[triangleIndex++] = vertexIndex;

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
            _startingAngle = VectorToFloatAngle(direction) - _fov / 2;
            Debug.Log($"Setting Ray Direction is {direction}.\nStarting angle is {_startingAngle}");
        }

        private void RotateLOS()
        {

        }

        public void ShrinkLOS()
        {

        }

        public void ExpandLOS()
        {

        }

    
    }
}
