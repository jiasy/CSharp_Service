using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Objs {
    //带旋转值追赶位置
    public class PursueAngleV2Obj : BaseObj {
        public PursueAngleObj _pursueAngle = null;
        public Vector2 currentV2;
        private Vector2 speed = Vector2.zero;
        public float currentRotation = 0f; //当前角度
        private bool isInited = false;
        private float speedValue;
        private bool isReverseRotation = false; //翻转角度,也就是远离

        public PursueAngleV2Obj () : base () {

        }
        public void resetAsInterval (
            Vector2 currentV2_, //当前位置
            float currentRotation_, //当前转向
            float speedValue_ = 5f, //移动速度
            float angleSpeed_ = 2f, //转向速度
            bool isReverseRotation_ = false // 是否是远离
        ) {
            currentV2 = currentV2_;

            if (speedValue_ <= 0f) {
                Debug.LogError ("ERROR " + System.Reflection.MethodBase.GetCurrentMethod ().ReflectedType.FullName + " -> " + new System.Diagnostics.StackTrace ().GetFrame (0).GetMethod ().Name + " : " +
                    "速度 一定要大于零"
                );
            }
            speedValue = speedValue_;
            isInited = true;
            if (_pursueAngle == null) {
                _pursueAngle = new PursueAngleObj ();
            }
            _pursueAngle.resetAsInterval (
                currentRotation_,
                angleSpeed_
            );
            //获取折算后的当前角度作为角度值
            currentRotation = _pursueAngle.currentValue;
            //是行动上远离，但是发射时的角度还是正常的，在运动中体现远离
            isReverseRotation = isReverseRotation_;
        }
        public void resetAsEasing (
            Vector2 currentV2_, //当前位置
            float currentRotation_, //当前转向
            float speedValue_ = 5f, //移动速度
            float angleXS_ = 0.5f, //转向系数
            bool isReverseRotation_ = false // 是否是远离
        ) {
            currentV2 = currentV2_;
            if (speedValue_ <= 0f) {
                Debug.LogError ("ERROR " + System.Reflection.MethodBase.GetCurrentMethod ().ReflectedType.FullName + " -> " + new System.Diagnostics.StackTrace ().GetFrame (0).GetMethod ().Name + " : " +
                    "速度 一定要大于零"
                );
            }
            speedValue = speedValue_;
            isInited = true;
            if (_pursueAngle == null) {
                _pursueAngle = new PursueAngleObj ();
            }
            _pursueAngle.resetAsEasing (
                currentRotation_,
                angleXS_
            );
            //获取折算后的当前角度作为角度值
            currentRotation = _pursueAngle.currentValue;
            //是行动上远离，但是发射时的角度还是正常的，在运动中体现远离
            isReverseRotation = isReverseRotation_;
        }
        public override void Dispose () {
            base.Dispose ();
        }

        //永远在追，每次都重新设置目标点，哪怕坐标点和上次一致
        public void nextPursue (Vector2 targetV2_) {
            if (!isInited) {
                Debug.LogError ("ERROR " + System.Reflection.MethodBase.GetCurrentMethod ().ReflectedType.FullName + " -> " + new System.Diagnostics.StackTrace ().GetFrame (0).GetMethod ().Name + " : " +
                    "没有初始化，无法使用"
                );
            }
            //且跟踪旋转的量不是零;
            float angleTemp;
            if (isReverseRotation) {
                angleTemp = LineUtils.getAngle2D (targetV2_, currentV2);
            } else {
                angleTemp = LineUtils.getAngle2D (currentV2, targetV2_);
            }
            _pursueAngle.nextPursue (angleTemp); //追赶一下当前的角度
            //if (_pursueAngle.pursuedCount < 2) { //角度没追上的时候，重新算速度
            //小于2是因为，一开始设置成角度和目标一致，也需要算一次速度
            currentRotation = _pursueAngle.currentValue; //获取当前的角度
            float _radians = CircleUtils.DegreetoRadians (currentRotation);
            speed.x = speedValue * CircleUtils.cosPre (_radians);
            speed.y = speedValue * CircleUtils.sinPre (_radians);
            //}
            currentV2 += speed;
        }

        public void nextPursue () {
            if (speed == Vector2.zero) { //没有算过，就按照当前的角度和速度给算出来一个
                float _radians = CircleUtils.DegreetoRadians (currentRotation);
                if (isReverseRotation) { //反向180度就是远离
                    _radians = _radians + Mathf.PI;
                }
                speed = new Vector2 (speedValue * CircleUtils.cosPre (_radians), speedValue * CircleUtils.sinPre (_radians));
            }
            //曾经有过的moveV2，就沿用
            currentV2 += speed;
        }

    }
}