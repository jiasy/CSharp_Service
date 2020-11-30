using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Objs {
    //路径，Fixed 不受任何力影响
    public class FixedRouteBaseObj : MoveControlObj, IUpdateAbleObj {
        private Vector2[] _paths = null;
        protected int _pathLength;
        protected int _currentIdx = 0;

        public FixedRouteBaseObj (PosType posType_) : base (posType_) {

        }
        public override void Dispose () {
            base.Dispose ();
        }

        public virtual void resetPaths (Vector2[] paths_) {
            _paths = paths_;
            _pathLength = _paths.Length;
        }

        public override void updateF () {
            next();//更新当前所在序位
            updatePosByIndex(_currentIdx);//根据当前序位进行刷新
            base.updateF ();
        }
        public virtual void next(){
            if (_currentIdx < _pathLength) { //没有刷新结束就继续刷新
                _currentIdx++;
            }
        }
        public void updatePosByIndex (int currentIndx_) {
            Vector2 _currentV2 = _paths[currentIndx_];
            realPos.x = _currentV2.x;
            realPos.y = _currentV2.y;
        }
    }
}