using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using Objs;

namespace Game {
    //技能，持有道具，武器等等
    public class HandObj : BaseObj, IUpdateAbleObj {
        private CreatureObj owner;
        public HandObj (CreatureObj owner_) : base () {
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