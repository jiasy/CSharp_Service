using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Objs {
    public class BaseObj : IDisposable {
        public string fullClassName = null;
        public bool _disposed = false;
        private List<BaseObj> _belongToRunningList = null;
        public BaseObj () {
            //获取当前子类的类名
            fullClassName = GetType ().Namespace + "." + GetType ().Name;
            //添加到运行时，获取所属的运行时列表，放入其中
            _belongToRunningList = RunningUtils.getRunningList (fullClassName);
            _belongToRunningList.Add (this);
            //RunningUtils.runningInfo(true);
        }
        ~BaseObj () { //析构
            //即使没有手动调用过 Dispose，在GC回收的时候，还是会调用一次。
            Dispose (false);
        }
        public virtual void Dispose () {
            Dispose (true);
            GC.SuppressFinalize (this); //标记gc不在调用析构函数
        }
        //调用自己的销毁
        public void Dispose (bool disposing) {
            if (_disposed) return; //如果已经被回收，就中断执行
            if (disposing) { //释放本对象中管理的托管资源
                //由 手动调用Dispose 释放
            } else {
                //由 析构 释放
            }
            //释放非托管资源
            _belongToRunningList.Remove (this);
            fullClassName = null;
            _disposed = true;
        }
    }
}