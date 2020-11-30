using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Objs {
    //确定的速度来追赶，带动角度旋转
    public class PursueInertanceV2Obj : BaseObj {
        public Vector2 currentV2;
        private float angleSpeed;
        private float speedValue;
        private Vector2 speed = Vector2.zero;
        public float currentRotation = 0f; //当前角度
        private bool isInited = false;
        public PursueInertanceV2Obj () : base () {

        }
        public void reset (
            Vector2 currentV2_, //当前位置
            float currentRotation_, //当前转向
            float speedValue_ = 5f, //移动速度
            float angleSpeed_ = float.MaxValue //转向速度
        ) {
            currentV2 = currentV2_;
            currentRotation = currentRotation_;
            if (speedValue_ <= 0f) {
                Debug.LogError ("ERROR " + System.Reflection.MethodBase.GetCurrentMethod ().ReflectedType.FullName + " -> " + new System.Diagnostics.StackTrace ().GetFrame (0).GetMethod ().Name + " : " +
                    "速度 一定要大于零"
                );
            }
            if (angleSpeed_ <= 0f) {
                Debug.LogError ("ERROR " + System.Reflection.MethodBase.GetCurrentMethod ().ReflectedType.FullName + " -> " + new System.Diagnostics.StackTrace ().GetFrame (0).GetMethod ().Name + " : " +
                    "转向角度 一定要大于零"
                );
            }
            speedValue = speedValue_;

            //如果不填写角速度，那么，他就是速度的十分之一
            if (angleSpeed_ < float.MaxValue) {
                angleSpeed = angleSpeed_;
            } else {
                angleSpeed = speedValue * 0.1f;
            }
            isInited = true;
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
            //get distance between follower and target
            Vector2 _dis = targetV2_ - currentV2;
            //get total distance as one number
            float distanceTotal = _dis.magnitude;
            if (distanceTotal == 0f) {
                return;
            }
            //calculate how much to move
            float moveDistanceX = angleSpeed * _dis.x / distanceTotal;
            float moveDistanceY = angleSpeed * _dis.y / distanceTotal;
            //increase current speed
            speed.x += moveDistanceX;
            speed.y += moveDistanceY;
            //get total move distance
            float totalMove = speed.magnitude;
            //apply easing
            speed.x = speedValue * speed.x / totalMove;
            speed.y = speedValue * speed.y / totalMove;
            //move follower
            currentV2 += speed;
            // if (float.IsNaN (currentV2.x) || float.IsNaN (currentV2.y)) {
            //     Debug.LogError ("ERROR " + System.Reflection.MethodBase.GetCurrentMethod ().ReflectedType.FullName + " -> " + new System.Diagnostics.StackTrace ().GetFrame (0).GetMethod ().Name + " : " +
            //         "出现 NaN 错误"
            //     );
            // }
            //rotate follower toward target
            currentRotation = LineUtils.getAngle2D (speed);
        }

        public void nextPursue () {
            if (speed == Vector2.zero) { //没有算过，就按照当前的角度和速度给算出来一个
                float _radians = CircleUtils.DegreetoRadians (currentRotation);
                speed = new Vector2 (speedValue * CircleUtils.cosPre (_radians), speedValue * CircleUtils.sinPre (_radians));
            }
            //曾经有过的moveV2，就沿用
            currentV2 += speed;
        }

    }
}