using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Objs;
using UnityEngine;

namespace Utils {
    //距离判断
    public class RangeUtils {
        public static void doSample () {
            List<MoveControlObj> _mcs = new List<MoveControlObj> ();
            MoveControlObj _mc;
            for (int _idx = 0; _idx < 20; _idx++) {
                _mc = new MoveControlObj (MoveControlObj.PosType.UI);
                // 0 - 10 - ... - 190，每隔10 有一个点
                _mc.pos = new Vector2 (10 * _idx, 20);
                _mcs.Add (_mc);
            }
            _mc = new MoveControlObj (MoveControlObj.PosType.UI);
            _mc.pos = new Vector2 (22, 20);
            MoveControlObj _nearst;
            //22 最近的是20
            _nearst = nearestInRangeMC (0, 10, _mc, _mcs);
            Debug.Log ("_nearst 1 : " + _nearst.pos.x.ToString ());
            //22 最近的是20，但是距离是5为最小，所以，只能取到30最近。
            _nearst = nearestInRangeMC (5, 10, _mc, _mcs);
            Debug.Log ("_nearst 2 : " + _nearst.pos.x.ToString ());
            //22 在距离内按照创建的顺序，找到第一个就停止的话。第一个应当是0。 
            _nearst = firstInRangeMC (5, 30, _mc, _mcs);
            Debug.Log ("first : " + _nearst.pos.x.ToString ());
            //22 在距离随机找一个 
            _nearst = randomInRangeMC (25, 200, _mc, _mcs);
            Debug.Log ("random : " + _nearst.pos.x.ToString ());
            List<MoveControlObj> _nearestMCs = inRangeAndNearestMC (5, 10, _mc, _mcs);
            //22 距离 ，5-10 之间的MC集合，12 -> 17, 27 -> 32 中的满足。只有30一个
            Debug.Log ("_nearestMCs.Count : " + _nearestMCs.Count.ToString ());
        }
        public static MoveControlObj randomInRangeMC (float min_, float max_, MoveControlObj moveControl_, List<MoveControlObj> targetMoveControls_) {
            List<MoveControlObj> _inRangeMCs = inRangeAndNearestMC (min_, max_, moveControl_, targetMoveControls_, false);
            if (_inRangeMCs.Count >= 1) {
                return _inRangeMCs[UnityEngine.Random.Range (0, _inRangeMCs.Count)];
            } else {
                return null;
            }
        }
        //找到范围中的最近的一个
        public static MoveControlObj nearestInRangeMC (float min_, float max_, MoveControlObj moveControl_, List<MoveControlObj> targetMoveControls_) {
            List<MoveControlObj> _inRangeMCs = inRangeAndNearestMC (min_, max_, moveControl_, targetMoveControls_, false, true);
            if (_inRangeMCs.Count >= 1) {
                return _inRangeMCs[0];
            } else {
                return null;
            }
        }

        //找到范围中的第一个
        public static MoveControlObj firstInRangeMC (float min_, float max_, MoveControlObj moveControl_, List<MoveControlObj> targetMoveControls_) {
            List<MoveControlObj> _inRangeMCs = inRangeAndNearestMC (min_, max_, moveControl_, targetMoveControls_, true);
            if (_inRangeMCs.Count == 1) {
                return _inRangeMCs[0];
            } else {
                return null;
            }
        }

        //从移动控制器中找到范围之内的，然后，将最近的摆放到前面。
        //realPos 之间的比较。
        public static List<MoveControlObj> inRangeAndNearestMC (float min_, float max_, MoveControlObj moveControl_, List<MoveControlObj> targetMoveControls_, bool isOnlyNeedFirstOne_ = false, bool findNearest_ = false) {
            List<MoveControlObj> _inRangeMCs = new List<MoveControlObj> ();
            MoveControlObj _closestMoveControl = null;
            float _minDis = Mathf.Infinity;
            Vector2 _pos = moveControl_.realPos;
            Vector2 _otherPos;
            Vector2 _loopDiff;
            float _loopDis;
            min_ = min_ * min_;
            max_ = max_ * max_;
            foreach (MoveControlObj _mc in targetMoveControls_) {
                _otherPos = _mc.realPos;
                _loopDiff = _otherPos - _pos;
                _loopDis = _loopDiff.sqrMagnitude;
                //在范围内
                if (_loopDis >= min_ && _loopDis <= max_) {
                    _inRangeMCs.Add (_mc);
                    if (isOnlyNeedFirstOne_) {
                        return _inRangeMCs;
                    }
                }
                if (findNearest_) { //获取到距离最小的那个
                    if (_loopDis < _minDis && _loopDis >= min_ && _loopDis <= max_) {
                        _closestMoveControl = _mc;
                        _minDis = _loopDis;
                    }
                }
            }
            if (findNearest_) { //获取到距离最小的那个
                //超过一个的时候，把最近的那个挪到最前面
                if (_inRangeMCs.Count > 1) {
                    _inRangeMCs.Remove (_closestMoveControl);
                    _inRangeMCs.Insert (0, _closestMoveControl);
                }
            }
            //可能是空的，但是如果，不是空的，第一个一定是最近的那个。
            return _inRangeMCs;
        }
    }
}