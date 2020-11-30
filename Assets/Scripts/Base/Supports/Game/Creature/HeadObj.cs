using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using Objs;

namespace Game {

    public class HeadObj : BaseObj, IUpdateAbleObj {
        private CreatureObj owner;
        public HeadObj (CreatureObj owner_) : base () {
            owner = owner_;
        }
        public override void Dispose () {
            base.Dispose ();
        }
        public void updateF () {

        }
    }
}