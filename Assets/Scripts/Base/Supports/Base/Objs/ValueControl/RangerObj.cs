using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Objs {
    //范围之内的摇摆值
    //同等帧间隔，就是摇摆
    //同一帧内，起点到终点，就是分割
    public class RangerObj : BaseObj {
        public float begin;
        public float end;

        public float current;
        public int face;
        private float _interval;
        protected float _easing;
        private MoveType moveType;
        protected float _minDis;
        private bool _isRangerInited = false;
        private enum MoveType {
            Interval,
            Easing
        }
        public static void doSample () {
            doSampleSub1 ();

            //不停的调整范围，比如指向一个方向，在方向上有个角度范围。在角度范围内进行扫射。
            doSampleSub2 ();
        }
        public static void doSampleSub1 () {
            //等分距离。记录起止两点------------------------------------------------------------------------
            float _begin = -30;
            float _interval = 1;
            RangerObj _rangerObj = new RangerObj ();
            _rangerObj.resetAsInterval (
                _begin, //起始值
                30, //终止值
                _begin, //当前值
                1, //朝向
                _interval //间隔多少来一个
            );
            int _countInRange = _rangerObj.getRangeCount ();
            //个数 + 1，是为了起始值也算值
            float[] _valueList = new float[_countInRange + 1];
            int _count = 0;
            //从当前第一个点开始，一直到最后达到边界时的所有值，都记录起来
            while (true) {
                _valueList[_count] = _rangerObj.current;
                if (_rangerObj.next ()) {
                    _count++;
                    _valueList[_count] = _rangerObj.current;
                    break;
                }
                _count++;
            }
        }

        public static void doSampleSub2 () {
            float _direction = 45;
            float _range = 60;
            float _interval = 1;
            RangerObj _rangerObj = new RangerObj ();
            _rangerObj.resetAsInterval (
                _direction - _range * 0.5f, //起始值
                _direction + _range * 0.5f, //终止值
                _direction, //当前值
                1, //朝向
                _interval //间隔多少来一个
            );
            /*
                updateF 中
                    _rangerObj.current // 作为当前帧的值
                    if(_rangerObj.next()){// 是否达到边界的判断
                        if(_rangerObj.face==1){//刚才是 face == -1，到界限

                        }else if(_rangerObj.face==-1){//刚才是 face == 1，到界限

                        }
                        //达到边界如何
                    } 
                指向变化
                    float _disValue = targetValue_ - _direction; 
                     _rangerObj.reset(
                         targetValue_ - _range * 0.5f, //起始值
                         targetValue_ + _range * 0.5f, //终止值
                         _rangerObj.current + _disValue
                     )
            */

        }

        public RangerObj () : base () {

        }
        public int getRangeCount () {
            if (moveType != MoveType.Interval) {
                Debug.LogError ("ERROR " + System.Reflection.MethodBase.GetCurrentMethod ().ReflectedType.FullName + " -> " + new System.Diagnostics.StackTrace ().GetFrame (0).GetMethod ().Name + " : " +
                    "不是 Interval 类型的，无法取得等距分割的点个数"
                );
            }
            return (int) ((end - begin) / _interval);
        }
        public void resetAsInterval (
            float begin_, //最小值
            float end_, //最大值
            float current_, //当前值
            float face_, //值变化朝向
            float interval_ = 0f //等距间隔
        ) {
            _isRangerInited = true;
            reset (begin_, end_, current_);
            resetFace (face_);
            _interval = interval_;
            moveType = MoveType.Interval;
        }
        public void resetAsEasing (
            float begin_, //最小值
            float end_, //最大值
            float current_, //当前值
            float face_, //值变化朝向
            float easing_, //缓动朝向
            float minDis_ = 0.1f //缓动过后，距离目标位置小于多少，认为达到目标
        ) {
            _isRangerInited = true;
            reset (begin_, end_, current_);
            resetFace (face_);
            _easing = easing_;
            _minDis = minDis_;
            moveType = MoveType.Easing;
        }

        public void resetFace (float face_) { //定朝向
            if (!_isRangerInited) {
                Debug.LogError ("ERROR " + System.Reflection.MethodBase.GetCurrentMethod ().ReflectedType.FullName + " -> " + new System.Diagnostics.StackTrace ().GetFrame (0).GetMethod ().Name + " : " +
                    "请先调用 resetAsInterval / resetAsEasing 初始化"
                );
            }
            if (Mathf.Abs (face_) < 1f) {
                Debug.LogError ("ERROR " + System.Reflection.MethodBase.GetCurrentMethod ().ReflectedType.FullName + " -> " + new System.Diagnostics.StackTrace ().GetFrame (0).GetMethod ().Name + " : " +
                    "朝向值 face_ 必须绝对值大于1"
                );
            }
            if (face_ > 0f) {
                face = 1;
            } else if (face_ < 0f) {
                face = -1;
            }
        }

        //这些值，可能是经常变更的。
        public void reset (float begin_, float end_, float current_) {
            if (!_isRangerInited) {
                Debug.LogError ("ERROR " + System.Reflection.MethodBase.GetCurrentMethod ().ReflectedType.FullName + " -> " + new System.Diagnostics.StackTrace ().GetFrame (0).GetMethod ().Name + " : " +
                    "请先调用 resetAsInterval / resetAsEasing 初始化"
                );
            }
            begin = begin_;
            end = end_;
            if (begin > end) {
                Debug.LogError ("ERROR " + System.Reflection.MethodBase.GetCurrentMethod ().ReflectedType.FullName + " -> " + new System.Diagnostics.StackTrace ().GetFrame (0).GetMethod ().Name + " : " +
                    "起始值 begin_ 大 于终止值 end_ ，请调整参数，颠倒两个值，再调整 face_ 正负"
                );
            }
            current = current_;
        }

        public override void Dispose () {
            base.Dispose ();
        }
        public bool next () {
            if (moveType == MoveType.Easing) {
                return easingCurrent ();
            } else if (moveType == MoveType.Interval) {
                return intervalCurrent ();
            } else {
                Debug.LogError ("ERROR " + System.Reflection.MethodBase.GetCurrentMethod ().ReflectedType.FullName + " -> " + new System.Diagnostics.StackTrace ().GetFrame (0).GetMethod ().Name + " : " +
                    "不支持的类型"
                );
                return false;
            }
        }

        //按照当前的朝向移动
        public bool intervalCurrent () {
            current += face * _interval;
            if (face == 1) {
                if (current > end) {
                    current = end;
                    face *= -1; //转向
                    return true;
                }
            } else if (face == -1) {
                if (current < begin) {
                    current = begin;
                    face *= -1; //转向
                    return true;
                }
            }
            return false;
        }
        //缓动当前值
        public bool easingCurrent () {
            float _valueTarget = current;
            if (face == 1) {
                _valueTarget = end;
            } else if (face == -1) {
                _valueTarget = begin;
            }
            current += (_valueTarget - current) * _easing;
            if (Mathf.Abs (_valueTarget - current) < _minDis) { //距离小于设定最小值，认为它到位了
                current = _valueTarget; //到位就同步位置 
                return true;
            }
            return false;
        }
    }
}