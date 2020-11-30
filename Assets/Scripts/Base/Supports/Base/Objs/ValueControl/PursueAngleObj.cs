using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Objs {
    //追赶角度，因为角度有个超过360的过程，需要按照当前的角度对比，向适当的方向旋转
    public class PursueAngleObj : PursueObj {

        public PursueAngleObj () : base () {

        }
        public void resetAsInterval (
            float currentRotation_, //当前转向
            float angleSpeed_ = 2f
        ) {
            while (currentRotation_ < -180f) {
                currentRotation_ += 360f;
            }
            while (currentRotation_ > 180f) {
                currentRotation_ -= 360f;
            }
            resetAsInterval (
                float.MaxValue, //临时目标点
                currentRotation_,
                angleSpeed_
            );
        }

        public void resetAsEasing (
            float currentRotation_, //当前转向
            float angleXS_ = 0.05f
        ) {
            while (currentRotation_ < -180f) {
                currentRotation_ += 360f;
            }
            while (currentRotation_ > 180f) {
                currentRotation_ -= 360f;
            }
            resetAsEasing (
                float.MaxValue, //临时目标点
                currentRotation_,
                angleXS_
            );
        }
        public override void Dispose () {
            base.Dispose ();
        }

        public bool nextPursue (float targetRotation_) {
            if (!isInited) {
                Debug.LogError ("ERROR " + System.Reflection.MethodBase.GetCurrentMethod ().ReflectedType.FullName + " -> " + new System.Diagnostics.StackTrace ().GetFrame (0).GetMethod ().Name + " : " +
                    "没有初始化，无法使用"
                );
            }
            float _targetRotation; //调整追寻角度
            if (pursueType == PursueType.Easing) {
                _targetRotation = targetRotation_;
                while ((_targetRotation - currentValue) > 180f) {
                    _targetRotation -= 360f;
                }
                while ((_targetRotation - currentValue) < -180f) {
                    _targetRotation += 360f;
                }
                resetTargetValue (_targetRotation); //重新设置目标
            } else if (pursueType == PursueType.Interval) {
                if (Mathf.Abs (currentValue - targetRotation_) <= 180f) {
                    _targetRotation = targetRotation_;
                } else if (currentValue > targetRotation_) {
                    _targetRotation = targetRotation_ + 360f;
                } else {
                    _targetRotation = targetRotation_ - 360f;
                }
                resetTargetValue (_targetRotation); //重新设置目标
            }

            if (next ()) { //然后追一下
                return true;
            }
            return false;
        }

    }
}