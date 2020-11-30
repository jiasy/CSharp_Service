using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Objs {
    //跟踪
    public class ApproachingInertanceObj : ApproachingBaseObj {
        private PursueInertanceV2Obj _pursueInertance = new PursueInertanceV2Obj ();
        public ApproachingInertanceObj (PosType posType_) : base (posType_) { }
        public void reset (
            Vector2 currentV2_, //当前位置
            float currentRotation_, //当前转向
            float speedValue_, //移动速度
            float angleSpeed_ = float.MaxValue //转向速度
        ) {
            resetRealPos (currentV2_);
            _pursueInertance.reset (currentV2_, currentRotation_, speedValue_, angleSpeed_);
            direction = _pursueInertance.currentRotation;
        }

        public override void Dispose () {
            base.Dispose ();
        }
        public override void updateF () {
            if (targetMC != null) { //有目标追目标
                _pursueInertance.nextPursue (targetMC.realPos);
            } else { //没目标跑直线
                _pursueInertance.nextPursue ();
            }
            //同步位置
            realPos.x = _pursueInertance.currentV2.x;
            realPos.y = _pursueInertance.currentV2.y;
            direction = _pursueInertance.currentRotation;
            //继续后续逻辑
            base.updateF ();
        }
    }
}