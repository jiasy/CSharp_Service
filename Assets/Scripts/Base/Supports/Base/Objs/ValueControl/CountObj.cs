using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Objs {
    //各种计数的基类，将判断逻辑限制到这里，直接调用取得返回值，来判断上下限情况。
    //弹药统计
    //技能冷却
    //上膛倒计时
    //Hp.Mp 加减
    //等等... 
    public class CountObj : BaseObj {
        private int _currentCount;
        private int _maxCount;
        public CountObj (int max_) : base () {
            reset (max_);
        }
        public override void Dispose () {

            base.Dispose ();
        }

        public void reset (int max_) {//重置最大值
            _maxCount = max_;
            if (_currentCount > _maxCount) { //限制当前的数值到上限。
                _currentCount = _maxCount;
            }
        }
        public void toEmptyCount () {//清空
            _currentCount = 0;
        }

        public void toMaxCount () {//加满
            _currentCount = _maxCount;
        }
        public bool add (int num_) {//添加指定数量
            _currentCount += num_;
            if (_currentCount >= _maxCount) {
                return true;
            }
            return false;
        }

        public bool countUp () {//增加1
            _currentCount += 1;
            if (_currentCount >= _maxCount) {
                return true;
            }
            return false;
        }
        public bool sub (int num_) {//减少指定数量
            _currentCount -= num_;
            if (_currentCount <= 0) {
                return true;
            }
            return false;
        }
        public bool countDown () {//减少1
            _currentCount -= 1;
            if (_currentCount <= 0) {
                return true;
            }
            return false;
        }
    }
}