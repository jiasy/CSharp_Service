using System;
using System.Collections;
using System.Collections.Generic;
using Objs;
using UnityEngine;
using Utils;

namespace Game {
    //碰撞形状
    public class ShapeLine : Shape2DObj {
        public float length = Mathf.Infinity;
        public float length2;
        //相对于 pos 的两侧点
        public Vector2 begin;
        public Vector2 end;
        //当前帧重新整合坐标后，的两侧点
        public Vector2 _currentBegin;
        public Vector2 _currentEnd;

        public float xMin;
        public float xMax;
        public float yMin;
        public float yMax;

        public float direction = Mathf.Infinity;

        public ShapeLine (Vector2 pos_, float direction_, float length_) : base (pos_) {
            type = ShapeType.Line;
            if (length_ < 0.1f) {
                Debug.LogError ("ERROR 长度小于10的线段，还不如当成是一个点来做，节省判断");
            }
            _currentBegin = new Vector2 ();
            _currentEnd = new Vector2 ();
            //第一次resetPos不会触发。只会触发形状。因为pos 就是传递进来的 pos_。所以，第一次比较会是一样的
            reset (pos_.x, pos_.y, direction_, length_);
        }
        public void reset (float posX_, float posY_, float direction_, float length_) {
            resetPos (posX_, posY_);
            resetDirectionAndLength (direction_, length_);
        }
        public void resetPos (float x_, float y_) {
            if (changePos (x_, y_)) {
                resetDetail ();
            }
        }
        public void resetDirectionAndLength (float angle_, float length_) {
            angle_ = angle_ % 360.0f;
            if (
                direction != angle_ ||
                length != length_
            ) {
                direction = angle_;
                length = length_;
                length2 = length * 0.5f;
                if (direction == 0) { //向右侧，起点左
                    begin = new Vector2 (-length2, 0);
                    end = new Vector2 (length2, 0);
                } else if (direction == 90) {
                    begin = new Vector2 (0, -length2);
                    end = new Vector2 (0, length2);
                } else if (direction == 180) {
                    begin = new Vector2 (length2, 0);
                    end = new Vector2 (-length2, 0);
                } else if (direction == 270) { //向下，起点在上
                    begin = new Vector2 (0, length2);
                    end = new Vector2 (0, -length2);
                } else {
                    float _currentRadins = CircleUtils.DegreetoRadians (direction);
                    float _endY = Mathf.Sin (_currentRadins) * length2; //沿着角度算终点
                    float _endX = Mathf.Cos (_currentRadins) * length2;
                    float _beginX = -_endX; //反向一下终点就是起点，因为是相对原点颠倒
                    float _beginY = -_endY;
                    begin = new Vector2 (_beginX, _beginY);
                    end = new Vector2 (_endX, _endY);
                }
                resetDetail ();
            }
        }
        public void resetDetail () {
            _currentBegin.x = pos.x + begin.x;
            _currentBegin.y = pos.y + begin.y;
            _currentEnd.x = pos.x + end.x;
            _currentEnd.y = pos.y + end.y;
            //算最大最小位置
            xMax = Mathf.Max (_currentBegin.x, _currentEnd.x);
            xMin = Mathf.Min (_currentBegin.x, _currentEnd.x);
            yMax = Mathf.Max (_currentBegin.y, _currentEnd.y);
            yMin = Mathf.Min (_currentBegin.y, _currentEnd.y);
        }
        public override void Dispose () {
            base.Dispose ();
        }

        //碰撞值,无论多少，认为碰不上
        public override bool hitV2 (Vector2 targetV2_) {
            return false;
        }
        
        public override bool hitOther (ShapePoint other_) {
            //Debug.LogError ("Point 和 Line 的碰撞，不应当存在。作为直线去碰撞物理，被判断的物体，应当是有体积的。");
            double _length = HitUtils.sqrDisPointToLine (_currentBegin, _currentEnd, other_.pos);
            if (_length < other_.dvalue * other_.dvalue) {
                return true;
            }
            return false;
        }
        public override bool hitOther (ShapeLine other_) {
            //Debug.LogError ("Line 和 Line 的碰撞，不应当存在。作为直线去碰撞物理，被判断的物体，应当是有体积的。");
            return HitUtils.isTwoSegmentHit (_currentBegin, _currentEnd, other_._currentBegin, other_._currentEnd);
        }
        public override bool hitOther (ShapeSquare other_) {
            //不搭边
            if (
                other_.currentRect.xMax < xMin ||
                other_.currentRect.xMin > xMax ||
                other_.currentRect.yMax < yMin ||
                other_.currentRect.yMin > yMax
            ) {
                return false;
            }
            return HitUtils.isRectHitSegment (_currentBegin, _currentEnd, other_.currentRect);
        }
        public override bool hitOther (ShapeRect other_) {
            //不搭边
            if (
                other_.currentRect.xMax < xMin ||
                other_.currentRect.xMin > xMax ||
                other_.currentRect.yMax < yMin ||
                other_.currentRect.yMin > yMax
            ) {
                return false;
            }
            return HitUtils.isRectHitSegment (_currentBegin, _currentEnd, other_.currentRect);
        }
        public override bool hitOther (ShapeCircle other_) {
            //不搭边
            if (
                other_.xMax < xMin ||
                other_.xMin > xMax ||
                other_.yMax < yMin ||
                other_.yMin > yMax
            ) {
                return false;
            }
            return HitUtils.isCircleHitSegment (_currentBegin, _currentEnd, other_.pos, other_.radius);
        }
        public override bool hitOther (ShapeSector other_) {
            //不搭边
            if (
                other_.xMax < xMin ||
                other_.xMin > xMax ||
                other_.yMax < yMin ||
                other_.yMin > yMax
            ) {
                return false;
            }
            //端点是否在多边形内
            if (HitUtils.isPointInSector (other_.pos, other_.edgePo1, other_.edgePo2, other_.radius, _currentBegin)) {
                return true;
            }
            if (HitUtils.isPointInSector (other_.pos, other_.edgePo1, other_.edgePo2, other_.radius, _currentEnd)) {
                return true;
            }
            //直线是不是和任意一边相交
            if (
                HitUtils.isSegmentHitPolygon (
                    _currentBegin,
                    _currentEnd,
                    other_.polygonPos,
                    other_.polygonPosCount
                )
            ) {
                return true;
            }
            return false;
        }
    }
}