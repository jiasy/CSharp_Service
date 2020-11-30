using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Objs;

namespace Game {
    //面朝一个方向施加力，方向固定
    public class ForceFaceToObj : ForceObj {
        public ForceFaceToObj (Vector3Int pos_) : base (pos_) {
            
        }
        public override void Dispose () {
            base.Dispose ();
        }

    }
}