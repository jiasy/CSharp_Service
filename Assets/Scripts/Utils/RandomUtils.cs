using System;
using UnityEngine;

namespace Utils {
    public class RandomUtils {

        public void doSample () {

        }

        // 在 pos_ 附近
        // 在 range_ 范围内随机
        // 获取一个点
        public static Vector2 randomPosInRange (Vector2 pos_, float range_) {
            float _dis = UnityEngine.Random.value * range_;
            float _angle = UnityEngine.Random.value * 360.0f;
            float _radians = CircleUtils.DegreetoRadians (_angle);
            Vector2 _temp2 = new Vector2 (Mathf.Cos (_radians) * _dis, Mathf.Sin (_radians) * _dis);
            return _temp2;
        }

    }
}