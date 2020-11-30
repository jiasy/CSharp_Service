using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Objs {

    //移动物体
    public class MoveControlObj : BaseObj, IUpdateAbleObj {
        //public const float sightY = 0.5f; //斜视45视角
        //public const float sightY = 0.75f; //斜视高角度。
        public const float sightY = 1.0f; //正视视角

        public float direction = 0.0f; //面相目标的转向角度
        public float rotateLimit = float.MaxValue; //最大值，也就是转角无限制
        public enum PosType {
            GAME,
            UI
        }
        //当前的位置，用Vector2来存放，作为参数直接段递给其他使用者
        public Vector2 pos = Vector2.zero;
        //上一帧的位置，没有使用 Vector2 来存放，只是单纯的纯数值
        public Vector3 lastPos = Vector3.zero;

        //没有发生位移的帧数累计
        public int noMoveFrameCount = 0;
        public int moveFrameCount = 0;

        public Vector3 realPos = Vector3.zero; //实际坐标

        public PosType posType;

        public MoveControlObj (PosType posType_) : base () {
            posType = posType_;
        }
        //重置坐标，和每一帧重新设置坐标不同，这里的坐标设置是跳跃的，有重置的意思
        public void resetRealPos (Vector2 realPos_) {
            realPos.x = realPos_.x;
            realPos.y = realPos_.y;
            noMoveFrameCount = 0;
            moveFrameCount = 0;
        }
        public override void Dispose () {
            base.Dispose ();
        }

        //是否世界坐标的转化， 不要在这里进行，因为这个moveControl，可能是UI使用，也可能是Game使用。
        public void updatePos (
            float posX_,
            float posY_,
            float posZ_ = 0.0f
        ) {
            //新位置
            Vector3 _currentRealPos = new Vector3 (posX_, posY_, posZ_);
            if (
                (lastPos - _currentRealPos).sqrMagnitude < 0.5f
            ) {
                noMoveFrameCount++; //不动帧自增
                moveFrameCount = 0; //已经不动就清零
                return;

            }
            //记录成上一帧
            lastPos = _currentRealPos;
            noMoveFrameCount = 0;
            moveFrameCount++;

            //坐标转换
            if (posType == PosType.GAME) {
                posX_ *= 0.01f;
                posY_ *= 0.01f;
                posZ_ *= 0.01f;
            }

            //有视角的情况下， Y值是要乘以系数的
            float _newPoxY = posY_ * sightY + posZ_;
            //想平面转化，最终使用的还是 pos 做为2D位置显示依据。
            pos.x = posX_;
            pos.y = _newPoxY;
        }

        public virtual void updateF () {
            //位置向实际坐标进行一次转换
            updatePos (
                realPos.x,
                realPos.y,
                realPos.z
            );
        }
    }
}