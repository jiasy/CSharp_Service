using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Objs {
    //范围内等分
    public class RangeAverageCutterObj : RangerObj {
        private float _valueRange; //值摆动范围

        private int _num; //个数

        private float _centerValue; //中间值

        //当前范围内的值分布
        public float[] valuesInRange = null;
        private bool _isInit = false;
        public static void doSample () {
            //将2PI切 _cutTime 份，算上两端边缘值有 _cutTime+1 项
            int _cutTime = 720;
            float[] _valueList = cut2PI (_cutTime);
            
        }
        public static float[] cut2PI (int cutNumber_) {
            RangeAverageCutterObj _cutter = new RangeAverageCutterObj ();
            float _targetValue = Mathf.PI * 2;
            _cutter.resetAverage (0, _targetValue, cutNumber_ + 1); //将2PI分成720份。两边边缘都取，所以是721份
            return _cutter.valuesInRange;
        }

        public RangeAverageCutterObj () : base () {
            
        }
        //平均值
        public void resetAverage (float centerValue_, float valueRange_, int num_) {
            _centerValue = centerValue_;
            _valueRange = valueRange_; //角度范围
            _num = num_; //算上两边一共有几个。
            valuesInRange = new float[_num];
            float _begin = _centerValue - _valueRange * 0.5f;
            resetAsInterval (
                _begin, //起始值
                _centerValue + _valueRange * 0.5f, //终止值
                _begin, //当前值
                1, //朝向
                _valueRange / (float) (_num - 1) //间隔多少来一，分n份儿，其中点家两边是有n+1个点，所以，算上两边 有 num 个，就分 (num - 1) 份
            );
            recaculateValuesInRange ();
            _isInit = true;
        }
        //重置中点値
        public void resetCenterValue (float centerValue_) {
            if (!_isInit) {
                Debug.LogError ("ERROR " + System.Reflection.MethodBase.GetCurrentMethod ().ReflectedType.FullName + " -> " + new System.Diagnostics.StackTrace ().GetFrame (0).GetMethod ().Name + " : " +
                    "没有初始化过请先调用 resetAverage 方法"
                );
            }
            float _dis = centerValue_ - _centerValue;
            if (Mathf.Abs (_dis) < 0.1f) {
                return;
            }
            _centerValue = centerValue_;
            reset (
                _centerValue - _valueRange * 0.5f, //起始值
                _centerValue + _valueRange * 0.5f, //终止值
                current + _dis //偏移值
            );
            recaculateValuesInRange ();
        }

        //重置范围
        public void resetValueRange (float valueRange_) {
            if (!_isInit) {
                Debug.LogError ("ERROR " + System.Reflection.MethodBase.GetCurrentMethod ().ReflectedType.FullName + " -> " + new System.Diagnostics.StackTrace ().GetFrame (0).GetMethod ().Name + " : " +
                    "没有初始化过请先调用 resetAverage 方法"
                );
            }
            if (Mathf.Abs (valueRange_ - _valueRange) < 0.1f) {
                return;
            }
            float _pect = valueRange_ / _valueRange; //伸缩百分比
            _valueRange = valueRange_;
            reset (
                _centerValue - _valueRange * 0.5f, //起始值
                _centerValue + _valueRange * 0.5f, //终止值
                (current - _centerValue) * _pect + _centerValue //距离终点的距离，乘以百分比，就是伸缩后距离终点的距离。在叠加回中点
            );
            recaculateValuesInRange ();
        }

        //重置个数
        public void resetNum (int num_) {
            if (!_isInit) {
                Debug.LogError ("ERROR " + System.Reflection.MethodBase.GetCurrentMethod ().ReflectedType.FullName + " -> " + new System.Diagnostics.StackTrace ().GetFrame (0).GetMethod ().Name + " : " +
                    "没有初始化过请先调用 resetAverage 方法"
                );
            }
            if (_num == num_) {
                return;
            }
            _num = num_;
            float _begin = _centerValue - _valueRange * 0.5f;
            resetAsInterval (
                _begin, //起始值
                _centerValue + _valueRange * 0.5f, //终止值
                _begin, //当前值
                1, //朝向
                _valueRange / (float) (_num - 1) //间隔多少来一，分n份儿，其中点家两边是有n+1个点，所以，算上两边 有 num 个，就分 (num - 1) 份
            );
            recaculateValuesInRange ();
        }
        public void recaculateValuesInRange () {
            current = begin; //将当前值放置到起点
            face = 1; //朝向为正
            int _count = 0;
            while (true) { //从当前第一个点开始，一直到最后达到边界时的所有值，都记录起来
                valuesInRange[_count] = current;
                _count++;
                if (next ()) {
                    if (_count != _num) { //如果算到最后的时候，当前的序号不是最后一个，这就证明最后一个在当前的判断条件之外
                        //手动把最后一个值补上。
                        valuesInRange[_count] = end;
                    }
                    break;
                }
            }
        }
    }
}