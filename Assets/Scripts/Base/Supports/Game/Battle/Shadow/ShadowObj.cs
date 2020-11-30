using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using Utils;

namespace Objs {
    //万事万物的影子
    public class ShadowObj : BaseObj, IMovingObj, IUpdateAbleObj {

        private MoveControlObj _currentMoveControl = null;
        public MoveControlObj currentMoveControl {
            get { return _currentMoveControl; }
            set { _currentMoveControl = value; }
        }
        public ShadowObj () : base () {

        }
        public override void Dispose () {

            base.Dispose ();
        }

        //设置移动控制器
        public void resetMoveControl (MoveControlObj moveControl_) {
            currentMoveControl = moveControl_;
        }
        //通过移动控制器，更新自己的位置
        public void resetByPos () {
            //显示对象来根据当前位置更新显示
        }
        public void updateF () {
            resetByPos ();
        }
    }
}