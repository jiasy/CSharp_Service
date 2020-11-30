using UnityEngine;

namespace Utils {
    public class CruveUtils {

        public static Vector3 Bezier (Vector3 p0, Vector3 p1, Vector3 p2, float t) {
            Vector3 p0p1 = (1 - t) * p0 + t * p1;
            Vector3 p1p2 = (1 - t) * p1 + t * p2;
            Vector3 result = (1 - t) * p0p1 + t * p1p2;
            return result;
        }

        public static Vector2 getBezierV2 (Vector2 begin_, Vector2 cruveTo_, Vector2 end_, float pect_) {
            // Vector2 p0p1 = (1 - t) * p0 + t * p1;
            // Vector2 p1p2 = (1 - t) * p1 + t * p2;
            // Vector2 result = (1 - t) * p0p1 + t * p1p2;
            return (1 - pect_) * ((1 - pect_) * begin_ + pect_ * cruveTo_) + pect_ * ((1 - pect_) * cruveTo_ + pect_ * end_);
        }

        public static Vector2[] getBezierV2s (Vector2 begin_, Vector2 end_, Vector2 cruveTo_, int length_) {
            int _currentIdx = 0;
            float _perLength = 1.0f / (float) length_;
            Vector2[] _v2s = new Vector2[length_];
            float _currentT = 0.0f;
            while (_currentIdx < length_) { //不算起点的下一个点
                _currentT += _perLength;
                _v2s[_currentIdx] = (1 - _currentT) * ((1 - _currentT) * begin_ + _currentT * cruveTo_) + _currentT * ((1 - _currentT) * cruveTo_ + _currentT * end_);
                //_v2s[_currentIdx] = getBezierV2(begin_,cruveTo_,end_,_currentT);
                _currentIdx++;
            }
            return _v2s;
        }

    }
}