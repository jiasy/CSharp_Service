using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Objs {
    //力
    public class ForceObj : BaseObj {
        public Vector3Int v3;//释放的位置

        public ForceObj (Vector3Int v3_) : base () {
            v3 = v3_;
        }
        public override void Dispose () {
            base.Dispose ();
        }
    }
} 