using System;
using System.Collections;
using System.Collections.Generic;
using Objs;
using UnityEngine;

namespace Game {
    //碰撞形状
    public class ShapeRect : Shape2DObj {
        public float width = Mathf.Infinity;
        public float height = Mathf.Infinity;
        public float width2;
        public float height2;

        public Rect currentRect;

        public ShapeRect (Vector2 pos_, float width_, float height_) : base (pos_) {
            type = ShapeType.Rect;
            currentRect = new Rect ();
            //第一次resetPos不会触发。只会触发形状。因为pos 就是传递进来的 pos_。所以，第一次比较会是一样的
            reset (pos_.x, pos_.y, width_, height_);
        }
        public void reset (float x_, float y_, float w_, float h_) {
            resetPos (x_, y_);
            resetWH (w_, h_);
        }
        public override void Dispose () {

            base.Dispose ();
        }
        public void resetPos (float x_, float y_) {
            if (changePos (x_, y_)) {
                resetDetail ();
            }
        }
        public void resetWH (float width_, float height_) {
            if (
                width != width_ ||
                height != height_
            ) {
                width = width_;
                height = height_;
                width2 = width * 0.5f;
                height2 = height * 0.5f;
                resetDetail ();
            }
        }
        public void resetDetail () {
            currentRect = new Rect (
                pos.x - width2,
                pos.y - height2,
                width,
                height
            );
        }
        //碰撞值,无论多少，认为碰不上
        public override bool hitV2 (Vector2 targetV2_) {
            if (currentRect.Contains (targetV2_)) {
                return true;
            }
            return false;
        }
        public override bool hitOther (ShapePoint other_) {
            //借用点碰撞长方形
            return other_.hitOther (this);
        }
        public override bool hitOther (ShapeLine other_) {
            return other_.hitOther (this);
        }
        public override bool hitOther (ShapeSquare other_) {
            return currentRect.Overlaps (other_.currentRect);
        }
        public override bool hitOther (ShapeRect other_) {
            return currentRect.Overlaps (other_.currentRect);
        }
        public override bool hitOther (ShapeCircle other_) {
            //不搭边
            if (
                currentRect.xMax < other_.xMin ||
                currentRect.xMin > other_.xMax ||
                currentRect.yMax < other_.yMin ||
                currentRect.yMin > other_.yMax
            ) {
                return false;
            }
            //搭边在判断后面
            if (
                other_.radius > width2 ||
                other_.radius > height2
            ) { //当直径大于最小的一个边，才按照长方形和圆的碰撞方式碰撞
                Vector2 v = Vector2.Max (pos - other_.pos, (other_.pos - pos));
                Vector2 u = Vector2.Max (v - new Vector2 (width2, height2), Vector2.zero);
                return u.sqrMagnitude < other_.radius * other_.radius;
            } else { //当直径比长宽都窄的时候，按照正方形的方式进行判断。
                if (Math.Abs (pos.x - other_.pos.x) < (width2 + other_.radius)) {
                    if (Math.Abs (pos.y - other_.pos.y) < (height2 + other_.radius)) {
                        return true;
                    }
                }
                return false;
            }
        }
        public override bool hitOther (ShapeSector other_) {
            return other_.hitOther (this);
        }
    }
}