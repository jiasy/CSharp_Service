using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Objs {
    //沿着某个角度进行运动，Fixed 不受任何力影响
    public class FixedAngleBaseObj : MoveControlObj {
        private float _direction; //方向 
        private Vector2 _acc = Vector2.zero; //加速度
        public Vector2 speed = Vector2.zero; //速度
        private Vector2 _speedMax = Vector2.zero; //速度最大值，最小值就是0.
        private bool _isSpeedLimited = false;
        public bool isAccEnd = false; //加速是否借宿
        public SpeedChangeType accType; //当前类型 
        public enum SpeedChangeType {
            Negative_ACC, //负向加速度
            None_ACC, //没有加速度
            Positive_ACC //有加速度
        }

        public FixedAngleBaseObj (PosType posType_) : base (posType_) {

        }

        public void reset (Vector2 realPos_, float direction_, float speed_, float acc_ = float.Epsilon, float speedMax_ = float.MaxValue) {
            resetRealPos(realPos_);//重置MC
            updatePos (realPos.x, realPos.y); //换成显示位置
            _direction = direction_;

            float _radians = CircleUtils.DegreetoRadians (_direction);
            float _cos = Mathf.Cos (_radians);
            float _sin = Mathf.Sin (_radians);

            //有加速度
            if (Math.Abs (acc_) > float.Epsilon) {
                if (acc_ > 0f) {
                    accType = SpeedChangeType.Positive_ACC;
                } else if (acc_ < 0f) {
                    accType = SpeedChangeType.Negative_ACC;
                }
                _acc = new Vector2 (_cos * acc_, _sin * acc_);
                isAccEnd = false;
            } else { //没有加速度
                _acc = Vector2.zero;
                accType = SpeedChangeType.None_ACC;
                isAccEnd = true;
            }

            if (Math.Abs (speed_) > float.Epsilon) { //当前速度
                speed = new Vector2 (_cos * speed_, _sin * speed_);
            } else {
                speed = Vector2.zero;
                if (isAccEnd) {
                    Debug.LogError ("ERROR " + System.Reflection.MethodBase.GetCurrentMethod ().ReflectedType.FullName + " -> " + new System.Diagnostics.StackTrace ().GetFrame (0).GetMethod ().Name + " : " +
                        "速度 和 加速度 不能同时为零"
                    );
                }
            }

            if (accType == SpeedChangeType.Positive_ACC) { //正向移动
                if (speedMax_ < float.MaxValue) { //当前速度限制
                    _speedMax = new Vector2 (_cos * speedMax_, _sin * speedMax_);
                    _isSpeedLimited = true;
                } else {
                    _isSpeedLimited = false;
                }
            }

        }

        public override void Dispose () {
            base.Dispose ();
        }
        public override void updateF () {
            if (!isAccEnd) { //加速过程没结束
                if (accType == SpeedChangeType.Positive_ACC) { //正向加速移动
                    speed += _acc;
                    if (_isSpeedLimited) { //速度有上限
                        //速度开始是负数，逐渐加速到正值。
                        //speedMax.x 的正负，就是正值所在方向。只有在同向超越，才能是超越
                        if (
                            speed.x > 0 && _speedMax.x > 0 && speed.x > _speedMax.x ||
                            speed.x < 0 && _speedMax.x < 0 && speed.x < _speedMax.x
                        ) { //x,y都是等量增加，所以一个方向上达标，另外一个方向也达标
                            speed = _speedMax;
                            isAccEnd = true;
                        }
                    }
                } else if (accType == SpeedChangeType.Positive_ACC) { //负向加速过程
                    speed += _acc;
                    if (
                        speed.x > 0 && speed.x < 0f ||
                        speed.x < 0 && speed.x > 0f
                    ) { //速度达到0，加速度过程结束
                        isAccEnd = true;
                        speed = Vector2.zero;
                    }
                }
            }

            realPos.x += speed.x; //移动位置
            realPos.y += speed.y; //移动位置

            base.updateF (); //realPos -> Pos
        }
    }
}