using System;
using System.Collections;
using System.Collections.Generic;
using Objs;
using UnityEngine;
using Utils;

namespace Game {
    //当一个物体是可形状化的，那么它的形状，可以是动态变化的。所以，使用的形状可以设置。
    //当然，它也可以是一个形状集合拼接而成的[特殊形状拆分成形状数组]
    public class ShapefulObj : BaseObj {
        //当前正在使用的 shape
        private Shape2DObj _currentShape = null;

        public ShapefulObj () : base () {

        }
        public override void Dispose () {

            base.Dispose ();
        }
        //重置自己的形状 
        public void resetShape (Shape2DObj shape_) {
            _currentShape = shape_;
        }
    }
}