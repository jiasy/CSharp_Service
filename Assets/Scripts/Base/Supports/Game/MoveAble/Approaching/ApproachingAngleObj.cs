using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Objs {
    //跟踪
    public class ApproachingAngleObj : ApproachingBaseObj {
        private PursueAngleV2Obj _pursueAngle = new PursueAngleV2Obj ();

        public ApproachingAngleObj (PosType posType_) : base (posType_) { }
        public void resetAsInterval (
            Vector2 currentV2_, //当前位置
            float currentRotation_, //当前转向
            float speedValue_, //移动速度
            float angleSpeed_, //转向速度
            bool isReverseRotation_ = false // 是否是远离
        ) {
            resetRealPos (currentV2_);
            _pursueAngle.resetAsInterval (
                currentV2_, //当前位置
                currentRotation_, //当前角度
                speedValue_, //移动速度
                angleSpeed_, //当前转向速度
                isReverseRotation_
            );
            direction = _pursueAngle.currentRotation;
        }

        public void resetAsEasing (
            Vector2 currentV2_, //当前位置
            float currentRotation_, //当前转向
            float speedValue_, //移动速度
            float angleSX_, //转向系数
            bool isReverseRotation_ = false // 是否是远离
        ) {
            resetRealPos (currentV2_);
            _pursueAngle.resetAsEasing (
                currentV2_, //当前位置
                currentRotation_, //当前角度
                speedValue_, //移动速度
                angleSX_, //当前转向速度
                isReverseRotation_
            );
            direction = _pursueAngle.currentRotation;
        }
        public override void Dispose () {
            base.Dispose ();
        }
        public override void updateF () {
            if (targetMC != null) { //有目标追目标
                _pursueAngle.nextPursue (targetMC.realPos);
            } else { //没目标跑直线
                _pursueAngle.nextPursue ();
            }
            //同步位置
            realPos.x = _pursueAngle.currentV2.x;
            realPos.y = _pursueAngle.currentV2.y;
            direction = _pursueAngle.currentRotation;
            //继续后续逻辑
            base.updateF ();
        }
    }
}