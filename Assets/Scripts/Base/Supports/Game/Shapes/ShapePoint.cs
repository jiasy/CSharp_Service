using System;
using System.Collections;
using System.Collections.Generic;
using Objs;
using UnityEngine;
using Utils;

namespace Game {
    //碰撞形状
    public class ShapePoint : Shape2DObj {
        public float dvalue = 0.05f; //点的近似值
        public ShapePoint (Vector2 pos_) : base (pos_) {
            type = ShapeType.Point;
            //第一次resetPos不会触发。只会触发形状。因为pos 就是传递进来的 pos_。所以，第一次比较会是一样的
            reset (pos_.x, pos_.y);
        }
        public void reset (float x_, float y_) {
            changePos (x_, y_);
        }
        public override void Dispose () {
            base.Dispose ();
        }
        //碰撞值,无论多少，认为碰不上
        public override bool hitV2 (Vector2 targetV2_) {
            return false;
        }
        public override bool hitOther (ShapePoint other_) {
            // float _disx = (other_.pos.x - pos.x);
            // float _disy = (other_.pos.y - pos.y);
            // //5以内认为，点碰点
            // if (_disx * _disx + _disy * _disy <= dvalue * dvalue) {
            //     return true;
            // }
            // return false;
            return false;
        }
        public override bool hitOther (ShapeLine other_) {
            //借用线碰撞点的结果
            return other_.hitOther (this);
        }
        public override bool hitOther (ShapeSquare other_) {
            if (other_.currentRect.Contains (pos)) {
                return true;
            }
            return false;
        }
        public override bool hitOther (ShapeRect other_) {
            if (other_.currentRect.Contains (pos)) {
                return true;
            }
            return false;
        }
        public override bool hitOther (ShapeCircle other_) {
            return other_.hitOther (this);
        }
        public override bool hitOther (ShapeSector other_) {
            return other_.hitOther (this);
        }
    }
}