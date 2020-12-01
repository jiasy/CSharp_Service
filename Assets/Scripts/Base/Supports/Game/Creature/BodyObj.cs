using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using Objs;

namespace Game {
    //buff堆叠，持有护甲等等
    //持有形状
    public class BodyObj : ShapefulObj, IUpdateAbleObj {
        private CreatureObj owner;
        
        public BodyObj (CreatureObj owner_) : base () {
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