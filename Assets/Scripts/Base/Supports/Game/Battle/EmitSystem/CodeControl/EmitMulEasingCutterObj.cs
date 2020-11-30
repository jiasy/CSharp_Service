using System;
using System.Collections;
using System.Collections.Generic;
using Objs;
using UnityEngine;

namespace Game {
    //单个摆动
    public class EmitMulEasingCutterObj : EmitBaseObj {
        protected RangeEasingCutterObj _rangeEasingObj = new RangeEasingCutterObj ();

        public EmitMulEasingCutterObj () : base () {

        }

        public void reset (int frameInterval_, float centerValue_, float valueRange_, float ease_ , float minDis_ , bool needNegative_ = false) {
            resetInterval (frameInterval_); //间隔
            _rangeEasingObj.resetByCenterAndRange (
                centerValue_, //中间值
                valueRange_, //范围值
                ease_, //缓动
                minDis_, //最小成立间隔
                needNegative_ //是否需要计算负向的值列表
            );
            resetDirection (centerValue_); //朝向
        }

        public override void Dispose () {
            base.Dispose ();
        }

        public override void updateF () {
            base.updateF ();
        }

        public override void emit () {
            base.emit ();
        }

        public override void resetDirection (float direction_) {
            _rangeEasingObj.resetCenterValue (direction_);
            direction = direction_;
        }
    }
}