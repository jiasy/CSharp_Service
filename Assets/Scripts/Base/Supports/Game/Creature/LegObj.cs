using System;
using System.Collections;
using System.Collections.Generic;
using Objs;
using UnityEngine;
using Utils;

namespace Game {
    //移动相关的信息
    public class LegObj : GBaseObj {
        /*
            buff 叠加造成的速度变更
        */
        private float speedXS = 1.0f;
        private float rotation = 0.0f; //当前转向角
        private float vr = 0; //旋转速度
        private CreatureObj owner;
        public LegObj (CreatureObj owner_ = null) : base () {
            owner = owner_;
        }
        public override void Dispose () {
            owner = null;
            base.Dispose ();
        }
        public override void updateF () {
            rotation += vr; //旋转

            acc.x *= speedXS;
            acc.y *= speedXS;

            base.updateF ();

            speedXS = 1.0f;
        }
        
        //重置坐标
        public void resetToPos (float posX_, float posY_) {
            realPos = new Vector3 (posX_, posY_, 0); //更新下它的坐标。放置到这个随机点上
            updatePos (
                realPos.x,
                realPos.y,
                0
            ); //重置显示位置
        }
    }
}