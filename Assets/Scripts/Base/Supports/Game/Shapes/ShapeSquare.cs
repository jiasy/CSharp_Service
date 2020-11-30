using System;
using System.Collections;
using System.Collections.Generic;
using Objs;
using UnityEngine;
using Utils;

namespace Game {
    //碰撞形状
    public class ShapeSquare : Shape2DObj {
        public float side = Mathf.Infinity;
        public float side2;
        public Rect currentRect;

        public ShapeSquare (Vector2 pos_, float side_) : base (pos_) {
            type = ShapeType.Square;
            currentRect = new Rect ();
            //第一次resetPos不会触发。只会触发形状。因为pos 就是传递进来的 pos_。所以，第一次比较会是一样的
            reset (pos_.x, pos_.y, side_);
        }
        public void reset (float x_, float y_, float side_) {
            resetPos (x_, y_);
            resetSide (side_);
        }
        public override void Dispose () {
            base.Dispose ();
        }
        public void resetPos (float x_, float y_) {
            if (changePos (x_, y_)) {
                resetDetail ();
            }
        }
        public void resetSide (float side_) {
            if (side != side_) {
                side = side_;
                side2 = side * 0.5f;
                resetDetail ();
            }
        }
        public void resetDetail () {
            //重新创建，比设置四次值要好。每一次设置值，都要重新运算一下Rect的参数
            currentRect = new Rect (
                pos.x - side2,
                pos.y - side2,
                side,
                side
            );
        }
        //碰撞值
        public override bool hitV2 (Vector2 targetV2_) {
            return currentRect.Contains (targetV2_);
        }
        public override bool hitOther (ShapePoint other_) {
            //借用点碰撞方形
            return other_.hitOther (this);
        }
        public override bool hitOther (ShapeLine other_) {
            return other_.hitOther (this);
        }
        public override bool hitOther (ShapeSquare other_) {
            if (currentRect.Overlaps (other_.currentRect)) {
                return true;
            }
            return false;
        }
        public override bool hitOther (ShapeRect other_) {
            return other_.hitOther (this);
        }
        public override bool hitOther (ShapeCircle other_) {
            int type = 1; //1,正常比较，2，都当成方形，3，都当成圆形
            if (side2 > other_.radius && side2 < 100) { //方形大，方形半径小于100
                type = 2;
            } else if (other_.radius > side2 && other_.radius < 100) { //圆形大，半径小于100
                type = 3;
            } else {
                if (side2 * 0.5f > other_.radius) { //半边长大于圆的半径，那么圆当成是方形
                    type = 2;
                } else if (other_.radius > side) { //半径比边长大，那么方形当成是圆型
                    type = 3;
                }
            }
            if (type == 2) {
                if (Math.Abs (pos.x - other_.pos.x) < side2 + other_.radius) {
                    if (Math.Abs (pos.y - other_.pos.y) < side2 + other_.radius) {
                        return true;
                    }
                }
                return false;
            } else if (type == 3) {
                float _dx = other_.pos.x - pos.x;
                float _dy = other_.pos.y - pos.y;
                float _dis = (other_.radius + side2);
                if (_dx * _dx + _dy * _dy < _dis * _dis) { //半径距离
                    return true;
                }
                return false;
            }
            //大小接近的时候，按照正常的来做
            Vector2 v = Vector2.Max (pos - other_.pos, (other_.pos - pos));
            Vector2 u = Vector2.Max (v - new Vector2 (side2, side2), Vector2.zero);
            return u.sqrMagnitude < other_.radius * other_.radius;
        }
        public override bool hitOther (ShapeSector other_) {
            return other_.hitOther (this);
        }
    }
}