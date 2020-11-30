using System;
using System.Collections;
using System.Collections.Generic;
using Objs;
using UnityEngine;

namespace Game {
    //单个摆动
    public class EmitRotateAverObj : EmitBaseObj {
        private RangerObj _rangerObj = new RangerObj ();
        private float _rangeAngle; //摆动角度范围
        public float faceDirection; //当前朝向

        public EmitRotateAverObj () : base () {

        }

        public void reset (
            int frameInterval_, //帧间隔
            float faceDirection_, //朝向
            float rangeAngle_, //角度范围
            float rotateSpeed_, //旋转速度
            int face_ //当前旋转朝向
        ) {
            if (Mathf.Abs (face_) < 1f) {
                Debug.LogError ("ERROR " + System.Reflection.MethodBase.GetCurrentMethod ().ReflectedType.FullName + " -> " + new System.Diagnostics.StackTrace ().GetFrame (0).GetMethod ().Name + " : " +
                    "朝向值 face_ 必须绝对值大于1"
                );
            }

            if (rangeAngle_ < 1f) {
                Debug.LogError ("ERROR " + System.Reflection.MethodBase.GetCurrentMethod ().ReflectedType.FullName + " -> " + new System.Diagnostics.StackTrace ().GetFrame (0).GetMethod ().Name + " : " +
                    "角度范围 rangeAngle_ 必须大于零，且不能太小"
                );
            }

            resetInterval (frameInterval_); //间隔

            faceDirection = faceDirection_; //朝向

            _rangeAngle = rangeAngle_; //摆动角度范围

            _rangerObj.resetAsInterval (
                faceDirection_ - _rangeAngle * 0.5f, //起始值
                faceDirection_ + _rangeAngle * 0.5f, //终止值
                faceDirection_, //当前值
                face_, //朝向
                rotateSpeed_ //间隔多少来一个
            );

            //计算当前应当放在那一边，从边缘开始转动扫射
            if (_rangerObj.face > 0) {
                _rangerObj.current = _rangerObj.begin;
            } else if (_rangerObj.face < 0) {
                _rangerObj.current = _rangerObj.end;
            }

            //设置当前的角度值
            resetDirection (_rangerObj.current);

            //重置之后，应当，立刻就能发射
            emitIntervalCount.toMaxCount ();
        }

        public void resetFaceDirection (float faceDirection_) {
            float _disAngle = faceDirection_ - faceDirection; //目标减去现有，就是变化
            faceDirection = faceDirection_; //指向角变化

            _rangerObj.reset (
                faceDirection_ - _rangeAngle * 0.5f, //起始值
                faceDirection_ + _rangeAngle * 0.5f, //终止值
                _rangerObj.current + _disAngle //当前值也跟着偏移
            );

            resetDirection (_rangerObj.current); //同步朝向
        }

        public override void Dispose () {
            _rangerObj.Dispose ();
            base.Dispose ();
        }

        public override void updateF () {
            resetDirection (_rangerObj.current); //同步当前值
            _rangerObj.next (); //指向下一个值
            base.updateF ();
        }
    }
}