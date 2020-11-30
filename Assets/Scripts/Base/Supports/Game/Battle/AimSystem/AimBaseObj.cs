using System;
using System.Collections;
using System.Collections.Generic;
using Objs;
using UnityEngine;

namespace Game {
    //瞄准系统的基类，范围内索敌，判断形状区域内是否有目标
    public class AimBaseObj : BaseObj, IUpdateAbleObj {
        private MoveControlObj _currentTargetMC = null;
        private Shape2DObj _rangeShape2D = null;
        public AimBaseObj () : base () {
            
        }

        public override void Dispose () {
            base.Dispose ();
        }

        public virtual void updateF () {

        }

    }
}