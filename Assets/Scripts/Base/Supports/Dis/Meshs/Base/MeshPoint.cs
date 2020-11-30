using Objs;
using UnityEngine;
using Utils;

namespace Dis {
    [RequireComponent (typeof (MeshRenderer), typeof (MeshFilter))]
    public class MeshPoint : MonoBehaviour {
        private MeshFilter _meshFilter;
        void Awake () {
            _meshFilter = GetComponent<MeshFilter> ();
            GetComponent<MeshRenderer>().material.color = Color.white;
        }

        void Start () {
            _meshFilter.mesh = CreateMesh ();
        }

        Mesh CreateMesh () { //点 
            float _lineWidth = 0.1f; //10像素
            float _lineWidth2 = _lineWidth * 0.5f;
            Mesh mesh = new Mesh ();
            mesh.vertices = new Vector3[] {
                new Vector3 (-_lineWidth2, -_lineWidth2, 0),
                new Vector3 (-_lineWidth2, _lineWidth2, 0),
                new Vector3 (_lineWidth2, _lineWidth2, 0),
                new Vector3 (_lineWidth2, -_lineWidth2, 0)
            };
            mesh.triangles = new int[] {
                0,
                1,
                2,
                0,
                2,
                3
            };
            return mesh;
        }

    }
}