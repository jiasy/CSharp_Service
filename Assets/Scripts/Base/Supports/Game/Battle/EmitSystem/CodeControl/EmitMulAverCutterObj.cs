using System;
using System.Collections;
using System.Collections.Generic;
using Objs;
using UnityEngine;

namespace Game {
    //单个摆动
    public class EmitMulAverCutterObj : EmitBaseObj {
        protected RangeAverageCutterObj _rangeAverObj = new RangeAverageCutterObj ();

        public EmitMulAverCutterObj () : base () {

        }
        public void reset (int frameInterval_, float centerValue_, float valueRange_, int num_) {
            resetInterval (frameInterval_); //间隔
            _rangeAverObj.resetAverage(centerValue_, valueRange_, num_);
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
            _rangeAverObj.resetCenterValue(direction_);
            direction = direction_;
        }
    }
}