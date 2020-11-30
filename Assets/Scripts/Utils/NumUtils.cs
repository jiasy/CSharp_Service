using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
namespace Utils {
    public class NumUtils {
        // int _randomInt = NumUtils.randomInt(1,100);
        public static int randomInt (int begin_, int end_) {
            int _value = Random.Range (begin_, end_);
            return _value;
        }
        public static bool isInt (string str_) {
            return Regex.IsMatch (str_, @"^\d+$");
        }
        public static int toInt (string str_) {
            return int.Parse (str_);
        }
        //翻转数组
        public static float[] reverse (float[] s_) {
            int _Length = s_.Length;
            float[] _temps = new float[_Length];
            int _targetIdx = 0;
            int _fromIdx = _Length - 1;
            while (_targetIdx != _Length) {
                _temps[_targetIdx] = s_[_fromIdx];
                _targetIdx++;
                _fromIdx--;
            }
            return _temps;
        }
        //放大倍数
        public static float[] multiple (float[] s_, float v_) {
            int _Length = s_.Length;
            float[] _temps = new float[_Length];
            for (int _idx = 0; _idx < _Length; _idx++) {
                _temps[_idx] = s_[_idx] * v_;
            }
            return _temps;
        }
    }
}