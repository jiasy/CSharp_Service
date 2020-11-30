using System;
using System.Collections;
using System.Collections.Generic;
using Objs;
using UnityEngine;
using Utils;

namespace Game {
    //重力作用下的物体
    public class GBaseObj : MoveControlObj {
        public bool isLand = true; //是否在地上
        public bool isOnIce = false; //是否在冰面上
        /*
            地面效果 ------
                0.5为正常衰减速度
                0.9为少量衰减[地面光滑] 
                0.2 - 0.3 大量衰减[泥泞等地面]
        */
        protected float friction = 0.5f;
        protected float iceFriction = 0.9f; //冰面上的摩擦力

        public float bounce = 0.5f; //落地的弹跳，0就是没有弹力，落地，即平稳。
        protected float bounceFriction = 0.5f; //落地的一瞬间，减少多少xy速度

        private Vector3 acc2 = Vector3.zero; //急动度 加速度的变化量
        public Vector3 acc = Vector3.zero; //加速度
        public Vector3 speed = Vector3.zero; //速度

        public float G = -3.0f; //重力
        public GBaseObj () : base (PosType.GAME) {

        }
        public override void Dispose () {
            base.Dispose ();
        }
        public override void updateF () {
            //加速度正常递减
            acc.x *= friction;
            acc.y *= friction;
            if (isLand) { //在地面上，速度进行摩擦削弱
                //速度在不同地面上，递减规则不同
                if (!isOnIce) { //不在冰面上
                    speed.x *= friction;
                    speed.y *= friction;
                    speed.x += acc.x;
                    speed.y += acc.y;
                } else { //在冰面上，急动度 生效
                    speed.x *= iceFriction;
                    speed.y *= iceFriction;
                    //冰上附加的急动度趋近于速度
                    acc2.x += (acc.x - acc2.x) * 0.05f;
                    acc2.y += (acc.y - acc2.y) * 0.05f;
                    float _tempSpeedX = acc2.x;
                    float _tempSpeedY = acc2.y;
                    if (_tempSpeedX < 0f) { _tempSpeedX = -_tempSpeedX; }
                    if (_tempSpeedY < 0f) { _tempSpeedY = -_tempSpeedY; }
                    if (_tempSpeedX > 40f) { _tempSpeedX = 40f; }
                    if (_tempSpeedY > 40f) { _tempSpeedY = 40f; }
                    speed.x += _tempSpeedX * (acc2.x > 0 ? 1 : -1);
                    speed.y += _tempSpeedY * (acc2.y > 0 ? 1 : -1);
                }
            } else { //空中，空气阻力就是摩擦力
                speed.x += acc.x;
                speed.y += acc.y;
            }

            // //速度灭掉
            // if (-0.0001f < speed.x && speed.x < 0.0001f) { speed.x = 0f; }
            // if (-0.0001f < speed.y && speed.y < 0.0001f) { speed.y = 0f; }

            //速度叠加到位置上
            realPos += speed;

            //是否落地的运算
            if (realPos.z > -G) { //浮空
                speed.z += G;
                isLand = false;
            } else {
                speed.z *= -bounce; //在某些地面弹力会很高
                if (speed.z < 1.0f && speed.z > 0.0f) { //回弹起来速度很小，认为它落地，向上为正
                    speed.z = 0.0f;
                    realPos.z = 0.0f;
                    isLand = true;
                } else { //落地的瞬间
                    realPos.z *= -bounce;
                    speed.x *= bounceFriction;
                    speed.y *= bounceFriction;
                }
            }

            base.updateF();
        }
    }
}