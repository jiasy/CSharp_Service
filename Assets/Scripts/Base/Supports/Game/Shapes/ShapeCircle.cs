using System;
using System.Collections;
using System.Collections.Generic;
using Objs;
using UnityEngine;
using Utils;

namespace Game {
    //碰撞形状
    public class ShapeCircle : Shape2DObj {
        public float radius;
        public float inRadius = 0.0f;
        public float xMin;
        public float xMax;
        public float yMin;
        public float yMax;
        public ShapeCircle (Vector2 pos_, float radius_) : base (pos_) {
            type = ShapeType.Circle;
            //第一次resetPos不会触发。只会触发形状。因为pos 就是传递进来的 pos_。所以，第一次比较会是一样的
            reset (pos_.x, pos_.y, radius_);
        }
        //更新位置
        public override void Dispose () {
            base.Dispose ();
        }
        public void reset (float x_, float y_, float radius_) {
            resetPos (x_, y_);
            resetRaidus (radius_);
        }
        public void resetPos (float x_, float y_) {
            if (changePos (x_, y_)) {
                resetDetail ();
            }
        }
        public void resetRaidus (float radius_) {
            if (radius != radius_) {
                radius = radius_;
                resetDetail ();
            }
        }
        public void resetDetail () {
            xMin = pos.x - radius;
            xMax = pos.x + radius;
            yMin = pos.y - radius;
            yMax = pos.y + radius;
        }

        //碰撞值
        public override bool hitV2 (Vector2 targetV2_) {
            if ((targetV2_ - pos).sqrMagnitude < radius * radius) { //半径距离
                return true;
            }
            return false;
        }
        //圆 - 点
        public override bool hitOther (ShapePoint other_) {
            return hitV2 (other_.pos);
        }
        //圆 - 直线
        public override bool hitOther (ShapeLine other_) {
            return other_.hitOther (this);
        }
        //圆 - 正方
        public override bool hitOther (ShapeSquare other_) {
            return other_.hitOther (this);
        }
        //圆 - 长方
        public override bool hitOther (ShapeRect other_) {
            return other_.hitOther (this);
        }
        //圆 - 圆
        public override bool hitOther (ShapeCircle other_) {
            float _dx = other_.pos.x - pos.x;
            float _dy = other_.pos.y - pos.y;
            float _dis = (other_.radius + radius);
            if (_dx * _dx + _dy * _dy < _dis * _dis) { //半径距离
                return true;
            }
            return false;
        }
        //圆 - 扇
        public override bool hitOther (ShapeSector other_) {
            return other_.hitOther (this);
        }
    }
}