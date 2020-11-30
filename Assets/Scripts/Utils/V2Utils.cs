using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace Utils {
    public class V2Utils {
        //X方向翻转
        public static Vector2[] flipX (Vector2[] v2s_) {
            int _v2Length = v2s_.Length;
            Vector2[] _tempV2s = new Vector2[_v2Length];
            for (int _idx = 0; _idx < _v2Length; _idx++) {
                _tempV2s[_idx].x = -v2s_[_idx].x;
                _tempV2s[_idx].y = v2s_[_idx].y;
            }
            return _tempV2s;
        }
        //Y方向翻转
        public static Vector2[] flipY (Vector2[] v2s_) {
            int _v2Length = v2s_.Length;
            Vector2[] _tempV2s = new Vector2[_v2Length];
            for (int _idx = 0; _idx < _v2Length; _idx++) {
                _tempV2s[_idx].x = v2s_[_idx].x;
                _tempV2s[_idx].y = -v2s_[_idx].y;
            }
            return _tempV2s;
        }
        //XY方向翻转
        public static Vector2[] flipXY (Vector2[] v2s_) {
            int _v2Length = v2s_.Length;
            Vector2[] _tempV2s = new Vector2[_v2Length];
            for (int _idx = 0; _idx < _v2Length; _idx++) {
                _tempV2s[_idx].x = -v2s_[_idx].x;
                _tempV2s[_idx].y = -v2s_[_idx].y;
            }
            return _tempV2s;
        }
        //平移
        public static Vector2[] trans (Vector2 bufferPos_, Vector2[] v2s_) {
            if ((int) bufferPos_.x != 0 || (int) bufferPos_.y != 0) {
                int _v2Length = v2s_.Length;
                Vector2[] _tempV2s = new Vector2[_v2Length];
                for (int _idx = 0; _idx < _v2Length; _idx++) {
                    _tempV2s[_idx].x = bufferPos_.x + v2s_[_idx].x;
                    _tempV2s[_idx].y = bufferPos_.y + v2s_[_idx].y;
                }
                return _tempV2s;
            } else {
                return v2s_;
            }
        }

        public static Vector2[] rotate (float angle_, Vector2[] v2s_) {
            if (angle_ == 0.0f) {
                return v2s_;
            }
            int _v2Length = v2s_.Length;
            Vector2[] _tempV2s = new Vector2[_v2Length];
            for (int _idx = 0; _idx < _v2Length; _idx++) {
                _tempV2s[_idx] = LineUtils.rotation (v2s_[_idx], angle_);
            }
            return _tempV2s;
        }

        //x方向放缩
        public static Vector2[] scaleX (float scaleX_, Vector2[] v2s_) {
            if (scaleX_ == 1.0f) {
                return v2s_;
            }
            int _v2Length = v2s_.Length;
            Vector2[] _tempV2s = new Vector2[_v2Length];
            for (int _idx = 0; _idx < _v2Length; _idx++) {
                _tempV2s[_idx].x = (float) v2s_[_idx].x * scaleX_;
                _tempV2s[_idx].y = v2s_[_idx].y;
            }
            return _tempV2s;
        }
        //y方向放缩
        public static Vector2[] scaleY (float scaleY_, Vector2[] v2s_) {
            if (scaleY_ == 1.0f) {
                return v2s_;
            }
            int _v2Length = v2s_.Length;
            Vector2[] _tempV2s = new Vector2[_v2Length];
            for (int _idx = 0; _idx < _v2Length; _idx++) {
                _tempV2s[_idx].x = v2s_[_idx].x;
                _tempV2s[_idx].y = v2s_[_idx].y * scaleY_;
            }
            return _tempV2s;
        }
        //xy方向放缩
        public static Vector2[] scale (float scale_, Vector2[] v2s_) {
            int _v2Length = v2s_.Length;
            Vector2[] _tempV2s = new Vector2[_v2Length];
            for (int _idx = 0; _idx < _v2Length; _idx++) {
                _tempV2s[_idx].x = v2s_[_idx].x * scale_;
                _tempV2s[_idx].y = v2s_[_idx].y * scale_;
            }
            return _tempV2s;
        }
        //取其中一部份
        public static Vector2[] percent (float percent_, Vector2[] v2s_) {
            int _v2Length = v2s_.Length;
            int _tempLength = (int) ((float) _v2Length * percent_);
            Vector2[] _tempV2s = new Vector2[_tempLength];
            for (int i = 0; i < _tempLength; ++i) {
                _tempV2s[i].x = v2s_[i].x;
                _tempV2s[i].y = v2s_[i].y;
            }
            return _tempV2s;
        }
        //倒叙
        public static Vector2[] reverse (Vector2[] v2s_) {
            int _v2Length = v2s_.Length;
            Vector2[] _tempV2s = new Vector2[_v2Length];
            int _targetIdx = 0;
            int _fromIdx = _v2Length - 1;
            while (_targetIdx != _v2Length) {
                _tempV2s[_targetIdx].x = v2s_[_fromIdx].x;
                _tempV2s[_targetIdx].y = v2s_[_fromIdx].y;
                _targetIdx++;
                _fromIdx--;
            }
            return _tempV2s;
        }
        //间隔几个取新数组
        public static Vector2[] interval (int interval_, Vector2[] v2s_, int start_ = 0) {
            if (interval_ <= 1) {
                Debug.LogError ("V2Utils -> interval : interval_ 不能小于等于1");
            }
            if (start_ >= interval_) {
                Debug.LogError ("V2Utils -> interval : start_ 只能比 interval_ 小");
            }
            int _v2Length = v2s_.Length;
            int _tempLength = (int) ((float) _v2Length / (float) interval_);
            Vector2[] _tempV2s = new Vector2[_tempLength];
            int _currentCount = 0;
            for (int i = start_; i < _v2Length; ++i) {
                if ((i - start_) % interval_ == 0) {
                    _tempV2s[_currentCount].x = v2s_[i].x;
                    _tempV2s[_currentCount].y = v2s_[i].y;
                    _currentCount++;
                    if (_currentCount >= _tempLength) {
                        break;
                    }
                }
            }
            return _tempV2s;
        }
        //将数组添加到List
        public static List<Vector2> addToList (List<Vector2> targetList_, Vector2[] fromV2s_) {
            int _v2Length = fromV2s_.Length;
            for (int _idx = 0; _idx < _v2Length; _idx++) {
                targetList_.Add (fromV2s_[_idx]);
            }
            return targetList_;
        }
        //从其中取多少个
        public static Vector2[] getCount (Vector2[] fromV2s_, int count_) {
            if (count_ > fromV2s_.Length) {
                Debug.LogError ("ERROR V2Utils getCount : 超长 " + count_.ToString () + "/" + fromV2s_.Length.ToString ());
                return null;
            }
            Vector2[] _tempV2s = new Vector2[count_];
            for (int _idx = 0; _idx < count_; _idx++) {
                _tempV2s[_idx] = fromV2s_[_idx];
            }
            return _tempV2s;
        }

        public static Vector2[] merge (Vector2[] v2s1_, Vector2[] v2s2_) {
            Vector2[] _tempV2s = new Vector2[v2s1_.Length + v2s2_.Length];
            int _v2Length1 = v2s1_.Length;
            int _idx = 0;
            for (_idx = 0; _idx < _v2Length1; _idx++) {
                _tempV2s[_idx] = v2s1_[_idx];
            }
            int _v2Length2 = v2s2_.Length;
            for (_idx = 0; _idx < _v2Length2; _idx++) {
                _tempV2s[_v2Length1 + _idx] = v2s2_[_idx];
            }
            return _tempV2s;
        }
        public static Vector2[] merge (Vector2[][] v2ss_) {
            int _count = 0;
            int _idx = 0;
            for (_idx = 0; _idx < v2ss_.Length; _idx++) {
                _count += v2ss_[_idx].Length;
            }
            Vector2[] _tempV2s = new Vector2[_count];
            Vector2[] _list;
            int _currentIdx = 0;
            for (_idx = 0; _idx < v2ss_.Length; _idx++) {
                _list = v2ss_[_idx];
                for (int _jdx = 0; _jdx < _list.Length; _jdx++) {
                    _tempV2s[_currentIdx] = _list[_jdx];
                    _currentIdx++;
                }
            }
            return _tempV2s;
        }

    }
}