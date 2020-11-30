using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Objs {
    //循环路径，Fixed 不受任何力影响
    public class FixedRouteLoopObj : FixedRouteBaseObj {
        //对于持有的路径数组，进行循环使用
        private IndexLoopObj _idxLoop = new IndexLoopObj ();

        public static void doSample () {
            FixedRouteLoopObj _fixedRouteLoopObj = new FixedRouteLoopObj (PosType.UI);
            Vector2[] _paths = new Vector2[] {
                new Vector2 (-1.0f, -1.0f),
                new Vector2 (-1.0f, 1.0f),
                new Vector2 (1.0f, 1.0f),
                new Vector2 (1.0f, -1.0f)
            };
            _fixedRouteLoopObj.resetPaths (_paths);
            _fixedRouteLoopObj.updateF ();//移动物体能力对象，必须实现帧更新方法。
        }

        public FixedRouteLoopObj (PosType posType_) : base (posType_) {

        }
        public override void Dispose () {
            base.Dispose ();
        }

        //重置固定路径
        public override void resetPaths (Vector2[] paths_) {
            base.resetPaths (paths_);
            _idxLoop.reset (_pathLength);
        }
        //向前移动多少个点
        public void bufferNext (int times_) {
            if (times_ >= _idxLoop.maxCapacity) {
                Debug.LogError ("ERROR FixedRouteLoopObj -> bufferNext times_ : " + times_.ToString () + " > " + _idxLoop.maxCapacity.ToString () + " 超长了");
            }
            int _count = 0;
            while (_count < times_) {
                _idxLoop.next ();
                _count++;
            }
        }
        //调用刷新
        public override void updateF () {
            base.updateF ();
        }
        //变更获取下一个序号的方式。
        public override void next () {
            _currentIdx = _idxLoop.next ();
        }
    }
}