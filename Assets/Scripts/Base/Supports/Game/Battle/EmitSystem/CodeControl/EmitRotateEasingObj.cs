using System;
using System.Collections;
using System.Collections.Generic;
using Objs;
using UnityEngine;

namespace Game {
    //单个摆动
    public class EmitRotateEasingObj : EmitBaseObj {
        private RangerObj _rangerObj = new RangerObj ();
        private float _rangeAngle; //摆动角度范围
        private float _faceDirection; //当前朝向

        public EmitRotateEasingObj () : base () {

        }
        
        
        public void reset (
            int frameInterval_, 
            float faceDirection_,
            float rangeAngle_, 
            float face_,
            float easing_ = 0.01f,
            float minDis_ = 0.1f
        ) {

            if (rangeAngle_ < 1f) {
                Debug.LogError ("ERROR " + System.Reflection.MethodBase.GetCurrentMethod ().ReflectedType.FullName + " -> " + new System.Diagnostics.StackTrace ().GetFrame (0).GetMethod ().Name + " : " +
                    "角度范围 rangeAngle_ 必须大于零，且不能太小"
                );
            }

            resetInterval (frameInterval_); //间隔

            _faceDirection = faceDirection_; //朝向

            _rangeAngle = rangeAngle_; //摆动角度范围


            _rangerObj.resetAsEasing (
                faceDirection_ - _rangeAngle * 0.5f, //起始值
                faceDirection_ + _rangeAngle * 0.5f, //终止值
                faceDirection_, //当前值
                face_,
                easing_, //缓动值
                minDis_ //近似间隔
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

        //重置朝向
        public void resetFaceDirection (float faceDirection_)
        {
            float _disAngle = faceDirection_ - _faceDirection; //目标减去现有，就是变化
            _faceDirection = faceDirection_; //指向角变化
            _rangerObj.reset (
                faceDirection_ - _rangeAngle * 0.5f, //起始值
                faceDirection_ + _rangeAngle * 0.5f, //终止值
                _faceDirection +_disAngle //当前值
            );

            resetDirection (_rangerObj.current); //同步朝向
        }

        public override void Dispose () {
            _rangerObj.Dispose();
            base.Dispose ();
        }

        public override void updateF () {
            resetDirection (_rangerObj.current);//同步当前值
            _rangerObj.next (); //指向下一个值
            base.updateF ();
        }
    }
}