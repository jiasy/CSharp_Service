using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using Objs;

namespace Game {

    public class BagObj : BaseObj, IUpdateAbleObj {
        private CreatureObj owner;
        public BagObj (CreatureObj owner_) : base () {
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