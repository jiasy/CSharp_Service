using UnityEngine;
using Utils;
namespace Dis {
    [RequireComponent (typeof (MeshRenderer), typeof (MeshFilter))]
    public class MeshLine : MonoBehaviour {
        public float length = 1.0f;

        public float direction = 0.0f;

        private MeshFilter _meshFilter;

        void Awake () {
            _meshFilter = GetComponent<MeshFilter> ();
            GetComponent<MeshRenderer> ().material.color = Color.white;
        }

        void Start () {

        }

        public void reCreateMesh () {
            _meshFilter.mesh.Clear ();
            _meshFilter.mesh = CreateMesh ();
            transform.rotation = Quaternion.Euler (new Vector3 (0f, 0f, direction));
        }

        //没有继承者的时候，直接根据自己的 angle 和 length 刷新。
        // public virtual void Update () {
        //     reCreateMesh ();
        // }

        //只需要创建长度就可以了
        Mesh CreateMesh () {
            float _length2 = length * 0.5f;
            float _lineWidth = 0.1f; //10像素
            float _lineWidth2 = _lineWidth * 0.5f;
            Mesh mesh = new Mesh ();
            mesh.vertices = new Vector3[] {
                new Vector3 (-_length2, -_lineWidth2, 0),
                new Vector3 (-_length2, _lineWidth2, 0),
                new Vector3 (_length2, _lineWidth2, 0),
                new Vector3 (_length2, -_lineWidth2, 0)
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