using System;
using System.Collections.Generic;

namespace Objs {
    public interface IPoolObj {
        //所在池对象
        PoolContainerObj belongToPool { get; set; }
        //所在运行时列表
        List<IPoolObj> belongToRunningList { get; set; }
        //是否正在运行
        bool isRunning { get; set; }
        //重新初始
        void reInit ();
    }
}