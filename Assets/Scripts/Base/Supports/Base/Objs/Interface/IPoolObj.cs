using System;
using System.Collections.Generic;

namespace Objs {
    public interface IPoolObj {//可以放在对象池中的对象，可复用。
        PoolContainerObj belongToPool { get; set; }//所在池对象
        List<IPoolObj> belongToRunningList { get; set; }//所在运行时列表
        bool isRunning { get; set; }//是否正在运行
        void reInit ();//重新初始
    }
}