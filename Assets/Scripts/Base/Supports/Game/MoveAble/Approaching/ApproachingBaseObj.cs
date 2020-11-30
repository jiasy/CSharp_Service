using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Objs {
    //跟踪
    public class ApproachingBaseObj : MoveControlObj {
        public MoveControlObj targetMC = null;
        protected Vector2 _lastTargetV2; //上一个 targetMC所在的位置。
        public int followTargetCount = 0;

        public ApproachingBaseObj (PosType posType_) : base (posType_) {

        }
        public void resetTargetMC (MoveControlObj targetMC_ = null) {
            targetMC = targetMC_;
            followTargetCount = 0;
        }
        public override void Dispose () {
            base.Dispose ();
        }
        public override void updateF () {
            if (targetMC != null) {
                _lastTargetV2 = targetMC.realPos;
                followTargetCount++;
            }
            base.updateF ();
        }
    }
}