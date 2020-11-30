using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Objs {
    //值追赶
    public class PursueV2Obj : BaseObj {
        private Vector2 targetV2; //目标值会经常修改，修改的同时会修改朝向
        public Vector2 currentV2; //一般不会修改当前值。
        private float speedValue;
        private Vector2 speedV2;
        private bool isInited = false;
        private bool isPursued = false;
        private float lastRadians = float.MaxValue;
        private bool lastFrameAlreadyInSpeedRange = false; //是否已经进入速度范围内
        public float direction = 0f;
        private bool _isJustSetTargetValue = false; //在同一帧内设置目标，然后又调用的话。
        private float _currentFrameDisSqrMagnitude = 0f;

        public PursueV2Obj () : base () {

        }
        public void reset (
            Vector2 targetV2_, //当前
            Vector2 currentV2_, //目标
            float speedValue_ //速度
        ) {
            isInited = true;
            currentV2 = currentV2_;
            if (speedValue_ <= 0f) {
                Debug.LogError ("ERROR " + System.Reflection.MethodBase.GetCurrentMethod ().ReflectedType.FullName + " -> " + new System.Diagnostics.StackTrace ().GetFrame (0).GetMethod ().Name + " : " +
                    "速度一定要大于零"
                );
            }
            speedValue = speedValue_;
            resetTargetValue (targetV2_);

            isPursued = false;
        }
        public void resetTargetValue (Vector2 targetV2_) {
            if (!isInited) {
                Debug.LogError ("ERROR " + System.Reflection.MethodBase.GetCurrentMethod ().ReflectedType.FullName + " -> " + new System.Diagnostics.StackTrace ().GetFrame (0).GetMethod ().Name + " : " +
                    "没有初始化，无法使用"
                );
            }
            //变更目标值是常态
            targetV2 = targetV2_;
            Vector2 _dis = targetV2 - currentV2;
            if (_dis.sqrMagnitude > 0f) { //有距离 
                float _currentRadians = Mathf.Atan2 (_dis.y, _dis.x); //获取当前弧度 -180 到 180 之间的 弧度
                if (Mathf.Abs (_currentRadians - lastRadians) < 0.0087f) { //3.14 为180 度，360 度 就是 6.24，0.5度认为没变化的话，那么就是1/720。6.24的720分之一为 0.0087
                    return;
                }
                speedV2 = new Vector2 (speedValue * CircleUtils.cosPre (_currentRadians), speedValue * CircleUtils.sinPre (_currentRadians));
                lastRadians = _currentRadians;
                direction = CircleUtils.RadianstoDegrees (_currentRadians);
                lastFrameAlreadyInSpeedRange = false;
                isPursued = false;
            } else {
                direction = 0f;
                lastRadians = 0f;
                lastFrameAlreadyInSpeedRange = true;
                isPursued = true;
            }
        }
        public void resetSpeedValue (float speedValue_) {
            if (!isInited) {
                Debug.LogError ("ERROR " + System.Reflection.MethodBase.GetCurrentMethod ().ReflectedType.FullName + " -> " + new System.Diagnostics.StackTrace ().GetFrame (0).GetMethod ().Name + " : " +
                    "没有初始化，无法使用"
                );
            }
            if (isPursued) {
                Debug.LogError ("ERROR " + System.Reflection.MethodBase.GetCurrentMethod ().ReflectedType.FullName + " -> " + new System.Diagnostics.StackTrace ().GetFrame (0).GetMethod ().Name + " : " +
                    "追上了，改变速度没意义，直接重置吧"
                );
            }
            speedValue = speedValue_; //利用当前存的角度，重新算速度，角度只能在重新设置终点的时候设置，所以，终点不变，变更速度，可以直接用上一帧的来算。
            speedV2 = new Vector2 (speedValue * CircleUtils.cosPre (lastRadians), speedValue * CircleUtils.sinPre (lastRadians));
            lastFrameAlreadyInSpeedRange = false; //速度变化了。那么上一帧在范围内就要重新算了
        }
        public override void Dispose () {
            base.Dispose ();
        }

        public bool next () {
            if (!isInited) {
                Debug.LogError ("ERROR " + System.Reflection.MethodBase.GetCurrentMethod ().ReflectedType.FullName + " -> " + new System.Diagnostics.StackTrace ().GetFrame (0).GetMethod ().Name + " : " +
                    "没有初始化，无法使用"
                );
            }

            if (isPursued) { //追赶上之后，就一直是成功追上。
                _isJustSetTargetValue = false;
                return true;
            }
            if (lastFrameAlreadyInSpeedRange) { //上一帧，已经完全进入了速度范围，那么这一帧，就直接认为它接触上了
                //因为本帧，还会继续以相同的速度接近，上一帧就已经在范围内了，本帧只能沿着这个方向跨越终点
                _isJustSetTargetValue = false;
                currentV2 = targetV2;
                isPursued = true;
                return true;
            }

            currentV2 += speedV2;
            float _sqlM;
            if (_isJustSetTargetValue) { //同一帧，既赋值目标，又调用推进的时候，用刚才存下来的值，不重新计算
                _sqlM = _currentFrameDisSqrMagnitude;
            } else {
                _sqlM = (currentV2 - targetV2).sqrMagnitude;
            }

            if (_sqlM < 1f) { //平方距离小于1，就认为已经到位了。和目标的距离特别小
                _isJustSetTargetValue = false;
                currentV2 = targetV2;
                isPursued = true;
                return true;
            } else { //没到位的话
                if (_sqlM <= speedValue * speedValue) { //平方距离小于速度平方。不是特别小，但是，很可能是跨越了终点 
                    lastFrameAlreadyInSpeedRange = true;
                }
            }
            _isJustSetTargetValue = false;
            return false;
        }
    }
}