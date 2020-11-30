using System;
using System.Collections;
using System.Collections.Generic;
using Objs;
using UnityEngine;

namespace Utils {
    public class HitUtils {

        public static void doSample () {

        }
        //两个线段碰撞
        public static bool isTwoSegmentHit (Vector2 begin1_, Vector2 end1_, Vector2 begin2_, Vector2 end2_) {
            //起点终点都在线的一侧，是不会相交的
            if ((EvaluatePointToLine (begin1_, end1_, begin2_.x, begin2_.y) > 0) == (EvaluatePointToLine (begin1_, end1_, end2_.x, end2_.y) > 0)) {
                return false;
            }
            //全都在一侧
            if ((EvaluatePointToLine (begin2_, end2_, begin1_.x, begin1_.y) > 0) == (EvaluatePointToLine (begin2_, end2_, end1_.x, end1_.y) > 0)) {
                return false;
            }
            return true;
        }
        //两个线段碰撞，其中一个水平
        public static bool isTwoSegmentHitHorizontal (Vector2 begin_, Vector2 end_, float y_, float xMin_, float xMax_) {
            if (begin_.y < y_ == end_.y < y_) {
                return false;
            }
            if ((EvaluatePointToLine (begin_, end_, xMin_, y_) > 0) == (EvaluatePointToLine (begin_, end_, xMax_, y_) > 0)) {
                return false;
            }
            return true;
        }
        //两个线段碰撞，其中一个竖直
        public static bool isTwoSegmentHitVertical (Vector2 begin_, Vector2 end_, float x_, float yMin_, float yMax_) {
            if (begin_.x < x_ == end_.x < x_) {
                return false;
            }
            if ((EvaluatePointToLine (begin_, end_, x_, yMin_) > 0) == (EvaluatePointToLine (begin_, end_, x_, yMax_) > 0)) {
                return false;
            }
            return true;
        }
        //线段碰撞多边形
        public static bool isSegmentHitPolygon (Vector2 begin_, Vector2 end_, Vector2[] polygonV2s_) {
            return isSegmentHitPolygon (begin_, end_, polygonV2s_);
        }
        public static bool isSegmentHitPolygon (Vector2 begin_, Vector2 end_, Vector2[] polygonV2s_, int polygonCount) {
            for (int _idx = 1; _idx < polygonCount; _idx++) {
                if (isTwoSegmentHit (begin_, end_, polygonV2s_[_idx - 1], polygonV2s_[_idx])) {
                    return true;
                }
            }
            //最后一个和第一个首尾相接
            if (isTwoSegmentHit (begin_, end_, polygonV2s_[polygonCount - 1], polygonV2s_[0])) {
                return true;
            }
            return false;
        }
        //rect 碰撞线段
        public static bool isRectHitSegment (Vector2 begin_, Vector2 end_, Rect rect_) {
            //两个点在方形内
            if (rect_.Contains (begin_) || rect_.Contains (end_)) {
                return true;
            }
            //两点组成的线段，和四条边是否相交
            if (
                isTwoSegmentHitHorizontal (begin_, end_, rect_.yMin, rect_.xMin, rect_.xMax) ||
                isTwoSegmentHitHorizontal (begin_, end_, rect_.yMax, rect_.xMin, rect_.xMax) ||
                isTwoSegmentHitVertical (begin_, end_, rect_.xMin, rect_.yMin, rect_.yMax) ||
                isTwoSegmentHitVertical (begin_, end_, rect_.xMax, rect_.yMin, rect_.yMax)
            ) {
                return true;
            }
            return false;
        }
        //rect 碰撞线段 线段为水平
        public static bool isRectHitSegmentHorizontal (Rect rect_, float y_, float xMin_, float xMax_) {
            if (xMin_ > rect_.xMax || xMax_ < rect_.xMin) {
                return false;
            }
            if (y_ < rect_.yMin || y_ > rect_.yMax) {
                return false;
            }
            return true;
        }
        //rect 碰撞线段 线段为竖直
        public static bool isRectHitSegmentVertical (Rect rect_, float x_, float yMin_, float yMax_) {
            if (yMin_ > rect_.yMax || yMax_ < rect_.yMin) {
                return false;
            }
            if (x_ < rect_.xMin || x_ > rect_.xMax) {
                return false;
            }
            return true;
        }

