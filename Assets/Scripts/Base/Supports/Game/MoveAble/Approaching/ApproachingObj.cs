using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Objs {
    //跟踪
    public class ApproachingObj : ApproachingBaseObj {
        private PursueV2Obj _pursue = new PursueV2Obj ();

        public ApproachingObj (PosType posType_) : base (posType_) { }
        public void reset (
            Vector2 currentV2_, //当前位置
            float speedValue_ // 速度
        ) {
            resetRealPos (currentV2_);
            _pursue.reset (
                currentV2_, //目标
                currentV2_, //当前
                speedValue_ //速度
            );
            direction = _pursue.direction;
        }

        public override void Dispose () {
            base.Dispose ();
        }
        public override void updateF () {
            if (targetMC != null) { //有目标追目标
                _pursue.resetTargetValue (targetMC.realPos);
            }
            _pursue.next (); //查找下一个点
            realPos.x = _pursue.currentV2.x;//同步位置
            realPos.y = _pursue.currentV2.y;
            direction = _pursue.direction;
            //继续后续逻辑
            base.updateF ();
        }
    }
}