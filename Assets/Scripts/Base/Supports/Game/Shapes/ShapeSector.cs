using System;
using System.Collections;
using System.Collections.Generic;
using Objs;
using UnityEngine;
using Utils;

namespace Game {
    //碰撞形状
    public class ShapeSector : Shape2DObj {
        public float radius; //半径
        public float inRadius = 0.0f; //半径
        public float angleRange; //角度范围
        public float direction; //朝向
        public Vector2 directionVector2; //朝向 和 半径构成的矢量
        public Vector2 edgePo1;
        public Vector2 edgePo2;
        //edge1 + 5分圆周，每36度一个，最多 4 个点 + edge2 + pos，所以最多7个点
        public Vector2[] polygonPos = new Vector2[8];
        public int polygonPosCount = 0; //用到几个圆周点

        public float xMin = Mathf.Infinity;
        public float xMax = -Mathf.Infinity;
        public float yMin = Mathf.Infinity;
        public float yMax = -Mathf.Infinity;

        public ShapeSector (Vector2 pos_, float direction_, float angleRange_, float radius_) : base (pos_) {
            type = ShapeType.Sector;
            if (angleRange_ >= 180) {
                Debug.LogError ("ERROR 扇形的角度范围不要超过180度");
            }
            reset (pos_.x, pos_.y, direction_, angleRange_, radius_);
        }
        public void reset (float x_, float y_, float direction_, float angleRange_, float radius_) {
            resetParams (direction_, angleRange_, radius_);
            resetPos (x_, y_);
        }
        public void resetPos (float x_, float y_) {
            if (changePos (x_, y_)) {
                resetDetail ();
            }
        }
        public void resetParams (float direction_, float angleRange_, float radius_) {
            if (
                direction != direction_ ||
                angleRange != angleRange_ ||
                radius != radius_
            ) {
                radius = radius_;
                direction = direction_;
                angleRange = angleRange_;
                resetDetail ();
            }
        }
        public void resetDetail () {

            //单位向量变为指向向量，然后旋转得到两侧的向量
            directionVector2 = LineUtils.getV2ByAngleAndLength (direction, radius);
            float angleRange2 = angleRange * 0.5f;
            Vector2 _localEdgePo1 = LineUtils.rotation (directionVector2, -angleRange2);
            //旋转过后偏移到当前的位置。
            edgePo1 = _localEdgePo1 + pos;
            edgePo2 = LineUtils.rotation (directionVector2, angleRange2) + pos;
            //近似多边形，edgePo2 -> pos ->edgePo1 -> cruvePos[0] -> ... -> cruvePos[cruveUseCount-1] -> edgePo2
            polygonPos[0] = edgePo2;
            resetXYMinMax (edgePo2);
            polygonPos[1] = pos;
            resetXYMinMax (pos);
            polygonPos[2] = edgePo1;
            resetXYMinMax (edgePo1);
            //分n份的话，就是n-1个圆周点可用，因为，edge1 或者 edge2 会占用一个。
            int cruveUseCount = Mathf.FloorToInt (angleRange / 36.0f); //圆弧上有几个点
            //这样，pos -> edgePo1 -> cruvePos[0]-> ... -> cruvePos[cruveUseCount] -> edgePo2 -> pos 就构成了一个多边形
            float _cruvePerAngle = angleRange / (float) (cruveUseCount + 1); //每两点间隔多少度
            Vector2 _tempV2;
            //将一个半圆以内的扇形，分割成几个36度之内的三角形拼接起来的多边形。
            for (int _i = 0; _i < cruveUseCount; _i++) {
                _tempV2 = polygonPos[3 + _i] = LineUtils.rotation (_localEdgePo1, _cruvePerAngle * (_i + 1)) + pos;
                resetXYMinMax (_tempV2);
            }
            //圆周上没有点，最少也要三个点 edge1，edge2，pos
            polygonPosCount = cruveUseCount + 3;
        }

        public void resetXYMinMax (Vector2 po_) {
            xMax = getBigger (po_.x, xMax);
            xMin = getSmaller (po_.x, xMin);
            yMax = getBigger (po_.y, yMax);
            yMin = getSmaller (po_.y, yMin);
        }

        public float getBigger (float a_, float b_) {
            if (a_ > b_) {
                return a_;
            }
            return b_;
        }
        public float getSmaller (float a_, float b_) {
            if (a_ < b_) {
                return a_;
            }
            return b_;
        }

        public override void Dispose () {
            base.Dispose ();
        }
        //碰撞值
        public override bool hitV2 (Vector2 targetV2_) {
            return HitUtils.isPointInPolygon (
                targetV2_,
                polygonPos,
                polygonPosCount
            );
        }
        public override bool hitOther (ShapePoint other_) {
            return HitUtils.isPointInPolygon (
                other_.pos,
                polygonPos,
                polygonPosCount
            );
        }
        public override bool hitOther (ShapeLine other_) {
            return other_.hitOther (this);
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
            //把方形直接当做圆形判断
            return HitUtils.isCircleHitSector (
                pos,
                edgePo1,
                edgePo2,
                radius,
                other_.pos,
                other_.side2
            );
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
            //Debug.LogError ("ERROR Rect <-> Sector 没实现过");
            return false;
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
            return HitUtils.isCircleHitSector (
                pos,
                edgePo1,
                edgePo2,
                radius,
                other_.pos,
                other_.radius
            );
        }
        public override bool hitOther (ShapeSector other_) {
            //Debug.LogError ("ERROR Sector <-> Sector 没实现过");
            return false;
        }
    }
}