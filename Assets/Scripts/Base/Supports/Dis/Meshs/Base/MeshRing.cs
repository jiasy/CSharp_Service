using Objs;
using UnityEngine;
using Utils;
using Game;

namespace Dis {
    [RequireComponent (typeof (MeshRenderer), typeof (MeshFilter))]
    public class MeshRing : MonoBehaviour {
        public float radius = 1.0f;
        public float innerRadius = 0.5f;
        public int Segments = 50;

        public float angleRange = 30.0f;
        public float direction = 0.0f;

        protected MeshFilter _meshFilter;

        void Awake () {
            _meshFilter = GetComponent<MeshFilter> ();
            GetComponent<MeshRenderer>().material.color = Color.white;
        }

        void Start () {

        }

        void resetBy (ShapeSector sector_) {

        }

        public void reCreateMesh () {
            _meshFilter.mesh.Clear ();
            _meshFilter.mesh = CreateMesh ();

        }

        //没有继承者的时候，直接根据自己的 direction 和 angleRange 刷新。
        // public virtual void Update () {
        //     reCreateMesh ();
        // }

        Mesh CreateMesh () {
            float _Radius = radius;
            float _InnerRadius = innerRadius;

            if (_Radius <= 0)
                _Radius = 0.01f;

            if (_InnerRadius <= 0)
                _Radius = 0.005f;

            if (_Radius <= _InnerRadius)
                _InnerRadius = 0.001f;

            if (angleRange > 360.0f) {
                angleRange = 360.0f;
            }

            float currAngle = Mathf.PI;

            int vertCount = (int) ((float) (2 * Segments) * angleRange / 360.0f) + 1;
            if (vertCount % 2 != 0) {
                vertCount++;
            }
            float deltaAngle = 2 * currAngle / Segments;
            Vector3[] vertices = new Vector3[vertCount];
            for (int i = 0; i < vertCount; i += 2, currAngle -= deltaAngle) {
                float cosA = Mathf.Cos (currAngle);
                float sinA = Mathf.Sin (currAngle);
                vertices[i] = new Vector3 (cosA * _InnerRadius, sinA * _InnerRadius, 0);
                vertices[i + 1] = new Vector3 (cosA * _Radius, sinA * _Radius, 0);
            }

            int[] triangles = new int[3 * (vertCount - 2)];
            for (int i = 0, j = 0; i < triangles.Length; i += 6, j += 2) {
                triangles[i] = j + 1;
                triangles[i + 1] = j + 2;
                triangles[i + 2] = j + 0;
                triangles[i + 3] = j + 1;
                triangles[i + 4] = j + 3;
                triangles[i + 5] = j + 2;
            }

            Vector2[] uvs = new Vector2[vertCount];
            for (int i = 0; i < vertCount; ++i) {
                uvs[i] = new Vector2 (vertices[i].x / _Radius / 2 + 0.5f, vertices[i].y / _Radius / 2 + 0.5f);
            }

            Mesh mesh = new Mesh ();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uvs;

            //将当前的transform 旋转，用来迎合 direction
            float _currentRotation = (180.0f - angleRange * 0.5f - direction);
            transform.rotation = Quaternion.Euler (new Vector3 (0f, 0f, -_currentRotation));

            return mesh;
        }

    }
}