        // 判断 Po 点 和 有向直线P1P2的关系. 小于0表示点在直线左侧，等于0表示点在直线上，大于0表示点在直线右侧  
        public static float EvaluatePointToLine (Vector2 p1_, Vector2 p2_, float poX_, float poY_) {
            float _faceY = p2_.y - p1_.y;
            float _faceX = p1_.x - p2_.x;
            float _cross = p2_.x * p1_.y - p1_.x * p2_.y;
            return _faceY * poX_ + _faceX * poY_ + _cross;
        }
        //判断 圆 和 线段 碰撞
        public static bool isCircleHitSegment (Vector2 begin_, Vector2 end_, Vector2 circleCenter_, float radius_) {
            float _sqrDis = sqrDisPointToLine (begin_, end_, circleCenter_);
            if (_sqrDis < radius_ * radius_) {
                return true;
            }
            return false;
        }
        //判断 圆 和 线段 碰撞 ，线段水平
        public static bool isCircleHitSegmentHorizontal (Vector2 circleCenter_, float radius_, float y_, float xMin_, float xMax_) {
            float _yMin = circleCenter_.y - radius_;
            float _yMax = circleCenter_.y + radius_;
            if (y_ < _yMin || y_ > _yMax) {
                return false;
            }
            if (xMin_ > circleCenter_.x + radius_ || xMax_ < circleCenter_.x + radius_) {
                return false;
            }
            bool _faceMin = xMin_ > circleCenter_.x;
            bool _faceMax = xMax_ > circleCenter_.x;
            if (_faceMin == _faceMax) { //在x方向的一侧
                float _disY = y_ - circleCenter_.y;
                float _disX = 0.0f;
                if (_faceMin) { //两点在右面，判断 xMin 点是否在园内
                    _disX = xMin_ - circleCenter_.x;
                }
                if (_faceMax) { //两点在左面，判断 xMax 是否在园内
                    _disX = xMax_ - circleCenter_.x;
                }
                if (_disY * _disY + _disX * _disX < radius_ * radius_) {
                    return true;
                } else {
                    return false;
                }
            } else { //在横直径的两端
                return true;
            }
        }
        //判断 圆 和 线段 碰撞 ，线段竖直
        public static bool isCircleHitSegmentVertical (Vector2 circleCenter_, float radius_, float x_, float yMin_, float yMax_) {
            float _xMin = circleCenter_.x - radius_;
            float _xMax = circleCenter_.x + radius_;
            if (x_ < _xMin || x_ > _xMax) {
                return false;
            }
            if (yMin_ > circleCenter_.y + radius_ || yMax_ < circleCenter_.y + radius_) {
                return false;
            }
            bool _faceMin = yMin_ > circleCenter_.y;
            bool _faceMax = yMax_ > circleCenter_.y;
            if (_faceMin == _faceMax) { //在y方向的一侧
                float _disX = x_ - circleCenter_.x;
                float _disY = 0.0f;
                if (_faceMin) { //两点在上面，判断yMin点是否在园内
                    _disY = yMin_ - circleCenter_.y;
                }
                if (_faceMax) { //两点在下面，判断 yMax 是否在园内
                    _disY = yMax_ - circleCenter_.y;
                }
                if (_disX * _disX + _disY * _disY < radius_ * radius_) {
                    return true;
                } else {
                    return false;
                }
            } else { //在横直径的两端
                return true;
            }
        }

        //逆时针顺序的坐标点，点是否在多边形内部
        public static bool isPointInPolygon (Vector2 po_, Vector2[] polygonV2s_) {
            return isPointInPolygon (po_, polygonV2s_, polygonV2s_.Length);
        }
        public static bool isPointInPolygon (Vector2 po_, Vector2[] polygonV2s_, int polygonCount) {
            for (int _idx = 1; _idx < polygonCount; _idx++) {
                if (EvaluatePointToLine (polygonV2s_[_idx - 1], polygonV2s_[_idx], po_.x, po_.y) <= 0.0f) {
                    return false;
                }
            }
            //最后一个和第一个首尾相接
            if (EvaluatePointToLine (polygonV2s_[polygonCount - 1], polygonV2s_[0], po_.x, po_.y) <= 0.0f) {
                return false;
            }
            return true;
        }
        //点是否在夹角内,begin-o,end-o 两条矢量内
        //begin-o ，的角度要小于 end-o;
        public static bool isPointInVectorAngle (Vector2 o_, Vector2 begin_, Vector2 end_, Vector2 po_) {
            if (EvaluatePointToLine (o_, begin_, po_.x, po_.y) > 0.0f) { //右
                if (EvaluatePointToLine (o_, end_, po_.x, po_.y) < 0.0f) { //左
                    return true;
                }
            }
            return false;
        }
        //点是否在扇形内
        public static bool isPointInSector (Vector2 o_, Vector2 begin_, Vector2 end_, float radius_, Vector2 po_) {
            if (isPointInVectorAngle (o_, begin_, end_, po_)) { //在两边之间
                if (Vector2.SqrMagnitude (o_ - po_) < radius_ * radius_) { //距离在圆内
                    return true;
                }
            }
            return false;
        }
        public static bool isCircleHitSector (Vector2 o_, Vector2 begin_, Vector2 end_, float radius_, Vector2 circleCenter_, float circleRadius_) {
            if (isPointInVectorAngle (o_, begin_, end_, circleCenter_)) { //在两边之间
                if (Vector2.SqrMagnitude (o_ - circleCenter_) < (radius_ + circleRadius_) * (radius_ + circleRadius_)) { //距离在圆内
                    return true;
                }
            } else {
                //和两个边相交
                if (
                    HitUtils.isCircleHitSegment (o_, begin_, circleCenter_, circleRadius_) ||
                    HitUtils.isCircleHitSegment (o_, end_, circleCenter_, circleRadius_)
                ) {
                    return true;
                }
            }
            return false;
        }

