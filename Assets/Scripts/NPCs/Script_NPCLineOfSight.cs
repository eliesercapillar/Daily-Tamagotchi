using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        [SerializeField] private Vector3 _origin;
        [SerializeField] private float _fov;
        [SerializeField] private int _numRays;
        [SerializeField] private float _viewDistance;

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
            CreateFOVMesh();
        }

        private void Update()
        {
            //RotateLOS();
        }

        private void CreateFOVMesh()
        {
            float currentAngle = 0f;
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
            return new Vector3(Mathf.Cos(rad), Mathf.Sin(rad));
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
