using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using UnityEngine;

namespace Utils {
    public class SpeedUtils {
        public static void doSample () {

        }

        //在一点上对一定范围内产生一个作用力。这个力根据位置来产生，越近越大
        //centerV3_ : 中心点
        //range_ : 范围
        //pos_ : 目标
        //xs_ : 产生速度时附加多少系数
        public static Vector3 explosionSpeedByPos (Vector3 centerV3_, float range_, Vector3 pos_, float xs_ = 0.3f) {
            Vector3 _tempV3 = new Vector3 ();
            _tempV3 = pos_ - centerV3_; //从中心指向位置的位移
            if (_tempV3.sqrMagnitude < range_ * range_) {
                //距离越近，速度越大
                //直接返回这个距离，那么一帧就飞出去了，所以要乘系数
                return (range_ - _tempV3.magnitude) * _tempV3.normalized * xs_;
            }
            return new Vector3 (0, 0, 0);
        }
        //二维的距离，产生三维的作用力
        public static Vector3 explosionSpeedByPos (Vector2 centerV2_, float range_, Vector2 pos_, float xs_ = 1.0f) {
            Vector2 _dis = pos_ - centerV2_; //从中心指向位置的位移
            if (_dis.sqrMagnitude < range_ * range_) {
                float _force = (range_ - _dis.magnitude);
                _dis = _dis.normalized * xs_ * _force;
                Vector3 _tempV3 = new Vector3 (
                    _dis.x,
                    _dis.y,
                    _force * 0.5f * xs_ //在z轴上根据当前作用力，也创建出对应的速度。
                );
                return _tempV3;
            } else {
                return new Vector3 (0, 0, 0);
            }
        }
    }
}