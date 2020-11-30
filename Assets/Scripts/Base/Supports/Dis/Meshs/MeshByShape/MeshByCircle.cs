using Objs;
using UnityEngine;
using Utils;
using Game;

namespace Dis {
    public class MeshByCircle : MeshRing {
        //DisplayObj 的控制的用来显示 GameObj 位置，大小，形状 的 MonoBehaviour
        public void resetByShape (ShapeCircle circle_) {
            radius = circle_.radius;
            resetByRadius (radius);
        }
        public void resetByRadius (float radius_) {
            angleRange = 360.0f;
            radius = radius_;
            //内部半径，就减少5像素的大小，这样
            innerRadius = radius - 0.05f;
            reCreateMesh ();
        }

    }
}