        /************************************************计算点到线段的距离**************************************************
                             /P                                          /P                           P\
                            /                                       　　/                              　\
                           /                                       　　/                               　 \
                          /                                          /                                    \
                        A ----C-------B                    A--------B   C                           C     A ----------B

        长度：                        CP                                BP                                        AP
        计算：d = dot(AP,AB)/|AB|^2
        判断依据：                    if(0<d<1)                    if(d>1)                                if(d<0)
        ************************************************计算点到线段的距离**************************************************/
        public static float sqrDisPointToLine (
            Vector2 A_,
            Vector2 B_,
            Vector2 P_
        ) {
            float pqx = B_.x - A_.x;
            float pqy = B_.y - A_.y;
            //float pqz = q.z - p.z;
            float dx = P_.x - A_.x;
            float dy = P_.y - A_.y;
            //float dz = pt.z - p.z;
            float d = pqx * pqx + pqy * pqy; // + pqz*pqz;      // qp线段长度的平方
            float t = pqx * dx + pqy * dy; // + pqz*dz;         // p pt向量 点积 pq 向量（ begin_ 相当于A点， end_ 相当于B点，pt相当于P点）
            if (d > 0)          // 除数不能为0; 如果为零 t应该也为零。下面计算结果仍然成立。                   
                t /= d;     // 此时t 相当于 上述推导中的 r。
            if (t < 0)
                t = 0;      // 当t（r）< 0时，最短距离即为 pt点 和 begin_ 点（A点和P点）之间的距离。
            else if (t > 1)
                t = 1;      // 当t（r）> 1时，最短距离即为 pt点 和 end_ 点（B点和P点）之间的距离。

            // t = 0，计算 pt点 和 p点的距离; t = 1, 计算 pt点 和 q点 的距离; 否则计算 pt点 和 投影点 的距离。
            dx = A_.x + t * pqx - P_.x;
            dy = A_.y + t * pqy - P_.y;
            //dz = p.z + t*pqz - pt.z;
            return dx * dx + dy * dy; // + dz*dz;
        }

        // ------------------------------直线与圆相交求交点--------------------------------------------
        /**
         * 求圆和直线之间的交点
         * 直线方程：y = kx + b
         * 圆的方程：(x - m)² + (x - n)² = r²
         * x1, y1 = 线坐标1, x2, y2 = 线坐标2, m, n = 圆坐标, r = 半径
         */
        public static List<Vector2> getInsertPointBetweenCircleAndLine (float x1, float y1, float x2, float y2, float m, float n, float r) {
            List<float> kbArr = binaryEquationGetKB (x1, y1, x2, y2);
            float k = kbArr[0];
            float b = kbArr[1];
            float aX = 1 + k * k;
            float bX = 2 * k * (b - n) - 2 * m;
            float cX = m * m + (b - n) * (b - n) - r * r;

            List<Vector2> insertPoints = new List<Vector2> ();
            List<float> xArr = quadEquationGetX (aX, bX, cX);
            int _idx = 0;
            int _length = xArr.Count;
            float _x;
            for (_idx = 0; _idx < _length; _idx++) {
                _x = xArr[_idx];
                float y = k * _x + b;
                insertPoints.Add (new Vector2 (_x, y));
            }
            return insertPoints;
        } 
        /**
         * 求二元一次方程的系数
         * y1 = k * x1 + b => k = (y1 - b) / x1
         * y2 = k * x2 + b => y2 = ((y1 - b) / x1) * x2 + b
         */
        private static List<float> binaryEquationGetKB (float x1, float y1, float x2, float y2) {
            float k = (y1 - y2) / (x1 - x2);
            float b = (x1 * y2 - x2 * y1) / (x1 - x2);
            return new List<float> () { k, b };
        }

        /**
         * 一元二次方程求根
         * ax² + bx + c = 0
         */
        private static List<float> quadEquationGetX (float a, float b, float c) {
            List<float> xArr = new List<float> ();
            float result = Mathf.Pow (b, 2) - 4 * a * c;
            if (result > 0) {
                xArr.Add ((-b + Mathf.Sqrt (result)) / (2 * a));
                xArr.Add ((-b - Mathf.Sqrt (result)) / (2 * a));
            } else if (result == 0) {
                xArr.Add (-b / (2 * a));
            }
            return xArr;
        }
    }
}