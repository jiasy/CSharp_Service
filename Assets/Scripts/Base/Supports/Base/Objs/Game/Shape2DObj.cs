using System;
using System.Collections;
using System.Collections.Generic;
using Objs;
using UnityEngine;
using Utils;

namespace Game {

    //碰撞形状
    public class Shape2DObj : BaseObj {
        public enum ShapeType {
            None,
            Point,
            Line,
            Rect,
            Square,
            Circle,
            Sector
        }

        public Vector2 pos; //移动中物体坐标的引用

        public bool isHitable = true; //当前是否可以碰撞
        public ShapeType type = ShapeType.None;
        public Shape2DObj (Vector2 v2_) : base () {
            pos = v2_;
        }
        public override void Dispose () {
            base.Dispose ();
        }
        public bool changePos (float posX_, float posY_) {
            if (pos.x != posX_ || pos.y != posY_) {
                pos.x = posX_;
                pos.y = posY_;
                return true;
            }
            return false;
        }
        public bool hitOther (Shape2DObj other_) {
            if (other_.type == ShapeType.Point) {
                return hitOther ((ShapePoint) other_);
            } else if (other_.type == ShapeType.Line) {
                return hitOther ((ShapeLine) other_);
            } else if (other_.type == ShapeType.Rect) {
                return hitOther ((ShapeRect) other_);
            } else if (other_.type == ShapeType.Square) {
                return hitOther ((ShapeSquare) other_);
            } else if (other_.type == ShapeType.Circle) {
                return hitOther ((ShapeCircle) other_);
            } else if (other_.type == ShapeType.Sector) {
                return hitOther ((ShapeSector) other_);
            }
            return false;
        }

        public virtual bool hitV2 (Vector2 v2_) {
            throw new NotImplementedException ();
        }
        public virtual bool hitOther (ShapePoint other_) {
            throw new NotImplementedException ();
        }
        public virtual bool hitOther (ShapeLine other_) {
            throw new NotImplementedException ();
        }
        public virtual bool hitOther (ShapeSquare other_) {
            throw new NotImplementedException ();
        }
        public virtual bool hitOther (ShapeRect other_) {
            throw new NotImplementedException ();
        }
        public virtual bool hitOther (ShapeCircle other_) {
            throw new NotImplementedException ();
        }
        public virtual bool hitOther (ShapeSector other_) {
            throw new NotImplementedException ();
        }
    }
}