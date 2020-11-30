using Objs;
using UnityEngine;
using Utils;
using Game;

namespace Dis {
    public class MeshBySector : MeshRing {
        //DisplayObj 的控制的用来显示 GameObj 位置，大小，形状 的 MonoBehaviour
        public void resetByShape (ShapeSector sector_) {
            resetByParams (sector_.direction,sector_.angleRange,sector_.radius);
        }
        public void resetByParams (float direction_, float angleRange_, float radius_) {
            angleRange = angleRange_;
            radius = radius_;
            Segments = (int)((float)radius/0.001f);
            if(Segments<50){
                Segments= 50;
            }
            if(Segments>500){
                Segments= 500;
            }
            innerRadius = 0.01f;
            direction = direction_;
            reCreateMesh ();
        }
        // public void Update () {
        //     resetByParams (direction,angleRange,radius);
        // }
    }
}