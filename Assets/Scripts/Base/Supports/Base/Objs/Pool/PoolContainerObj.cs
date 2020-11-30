using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Objs {
    public class PoolContainerObj : BaseObj {
        //维护某一个类的池
        public Queue<IPoolObj> _objs;

        public PoolContainerObj () : base () {
            //初始化对象池
            _objs = new Queue<IPoolObj> ();
        }

        //拉出一个类名对应的对象
        public IPoolObj pullOut (string fullClassName_, object[] parameters_ = null) {
            IPoolObj _obj = null;
            if (_objs.Count > 0) {
                _obj = _objs.Dequeue ();
            } else {
                _obj = (IPoolObj) TypeUtils.getObjectByClassName (fullClassName_, parameters_);
                _obj.belongToPool = this; //对象池创建出来的对象，都有所属池。
            }
            return _obj;
        }

        //放回一个对象
        public void pushBack (IPoolObj _obj) {
            _objs.Enqueue (_obj);
        }
        //是否包含对象
        public bool Contains (IPoolObj _obj) {
            return _objs.Contains (_obj);
        }
        //清理池内对象
        public void clearAll () {
            _objs.Clear ();
        }

        public override void Dispose () {
            clearAll ();
            base.Dispose ();
        }
    }
}