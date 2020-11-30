using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Objs {
    //值追赶
    public class PursueObj : BaseObj {
        private float targetValue; //目标值会经常修改，修改的同时会修改朝向
        public float currentValue;
        private float speed;
        private int face;
        private float xs;
        protected bool isInited = false;
        private bool isPursued = false;
        public int pursuedCount = 0;
        public float minDis = 0.01f;

        protected PursueType pursueType;
        protected enum PursueType {
            Interval,
            Easing
        }
        public PursueObj () : base () {

        }
        public void resetAsInterval (float targetValue_, float currentValue_, float speed_) {
            targetValue = targetValue_;
            currentValue = currentValue_;
            if (targetValue > currentValue) {
                face = 1;
            } else if (targetValue < currentValue) {
                face = -1;
            } else {
                Debug.LogError ("ERROR " + System.Reflection.MethodBase.GetCurrentMethod ().ReflectedType.FullName + " -> " + new System.Diagnostics.StackTrace ().GetFrame (0).GetMethod ().Name + " : " +
                    "当前值 和 目标值 不可能一样大"
                );
                face = 0;
            }
            if (speed_ <= 0f) {
                Debug.LogError ("ERROR " + System.Reflection.MethodBase.GetCurrentMethod ().ReflectedType.FullName + " -> " + new System.Diagnostics.StackTrace ().GetFrame (0).GetMethod ().Name + " : " +
                    "速度一定要大于零"
                );
            }
            speed = speed_;
            isInited = true;
            isPursued = false;
            pursuedCount = 0;
            pursueType = PursueType.Interval;
        }

        public void resetAsEasing (float targetValue_, float currentValue_, float xs_) {
            targetValue = targetValue_;
            currentValue = currentValue_;

            if (xs_ <= 0f) {
                Debug.LogError ("ERROR " + System.Reflection.MethodBase.GetCurrentMethod ().ReflectedType.FullName + " -> " + new System.Diagnostics.StackTrace ().GetFrame (0).GetMethod ().Name + " : " +
                    "系数一定要大于零"
                );
            }
            if (xs_ >= 1f) {
                Debug.LogError ("ERROR " + System.Reflection.MethodBase.GetCurrentMethod ().ReflectedType.FullName + " -> " + new System.Diagnostics.StackTrace ().GetFrame (0).GetMethod ().Name + " : " +
                    "系数一定要小于1"
                );
            }
            xs = xs_;
            isInited = true;
            isPursued = false;
            pursueType = PursueType.Easing;
        }
        public void resetTargetValue (float targetValue_) {
            if (!isInited) {
                Debug.LogError ("ERROR " + System.Reflection.MethodBase.GetCurrentMethod ().ReflectedType.FullName + " -> " + new System.Diagnostics.StackTrace ().GetFrame (0).GetMethod ().Name + " : " +
                    "没有初始化，无法使用"
                );
            }
            //目标值太小，当做没有变化
            if (Mathf.Abs (targetValue - targetValue_) < minDis) {
                return;
            }
            //变更目标值是常态
            targetValue = targetValue_;
            if (pursueType == PursueType.Easing) {
                if (Mathf.Abs (targetValue - currentValue) < minDis) {
                    isPursued = true;
                } else {
                    isPursued = false;
                }
            } else if (pursueType == PursueType.Interval) {
                //变更目标值的时候，也要变更当前的追击朝向
                if (targetValue > currentValue) {
                    face = 1;
                    isPursued = false;
                } else if (targetValue < currentValue) {
                    face = -1;
                    isPursued = false;
                } else {
                    isPursued = true; //直接就碰到了终点
                }
            }
        }

        public void resetCurrentValue (float currentValue_) {
            if (!isInited) {
                Debug.LogError ("ERROR " + System.Reflection.MethodBase.GetCurrentMethod ().ReflectedType.FullName + " -> " + new System.Diagnostics.StackTrace ().GetFrame (0).GetMethod ().Name + " : " +
                    "没有初始化，无法使用"
                );
            }
            //目标值太小，当做没有变化
            if (Mathf.Abs (currentValue - currentValue_) < minDis) {
                return;
            }
            if (pursueType == PursueType.Interval) {
                if (checkPursued (currentValue_)) { //判断给定的值是否已经到达终点了。到达了就是追上了
                    currentValue = targetValue;
                    isPursued = true;
                } else {
                    currentValue = currentValue_; //设置的值没有到达终点
                    isPursued = false;
                }
            } else if (pursueType == PursueType.Easing) {
                if (Mathf.Abs (targetValue - currentValue) < minDis) {
                    currentValue = targetValue;
                    isPursued = true;
                } else {
                    currentValue = currentValue_; //设置的值没有到达终点
                    isPursued = false;
                }
            }
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
                pursuedCount++;
                return isPursued;
            }
            if (pursueType == PursueType.Interval) {
                currentValue += face * speed;
                if (checkPursued (currentValue)) {
                    currentValue = targetValue;
                    isPursued = true;
                }
            } else if (pursueType == PursueType.Easing) {
                currentValue += (targetValue - currentValue) * xs;
                if (Mathf.Abs (targetValue - currentValue) < minDis) {
                    isPursued = true;
                }
            }
            if (isPursued) {
                pursuedCount++;
            } else {
                pursuedCount = 0;
            }
            return isPursued;
        }
        private bool checkPursued (float tempValue_) {
            if (face == 1) {
                if (tempValue_ >= targetValue) {
                    return true;
                }
            } else { //face == -1
                if (tempValue_ <= targetValue) {
                    return true;
                }
            }
            return false;
        }
    }
}