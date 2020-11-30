using System;
using System.Collections.Generic;
using Objs;
using UnityEngine;

namespace Utils {
    public class CircleUtils {
        //如果一个半径算过的话，就直接拿来用
        //                       半径            点个数           等距螺旋距离 : 点
        private static Dictionary<int, Dictionary<int, Dictionary<int, Vector2[]>>> _pointsDict = new Dictionary<int, Dictionary<int, Dictionary<int, Vector2[]>>> ();
        //圆周数组
        private static Dictionary<int, float> _circumferenceDict = new Dictionary<int, float> ();
        //半径对应角度bufferDegree
        private static Dictionary<int, float> _rToDegreeDict = new Dictionary<int, float> ();
        //半径对应弧度bufferRadians
        private static Dictionary<int, float> _rToRadinansDict = new Dictionary<int, float> ();

        private static float[] cosPreSave = null;
        private static float[] sinPreSave = null;

        public static float DegreetoRadians (float x_) {
            return x_ * 0.017453292519943295769f;
        }
        public static float RadianstoDegrees (float x_) {
            return x_ * 57.295779513082321f;
        }
        //保持值在 正负 PI之间
        public static float getNormalPI (float radians_) {
            float _PI = 3.141592f;
            while (radians_ < -_PI) {
                radians_ += 2 * _PI;
            }
            while (radians_ > _PI) {
                radians_ -= 2 * _PI;
            }
            return radians_;
        }
        //-180 <-> 180 之间的弧度，每隔5度，记录一个PI值
        public static float sinPre (float radians_) {
            float _radians;
            int _length;
            if (sinPreSave == null) {
                RangeAverageCutterObj _cutter = new RangeAverageCutterObj ();
                _cutter.resetAverage (0, Mathf.PI * 2, 721); //将2PI分成720份。两边边缘都取，所以是721份
                _length = _cutter.valuesInRange.Length;
                sinPreSave = new float[_length];
                for (int _idx = 0; _idx < _length; _idx++) {
                    _radians = _cutter.valuesInRange[_idx];
                    sinPreSave[_idx] = Mathf.Sin (_radians);
                }
                _cutter.Dispose ();
            }
            //保持值在 正负 PI之间
            radians_ = getNormalPI (radians_);
            //相对于 - Mathf.IP 的全程弧度
            float _rangePI = radians_ + Mathf.PI;
            //占 2 Mathf.PI 的百分比，就是数组的百分比作为序号取得预存元素。
            int _backIdx = Mathf.FloorToInt (721 * (_rangePI / (2 * Mathf.PI)));
            //_length = sinPreSave.Length;
            // if (_backIdx >= _length) {
            //     _backIdx = _length - 1;
            // }
            return sinPreSave[_backIdx];
        }
        public static float cosPre (float radians_) {
            float _radians;
            int _length;
            if (cosPreSave == null) {
                RangeAverageCutterObj _cutter = new RangeAverageCutterObj ();
                _cutter.resetAverage (0, Mathf.PI * 2, 721); //将2PI分成720份。两边边缘都取，所以是721份
                _length = _cutter.valuesInRange.Length;
                cosPreSave = new float[_length];
                for (int _idx = 0; _idx < _length; _idx++) {
                    _radians = _cutter.valuesInRange[_idx];
                    cosPreSave[_idx] = Mathf.Cos (_radians);
                }
                _cutter.Dispose ();
            }
            //保持值在 正负 PI之间
            _radians = getNormalPI (radians_);
            //相对于 - Mathf.IP 的全程弧度
            float _rangePI = _radians + Mathf.PI;
            //占 2 Mathf.PI 的百分比，就是数组的百分比作为序号取得预存元素。
            int _backIdx = Mathf.FloorToInt (721 * (_rangePI / (2 * Mathf.PI)));
            // _length = cosPreSave.Length;
            // if (_backIdx >= _length) {
            //     _backIdx = _length - 1;
            // }

            float _value = 0f;
            // try {
            _value = cosPreSave[_backIdx];
            // } catch (Exception e) {
            //     Debug.LogError ("ERROR " + System.Reflection.MethodBase.GetCurrentMethod ().ReflectedType.FullName + " -> " + new System.Diagnostics.StackTrace ().GetFrame (0).GetMethod ().Name + " : " +
            //         "索引错误"
            //     );
            // }

            return _value;
        }
        //获取 r_ 对应的点集合
        public static Vector2[] getV2s (int r_, int pointNum_ = 0, int buffer_ = 0) {
            //半径，
            Dictionary<int, Dictionary<int, Vector2[]>> _rDict;
            if (_pointsDict.ContainsKey (r_)) {
                _rDict = _pointsDict[r_];
            } else {
                _rDict = new Dictionary<int, Dictionary<int, Vector2[]>> ();
                _pointsDict[r_] = _rDict;
            }
            //点个数
            Dictionary<int, Vector2[]> _pDict;
            if (_rDict.ContainsKey (pointNum_)) {
                _pDict = _rDict[pointNum_];
            } else {
                _pDict = new Dictionary<int, Vector2[]> ();
                _rDict[pointNum_] = _pDict;
            }
            //等距螺旋距离
            Vector2[] _v2s;
            if (_pDict.ContainsKey (buffer_)) {
                _v2s = _pDict[buffer_];
            } else {
                _v2s = calculatePoints (r_, pointNum_, buffer_);
                _pDict[buffer_] = _v2s;
            }
            return _v2s;
        }
        //创建 r_ 对应的圆周点集合
        private static Vector2[] calculatePoints (int r_, int pointNum_ = 0, int buffer_ = 0) {
            int _length = pointNum_;
            if (_length == 0) { //默认值
                int _rPreCut = 10; //半径，每增加多少，添加一个圆周点
                int _tempR = r_;
                if (_tempR < 20) {
                    Debug.LogError ("ERROR CircleUtils calculatePoints : 半径太小，没办法用默认的方式计算\n   请手动输入 pointNum_ 确定分割圆周的点个数");
                }
                if (buffer_ != 0) { //等距螺旋的时候，其实半径，和附加值相加
                    _tempR = (r_ + buffer_) / 2; //减半为临时半径
                }
                if (_tempR > 360 * _rPreCut) { //一单位，一分圆
                    _length = 360;
                } else { //最大360分
                    _length = _tempR / _rPreCut;
                }
            }
            //循环用的变量，不用每次循环创建
            float _perDegree = 360.0f / (float) _length;
            float _perBuffer = (float) buffer_ / (float) _length;
            float _currentR = r_;
            float _currentRadians = 0.0f;
            float XPos;
            float YPos;

            //等距螺旋线，的最后一个位置和起始位置不重合，所以要多算一个
            if (buffer_ != 0) {
                _length = _length + 1;
            }
            //第一个和最后一个，是一样的。只是这样不用在循环中写判断，最后一个可以直接收尾相接
            Vector2[] _linePoints = new Vector2[_length];
            //每一份对应的圆周坐标
            for (int _idx = 0; _idx < _length; _idx++) {
                _currentR = r_ + _idx * _perBuffer;
                _currentRadians = DegreetoRadians (_idx * _perDegree);
                XPos = Mathf.Sin (_currentRadians) * _currentR;
                YPos = Mathf.Cos (_currentRadians) * _currentR;
                _linePoints[_idx] = new Vector2 (XPos, YPos);
            }
            return _linePoints;
        }

