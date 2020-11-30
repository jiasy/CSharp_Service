using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Objs {
    //范围内等分
    public class RangeEasingCutterObj : RangerObj {
        //当前范围内的值分布
        public float[] positiveValuesInRange = null;
        public float[] negativeValuesInRange = null;

        private List<float> _caculateUseFloatList = new List<float> ();

        private bool _isInit = false;

        private int _faceTo;
        private bool _needNegative = false;

        public RangeEasingCutterObj () : base () {

        }

        //通过 中值 范围 初始化 
        public void resetByCenterAndRange (
            float centerValue_, //中间值
            float range_, //范围值
            float easing_, //缓动朝向
            float minDis_, //最小成立间隔
            bool needNegative_ = false //是否需要计算负向的值列表      
        ) {
            reset (
                centerValue_ - range_ * 0.5f,
                centerValue_ + range_ * 0.5f,
                easing_,
                minDis_,
                needNegative_
            );
        }

        public void reset (
            float begin_, //最小值
            float end_, //最大值
            float easing_, //缓动朝向
            float minDis_, //最小成立间隔
            bool needNegative_ = false //是否需要计算负向的值列表
        ) {
            _isInit = true;
            _needNegative = needNegative_;
            //初始化
            resetAsEasing (begin_, end_, begin_, 1, easing_, minDis_);
            recaculateValuesInRange ();
        }

        public void resetCenterValue (float centerValue_) {
            float _range = (end - begin); //当前范围
            float _currentCenter = begin + (end - begin) * 0.5f; //当前的中值
            if (Mathf.Abs (_currentCenter - centerValue_) < 0.01f) {
                return;
            }
            //其他值按照现有，进行重置
            resetByCenterAndRange (
                centerValue_,
                _range,
                _easing,
                _minDis,
                _needNegative
            );
        }

        public void recaculateValuesInRange () {
            current = begin; //将当前值放置到起点
            face = 1; //朝向为正
            int _count = 0;

            while (true) { //从当前第一个点开始，一直到最后达到边界时的所有值，都记录起来
                _caculateUseFloatList.Add (current);
                _count++;
                if (next ()) {
                    break;
                }
            }

            //根据当前的值，创建正负向列表
            int _length = _caculateUseFloatList.Count;
            //初始化正负向长度
            positiveValuesInRange = new float[_length];
            if (_needNegative) {
                negativeValuesInRange = new float[_length];
            }
            //获取同获取负向的值
            for (int _idx = 0; _idx < _length; _idx++) {
                float _v2 = _caculateUseFloatList[_idx];
                positiveValuesInRange[_idx] = _v2;
                if (_needNegative) {
                    negativeValuesInRange[_length - _idx - 1] = _v2;
                }
            }

            _caculateUseFloatList.Clear ();
        }
    }
}