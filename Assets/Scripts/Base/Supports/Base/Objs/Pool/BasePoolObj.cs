using System;
using System.Collections.Generic;
using UnityEngine;

namespace Objs {
    public class BasePoolObj : BaseObj, IPoolObj {
        //隶属于那个池
        private PoolContainerObj _belongToPool;
        public PoolContainerObj belongToPool {
            get { return _belongToPool; }
            set { _belongToPool = value; }
        }
        //隶属于那个运行时
        private List<IPoolObj> _belongToRunningList;
        public List<IPoolObj> belongToRunningList {
            get { return _belongToRunningList; }
            set { _belongToRunningList = value; }
        }
        //创建出来的时候，直接就拿来用，所以，一开始是在运行中
        private bool _isRunning = false;
        public bool isRunning {
            get { return _isRunning; }
            set { _isRunning = value; }
        }
        public BasePoolObj () {
            if (isRunning) {
                Debug.LogError (fullClassName + " -> BasePoolObj -> isRunning " + isRunning.ToString () + " 初始化时，只能是 false");
                return;
            }
        }
        //刚创建 或者 从池内拿出来 。都要进行一次初始化
        public void reInit () {
            if (isRunning) {
                Debug.LogError (fullClassName + " -> reCreate -> isRunning " + isRunning.ToString () + " 正在运行的对象，无法再次初始化");
                return;
            }
            //已经初始化结束，进入运行状态
            isRunning = true;
        }

        public override void Dispose () {
            base.Dispose ();
        }
    }
}