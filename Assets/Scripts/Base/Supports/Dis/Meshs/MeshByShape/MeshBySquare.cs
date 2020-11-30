using Game;
using Objs;
using UnityEngine;
using Utils;

namespace Dis {
    public class MeshBySquare : MeshRing {
        public float side = 1.0f;
        //DisplayObj 的控制的用来显示 GameObj 位置，大小，形状 的 MonoBehaviour
        public void resetByShape (ShapeSquare square_) {
            side = square_.side;
            resetBySide (side * 0.5f);
        }
        public void resetBySide (float side2_) {
            Segments = 4;
            angleRange = 360.0f;
            direction = 45.0f;
            //根号2的值，半径的一半去乘，就得到半径。
            radius = side2_ * 1.414f;
            //内部半径，就减少5像素的大小，这样
            innerRadius = radius - 0.05f;
            reCreateMesh ();
        }

    }
}