        //通过变更中点，获取新的圆周点集合，等距螺旋线/圆周的一部分
        public static Vector2[] getCirclePointsVec2 (Vector2 centerPos_, int r_, int pointNum_ = 0, int buffer_ = 0, float percent_ = 1.0f) {
            Vector2[] _linePoints = getV2s (r_, pointNum_, buffer_);
            _linePoints = V2Utils.trans (centerPos_, _linePoints);
            if (percent_ >= 1.0f) { //全部取得
                return _linePoints;
            } else { //如果只去其中的一部分，那么，算出要多少个点
                _linePoints = V2Utils.percent (percent_, _linePoints);
            }
            return _linePoints;
        }
        public static Vector2 getV2 (float degree_, int r_) {
            //获取当前弧度
            float _currentRadins = DegreetoRadians (degree_);
            //获取当前坐标
            return new Vector2 (Mathf.Sin (_currentRadins) * r_, Mathf.Cos (_currentRadins) * r_);
        }

        //获取圆周
        public static float getCircumference (int r_) {
            if (!_circumferenceDict.ContainsKey (r_)) {
                _circumferenceDict[r_] = 2.0f * Mathf.PI * (float) r_;
            }
            return _circumferenceDict[r_];
        }
        //获取圆周角度
        public static float getRToDegree (int r_) {
            if (!_rToDegreeDict.ContainsKey (r_)) {
                float _circumference = getCircumference (r_); //周长
                _rToDegreeDict[r_] = 1.0f / _circumference * 360.0f; //弧长 比 周长 = 角度 比 360
                _rToRadinansDict[r_] = 1.0f / _circumference * Mathf.PI;
            }
            return _rToDegreeDict[r_];
        }
        //获取圆周弧度
        public static float getRToRadians (int r_) {
            if (!_rToRadinansDict.ContainsKey (r_)) {
                float _circumference = getCircumference (r_); //周长
                _rToDegreeDict[r_] = 1.0f / _circumference * 360.0f; //弧长 比 周长 = 角度 比 360
                _rToRadinansDict[r_] = 1.0f / _circumference * Mathf.PI;
            }
            return _rToRadinansDict[r_];
        }

    }
}