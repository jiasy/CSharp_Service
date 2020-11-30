using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

namespace Utils {
    public class LineUtils {
        //定比分点
        public static Vector3 interpolate (Vector3 p1_, Vector3 p2_, float pect_) {
            if (pect_ > 1 || pect_ < 0) {
                Debug.LogError ("ERROR LineUtils interpolate : pect_ 不合法 " + pect_.ToString ());
            }
            float x = (p1_.x + p2_.x * pect_) / (1 + pect_);
            float y = (p1_.y + p2_.y * pect_) / (1 + pect_);
            float z = (p1_.z + p2_.z * pect_) / (1 + pect_);
            return new Vector3 (x, y, z);
        }

        //p1_ 指向 p2_的弧度
        public static float getAngle2D (Vector2 begin_, Vector2 end_) {
            return CircleUtils.RadianstoDegrees (Mathf.Atan2 ((end_.y - begin_.y), (end_.x - begin_.x)));
        }
        public static float getRadian2D (Vector2 begin_, Vector2 end_) {
            return Mathf.Atan2 ((end_.y - begin_.y), (end_.x - begin_.x));
        }
        public static float getAngle2D (Vector2 p_) {
            return CircleUtils.RadianstoDegrees (Mathf.Atan2 (p_.y,p_.x));
        }
        public static float getRadian2D (Vector2 p_) {
            return Mathf.Atan2 (p_.y,p_.x);
        }

        //将现有的Vector 旋转
        public static Vector2 rotation (Vector2 current_, float angle_) {
            float _radians = CircleUtils.DegreetoRadians (angle_);
            float _cos = Mathf.Cos (_radians);
            float _sin = Mathf.Sin (_radians);
            return new Vector2 (
                current_.x * _cos + current_.y * _sin, -current_.x * _sin + current_.y * _cos
            );
        }

        //沿着角度，长度得到矢量
        public static Vector2 getV2ByAngleAndLength(float angle_,float length_){
            float _radians = CircleUtils.DegreetoRadians (angle_);
            //单位向量变为指向向量，然后旋转得到两侧的向量
            return new Vector2 (Mathf.Cos (_radians), Mathf.Sin (_radians)) * length_;
        }
    }
}