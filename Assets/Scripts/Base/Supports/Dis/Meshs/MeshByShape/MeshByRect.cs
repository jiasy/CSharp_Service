using System;
using Objs;
using UnityEngine;
using Utils;
using Game;

namespace Dis {

    public class MeshByRect : MonoBehaviour {
        public float width = 1.0f;
        public float height = 1.0f;

        public MeshRing _meshRing;

        public void Awake () {
            GameObject _meshRingGO = new GameObject ();
            //添加 MeshRing
            _meshRing = _meshRingGO.AddComponent<MeshRing> ();
            _meshRing.name = "meshRing";
            //将它作为子对象加载到自己身上。
            DisplayUtils.addBToA (this.gameObject, _meshRingGO);
        }
        //DisplayObj 的控制的用来显示 GameObj 位置，大小，形状 的 MonoBehaviour
        public void resetByShape (ShapeRect rect_) {
            width = rect_.width;
            height = rect_.height;
            resetByWH (width * 0.5f, height * 0.5f);
        }
        public void resetByWH (float width2_, float height2_) {
            if (Double.IsInfinity(height2_ / width2_)) {//无穷大就过滤掉
                return;
            }
            float _rate = height2_ / width2_;
            _meshRing.Segments = 4;
            //然后按照 width/height 的比值，将其旋转
            _meshRing.angleRange = 360.0f;
            _meshRing.direction = 45.0f;
            //根号2的值，半径的一半去乘，就得到半径。
            _meshRing.radius = width2_ * 1.414f;
            //内部半径，就减少5像素的大小，这样
            _meshRing.innerRadius = _meshRing.radius - 0.05f;
            //x方向是正常大小，y方向需要按照比例放缩， 自己容器的放缩带动子显示对象放缩。
            transform.localScale = new Vector3 (1, height2_ / width2_, 1);
            _meshRing.reCreateMesh ();
            //调整位置
            _meshRing.transform.localPosition = new Vector3 (0, 0, 0);
        }

    }
}