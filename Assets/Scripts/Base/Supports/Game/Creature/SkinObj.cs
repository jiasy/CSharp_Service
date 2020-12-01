using System;
using System.Collections;
using System.Collections.Generic;
using Objs;
using UnityEngine;
using Utils;

namespace Game {
    //状态化的对象，对显示持有控制权
    public class SkinObj : StatefulObj, IUpdateAbleObj {
        private CreatureObj owner;
        private DisplayControlObj _disControl;
        public SkinObj (CreatureObj owner_) : base () {
            owner = owner_;
        }
        public override void Dispose () {
            owner = null;
            base.Dispose ();
        }
        public void updateF () {

        }
    }
}