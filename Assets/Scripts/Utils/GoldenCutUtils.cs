using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils {
    public class GoldenCutUtils {
        //螺旋线缓存
        public static Dictionary<int, Dictionary<int, Vector2[]>> _spiralCircleDict = new Dictionary<int, Dictionary<int, Vector2[]>> ();

        //黄金分割螺旋线，从一个方形开始，起始点并不是从圆心开始。     
        public static Vector2[] getGoldCutSpiralV2s (float beginR_ = 1.0f, int length_ = 10) {
            Vector2[] _v2s = new Vector2[length_];
            float _currentR = beginR_; //起始位1.0f
            float _goldCut = 1.618f;
            int _currentIdx = 0;
            int _face = 0;
            while (_currentIdx < length_) {
                _face = _currentIdx % 4; //获取方向
                _currentR = beginR_ * Mathf.Pow (_goldCut, _currentIdx); //获取当前距离原点长度。
                if (_face == 0) { //上 
                    _v2s[_currentIdx].x = 0;
                    _v2s[_currentIdx].y = _currentR;
                } else if (_face == 1) { //右
                    _v2s[_currentIdx].x = _currentR;
                    _v2s[_currentIdx].y = 0;
                } else if (_face == 2) { //下
                    _v2s[_currentIdx].x = 0;
                    _v2s[_currentIdx].y = -_currentR;
                } else if (_face == 3) { //左
                    _v2s[_currentIdx].x = -_currentR;
                    _v2s[_currentIdx].y = 0;
                }
                _currentIdx++;
            }
            return _v2s;
        }
        //正方形显示黄金分割点
        public static Vector2[] getGoldCutRectV2s (float beginR_ = 1.0f, int length_ = 10) {
            Vector2[] _v2s = new Vector2[length_ * 4 + 1]; //没一次转换坐标轴，形成的都是正方形，那么正方形是4个点，最后闭合再加一个。
            float _currentR = beginR_; //起始位1.0f
            float _goldCut = 1.618f;
            int _currentIdx = 0;
            int _face = 0;
            int _multipleIdx = 0;
            while (_currentIdx < length_) {
                _face = _currentIdx % 4; //获取方向
                _multipleIdx = _currentIdx * 4; //每一个形成一个方形
                _currentR = beginR_ * Mathf.Pow (_goldCut, _currentIdx); //获取当前距离原点长度。
                //任何一方形都是原地起步
                _v2s[_multipleIdx].x = 0;
                _v2s[_multipleIdx].y = 0;
                if (_face == 0) { //上 - 左画方形
                    _v2s[_multipleIdx + 1].x = 0;
                    _v2s[_multipleIdx + 1].y = _currentR;
                    _v2s[_multipleIdx + 2].x = -_currentR;
                    _v2s[_multipleIdx + 2].y = _currentR;
                    _v2s[_multipleIdx + 3].x = -_currentR;
                    _v2s[_multipleIdx + 3].y = 0;
                } else if (_face == 1) { //右 - 上画方形
                    _v2s[_multipleIdx + 1].x = _currentR;
                    _v2s[_multipleIdx + 1].y = 0;
                    _v2s[_multipleIdx + 2].x = _currentR;
                    _v2s[_multipleIdx + 2].y = _currentR;
                    _v2s[_multipleIdx + 3].x = 0;
                    _v2s[_multipleIdx + 3].y = _currentR;
                } else if (_face == 2) { //下 - 右画方形
                    _v2s[_multipleIdx + 1].x = 0;
                    _v2s[_multipleIdx + 1].y = -_currentR;
                    _v2s[_multipleIdx + 2].x = _currentR;
                    _v2s[_multipleIdx + 2].y = -_currentR;
                    _v2s[_multipleIdx + 3].x = _currentR;
                    _v2s[_multipleIdx + 3].y = 0;
                } else if (_face == 3) { //左 - 下画方形
                    _v2s[_multipleIdx + 1].x = -_currentR;
                    _v2s[_multipleIdx + 1].y = 0;
                    _v2s[_multipleIdx + 2].x = -_currentR;
                    _v2s[_multipleIdx + 2].y = -_currentR;
                    _v2s[_multipleIdx + 3].x = 0;
                    _v2s[_multipleIdx + 3].y = -_currentR;
                }
                _currentIdx++;
            }
            _v2s[length_ * 4].x = 0;
            _v2s[length_ * 4].y = 0;
            return _v2s;
        }
        //螺旋线连线绘画
        public static Vector2[] getGoldCutSpiralBezierV2s (float beginR_ = 1.0f, int length_ = 10) {
            Vector2[] _v2s = new Vector2[length_];
            List<Vector2[]> _v2sList = new List<Vector2[]> ();
            int _finalLength = 0; //最后的曲线节点数
            float _currentR = beginR_; //起始位1.0f
            float _goldCut = 1.618f;
            int _currentIdx = 0;
            int _face = 0;
            Vector2 _p3 = new Vector2 ();
            while (_currentIdx < length_) {
                _face = _currentIdx % 4; //获取方向
                _currentR = beginR_ * Mathf.Pow (_goldCut, _currentIdx); //获取当前距离原点长度。
                if (_face == 0) { //上 
                    _v2s[_currentIdx].x = 0;
                    _v2s[_currentIdx].y = _currentR;
                } else if (_face == 1) { //右
                    _v2s[_currentIdx].x = _currentR;
                    _v2s[_currentIdx].y = 0;
                } else if (_face == 2) { //下
                    _v2s[_currentIdx].x = 0;
                    _v2s[_currentIdx].y = -_currentR;
                } else if (_face == 3) { //左
                    _v2s[_currentIdx].x = -_currentR;
                    _v2s[_currentIdx].y = 0;
                }

                float _cruvePect = 1.0f;
                //float _cruvePect = 0.618f + 0.2f;
                //当前点和上一个点可以生成贝塞尔曲线
                if (_currentIdx > 0) {
                    // 不好看
                    if (_face == 0 || _face == 2) { //上、下 
                        _p3.x = _v2s[_currentIdx - 1].x;
                        _p3.y = _v2s[_currentIdx].y * _cruvePect;
                    } else if (_face == 1 || _face == 3) { //右、左
                        _p3.x = _v2s[_currentIdx].x * _cruvePect;
                        _p3.y = _v2s[_currentIdx - 1].y;
                    }
                    //当前半径除2来做点个数
                    Vector2[] _tempCruveV2s = CruveUtils.getBezierV2s (_v2s[_currentIdx - 1], _v2s[_currentIdx], _p3, (int) _currentR / 2);
                    _v2sList.Add (_tempCruveV2s);
                    _finalLength += _tempCruveV2s.Length;
                }
                _currentIdx++;
            }
            //将各段的曲线节点集合合并回一个数组。
            Vector2[] _finalCruveV2s = new Vector2[_finalLength];
            int _currentCruveIdx = 0;
            for (int _idx = 0; _idx < _v2sList.Count; _idx++) {
                Vector2[] _tempV2s = _v2sList[_idx];
                for (int _insideIdx = 0; _insideIdx < _tempV2s.Length; _insideIdx++) {
                    _finalCruveV2s[_currentCruveIdx] = _tempV2s[_insideIdx];
                    _currentCruveIdx++; //主数组的索引位置。
                }
            }
            return _finalCruveV2s;
        }
        //螺旋线连线绘画渐变圆周
        public static Vector2[] getGoldCutSpiralCircleV2s (float beginR_ = 10.0f, int length_ = 10) {
            List<Vector2> _v2sList = new List<Vector2> ();
            if (beginR_ < 10.0f) {
                Debug.LogError ("GoldCutUtils -> getGoldCutSpiralCircleV2s : beginR_ 不能小于10.0f，以免出现问题");
                return null;
            }
            float _baseR = beginR_;
            float _currentR = _baseR; //当前的点位
            float _nextR = _currentR; //下一个目标
            float _currentDegree = 0.0f; //当前角度，用来算当前点坐标
            float _currentBufferDegree = 0.0f; //当前的半径下，移动一个像素的圆周，要运动多大角度。
            int _currentFace = 0; //0 上右，1 下右，2 下左，3 上左
            int _changeFaceCount = 0;
            Vector2 _currentV2;
            bool _break = false;
            int _realLength = length_ + 1;
            //突破象限的次数小于规定次数，就继续
            while (_changeFaceCount < _realLength) {
                _currentV2 = CircleUtils.getV2 (_currentDegree, (int) _currentR); //获取当前的坐标
                _v2sList.Add (_currentV2);
                _currentBufferDegree = 10 * CircleUtils.getRToDegree ((int) _currentR); //获取当前半径下，获取10像素弧度需要多少角度
                _currentDegree -= _currentBufferDegree; //变更当前角度
                _currentR = _currentR + (_nextR - _baseR) * (_currentBufferDegree / 90.0f); //当前变化角度评分90度，获取当前角度变化，对应的半径变化
                if (_currentFace == 0 && _currentV2.x < 0) { //突破第一象限，向左侧
                    _break = true;
                } else if (_currentFace == 1 && _currentV2.y < 0) { //突破第二象限，向下去
                    _break = true;
                } else if (_currentFace == 2 && _currentV2.x > 0) { //突破第三象限，向右去
                    _break = true;
                } else if (_currentFace == 3 && _currentV2.y > 0) { //突破第四象限，向上去
                    _break = true;
                } else {
                    _break = false;
                }
                if (_break) { //突破象限的时候
                    _currentFace++; //象限变更
                    if (_currentFace == 4) { //超越4象限，就返回1
                        _currentFace = 0;
                    }
                    _changeFaceCount++; //突破次数变更
                    _currentR = _nextR; //当前象限起始半径
                    _baseR = _currentR; //记录当前起始值
                    _nextR = _currentR * 1.618f; //下一个目标半径。
                }
            }
            //转换成数组，返回
            return _v2sList.ToArray ();
        }
        //旋转式，正确的画法
        public static Vector2[] getRectV2sReal (float beginR_ = 1.0f, int length_ = 10) {
            List<Vector2> _v2sList = new List<Vector2> ();
            //创建一边为1，另一边为1.618的长方形，然后，在1.618的边做一个1.618为边长的正方形。
            //然后，用长方形 1 + 1.618 的正方形组成的长方形处，再创建 一个 (1 + 1.618)*1.618 为边长的正方形
            int _currentFace = 0; // 0左，1上，2右，3下
            Vector2 _currentV2 = new Vector2 (0.0f, 0.0f);
            int _faceChangeCount = 0;
            float _currentR = (float) beginR_;
            while (_faceChangeCount < length_) {
                //当前为起始点，画正方形
                if (_currentFace == 0) { //左下 画正方形
                    _v2sList.Add (new Vector2 (_currentV2.x, _currentV2.y));
                    _v2sList.Add (new Vector2 (_currentV2.x, _currentV2.y - _currentR));
                    _v2sList.Add (new Vector2 (_currentV2.x - _currentR, _currentV2.y - _currentR));
                    _v2sList.Add (new Vector2 (_currentV2.x - _currentR, _currentV2.y));
                } else if (_currentFace == 1) { //左上
                    _v2sList.Add (new Vector2 (_currentV2.x, _currentV2.y));
                    _v2sList.Add (new Vector2 (_currentV2.x - _currentR, _currentV2.y));
                    _v2sList.Add (new Vector2 (_currentV2.x - _currentR, _currentV2.y + _currentR));
                    _v2sList.Add (new Vector2 (_currentV2.x, _currentV2.y + _currentR));
                } else if (_currentFace == 2) { //右上
                    _v2sList.Add (new Vector2 (_currentV2.x, _currentV2.y));
                    _v2sList.Add (new Vector2 (_currentV2.x, _currentV2.y + _currentR));
                    _v2sList.Add (new Vector2 (_currentV2.x + _currentR, _currentV2.y + _currentR));
                    _v2sList.Add (new Vector2 (_currentV2.x + _currentR, _currentV2.y));
                } else if (_currentFace == 3) { //右下
                    _v2sList.Add (new Vector2 (_currentV2.x, _currentV2.y));
                    _v2sList.Add (new Vector2 (_currentV2.x + _currentR, _currentV2.y));
                    _v2sList.Add (new Vector2 (_currentV2.x + _currentR, _currentV2.y - _currentR));
                    _v2sList.Add (new Vector2 (_currentV2.x, _currentV2.y - _currentR));
                }

                //然后，偏移当前圆心
                if (_currentFace == 0) { //右移圆心
                    _currentV2.x = _currentV2.x + _currentR * 0.618f;
                } else if (_currentFace == 1) {
                    _currentV2.y = _currentV2.y - _currentR * 0.618f;
                } else if (_currentFace == 2) {
                    _currentV2.x = _currentV2.x - _currentR * 0.618f;
                } else if (_currentFace == 3) {
                    _currentV2.y = _currentV2.y + _currentR * 0.618f;
                }
                _currentR *= 1.618f;
                _currentFace++;
                if (_currentFace == 4) {
                    _currentFace = 0;
                }
                _faceChangeCount++;
            }

            //最后添加一个封堵结尾
            if (_currentFace == 0) { //右
                _v2sList.Add (new Vector2 (_v2sList[_v2sList.Count - 1].x, _v2sList[_v2sList.Count - 1].y + _currentR * 0.618f));
            } else if (_currentFace == 1) { //下
                _v2sList.Add (new Vector2 (_v2sList[_v2sList.Count - 1].x + _currentR * 0.618f, _v2sList[_v2sList.Count - 1].y));
            } else if (_currentFace == 2) { //左
                _v2sList.Add (new Vector2 (_v2sList[_v2sList.Count - 1].x, _v2sList[_v2sList.Count - 1].y - _currentR * 0.618f));
            } else if (_currentFace == 3) { //上
                _v2sList.Add (new Vector2 (_v2sList[_v2sList.Count - 1].x - _currentR * 0.618f, _v2sList[_v2sList.Count - 1].y));
            }

            return _v2sList.ToArray ();
        }
        //旋转绘画弧线
        public static Vector2[] getCruveV2sReal (float beginR_ = 1.0f, int length_ = 10) {
            List<Vector2> _v2sList = new List<Vector2> ();
            //创建一边为1，另一边为1.618的长方形，然后，在1.618的边做一个1.618为边长的正方形。
            //然后，用长方形 1 + 1.618 的正方形组成的长方形处，再创建 一个 (1 + 1.618)*1.618 为边长的正方形
            int _currentFace = 0; // 0左，1上，2右，3下
            Vector2 _currentV2 = new Vector2 (0.0f, 0.0f);
            int _faceChangeCount = 0;
            float _currentR = (float) beginR_;
            int _v2Num = (int) _currentR;
            while (_faceChangeCount < length_) {
                Vector2[] _v2s = V2Utils.percent (0.25f, CircleUtils.getV2s ((int) _currentR, _v2Num));
                //当前为起始点，画正方形
                if (_currentFace == 0) { //左下 画四分之一方形
                    _v2s = V2Utils.flipXY (_v2s);
                } else if (_currentFace == 1) { //左上
                    _v2s = V2Utils.flipX (_v2s);
                    _v2s = V2Utils.reverse (_v2s);
                } else if (_currentFace == 2) { //右上

                } else if (_currentFace == 3) { //右下
                    _v2s = V2Utils.flipY (_v2s);
                    _v2s = V2Utils.reverse (_v2s);
                }

                //按照当前的圆心移动点，拼接前后节点
                _v2s = V2Utils.trans (_currentV2, _v2s);
                //组合节点数组
                _v2sList = V2Utils.addToList (_v2sList, _v2s);

                //偏移当前圆心
                if (_currentFace == 0) { //右移圆心
                    _currentV2.x = _currentV2.x + _currentR * 0.618f;
                } else if (_currentFace == 1) {
                    _currentV2.y = _currentV2.y - _currentR * 0.618f;
                } else if (_currentFace == 2) {
                    _currentV2.x = _currentV2.x - _currentR * 0.618f;
                } else if (_currentFace == 3) {
                    _currentV2.y = _currentV2.y + _currentR * 0.618f;
                }

                _currentR *= 1.618f;
                _currentFace++;
                if (_currentFace == 4) {
                    _currentFace = 0;
                }
                _faceChangeCount++;
                //圆周画点个数，超过360个，就按照360个来做吧
                _v2Num = (int) _currentR;
                if (_v2Num > 100) {
                    _v2Num = 100;
                }
            }

            return _v2sList.ToArray ();
        }
    }
}