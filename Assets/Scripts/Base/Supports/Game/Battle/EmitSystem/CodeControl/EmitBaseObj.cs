using System;
using System.Collections;
using System.Collections.Generic;
using Objs;
using UnityEngine;

namespace Game {
    //发射系统的基类，单个朝向，相对偏移位置。
    public class EmitBaseObj : BaseObj, IUpdateAbleObj {
        protected CountObj emitIntervalCount = null;
        protected bool _canEmit = false; //可否发射
        public bool isAutoEmit = false; //是否自动触发

        public float direction = 0f; //当前方向
        protected float _lastDirection = 0f; //当前方向
        public Vector2 offsetV2 = Vector2.zero; //偏移坐标
        public int emitCount = 0;

        public EmitBaseObj () : base () {

        }

        //间隔 + 朝向
        public void resetInterval (int frameInterval_, bool fireRightNow_ = false) {
            if (emitIntervalCount == null) {
                emitIntervalCount = new CountObj (frameInterval_);
            } else {
                emitIntervalCount.reset (frameInterval_);
            }
            if (fireRightNow_) { //进行一次next(),立刻就能发射
                emitIntervalCount.toMaxCount ();
            } else {
                emitIntervalCount.toEmptyCount ();
            }
        }
        public virtual void resetDirection (float direction_) {
            direction = direction_;
        }

        public override void Dispose () {
            base.Dispose ();
        }

        public virtual void updateF () {
            if (emitIntervalCount.countUp ()) { //倒计时
                _canEmit = true;
            } else {
                _canEmit = false;
            }
            //自动的情况下，就自动发射 
            if (isAutoEmit && _canEmit) {
                emit (); //根据自己的坐标偏移
            }
        }

        public virtual void emit () { //发射时，重置倒计时
            emitIntervalCount.toEmptyCount ();
            _canEmit = false;
            emitCount++; //发射次数
        }
    }
}