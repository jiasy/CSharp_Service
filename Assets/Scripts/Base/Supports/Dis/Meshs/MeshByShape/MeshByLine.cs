using Objs;
using UnityEngine;
using Utils;
using Game;

namespace Dis {
    public class MeshByLine : MeshLine {

        //DisplayObj 的控制的用来显示 GameObj 位置，大小，形状 的 MonoBehaviour
        public void resetByShape (ShapeLine line_) {
            resetByDictAndLength(line_.direction,line_.length);
        }


        public void resetByDictAndLength(float direction_,float length_){
            length = length_;
            direction = direction_;
            reCreateMesh();
        }
    }
}