using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Objs;

namespace Game {
    //根据自己所在的位置，计算距离施加力
    public class ForceCircleObj : ForceObj {
        public ForceCircleObj (Vector3Int pos_,int radius_) : base (pos_) {
            
        }
        public override void Dispose () {
            base.Dispose ();
        }

    }
}