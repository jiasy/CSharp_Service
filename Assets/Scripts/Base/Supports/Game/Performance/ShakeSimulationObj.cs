using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Objs {
    //模拟一个晃动，弹跳晃动
    public class ShakeSimulationObj : BaseObj {
        private int _shakeCount = 0;
        private int _shakeCountMax = 4;
        private float _shakeG = -2; //抖动的重力
        private float _shakeBounce = 0.5f; //弹跳的损耗
        private float _shakeVh = 0.0f; //当前y方向的速度
        private float _shakeHeight = 0.0f; //当前y的位置
        private float _targetR = 0; //设置的目标值
        private float _shakeR = 0; //在目标值的基础上随机80%-120%的当前抖动值
        private float _currentR = 0; //实际的旋转值
        public bool isEnd = true;
        public ShakeSimulationObj () : base () {

        }
        public override void Dispose () {
            base.Dispose ();
        }

        public void doShake (float vy_, float rotation_) {
            _shakeVh = vy_; //抖动
            _targetR = rotation_; //旋转
            _shakeCount = _shakeCountMax;
            isEnd = true;
        }

        public void updateF () {
            if (isEnd) { //结束状态下不进行任何运算
                return;
            }
            //震动
            _shakeCount--;
            //在打击可以造成状态变化的状态下
            if (_shakeCount > 0) {
                if (_shakeCount % 2 == 0) {
                    //随机一个角度
                    _shakeR = Random.Range (_targetR * 0.8f, _targetR * 1.2f);
                    //一半几率为另外一个方向
                    if (Random.value > 0.5f) {
                        _shakeR = -_shakeR;
                    }
                }
            } else {
                //Y方向，照常落下
                //旋转R也放置回原有的状态
                _shakeR = 0;
            }
            //角度趋近
            if (_currentR != _shakeR) {
                _currentR += (_shakeR - _currentR) * 0.5f;
            }
            //震动之后Y方向位置
            _shakeHeight += _shakeVh;
            //是否落地的判断
            if (_shakeHeight > 0.0f) { //空中
                _shakeVh += _shakeG;
            } else {
                if (_shakeVh < 0.0f) { //弹跳
                    _shakeVh *= -_shakeBounce;
                    if (_shakeVh < 1.0f) { //落地状态判断
                        _shakeVh = 0.0f;
                        _shakeHeight = 0.0f;
                        if (_shakeCount < 0) { //终结运算
                            isEnd = true;
                        }
                    }
                }
            }
        }
    }
}
/*
    _targetTransform.rotation = Quaternion.Euler (new Vector3 (0f, 0f, currentR));
    if (_targetTransform.position.y != shakeY) {
        _targetTransform.position = new Vector3 (
            _targetTransform.position.x,
            _targetTransform.position.y,
            shakeY
        );
    }
*/