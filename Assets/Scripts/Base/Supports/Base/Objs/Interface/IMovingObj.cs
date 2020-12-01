using System;
using System.Collections.Generic;

namespace Objs {
    public interface IMovingObj {
        MoveControlObj currentMoveControl { get; set; }//可以移动的对象
        void resetMoveControl (MoveControlObj moveControl_);//设置移动控制器
        void resetByPos();//都通过MoveAbleObj的pos来重置自己的pos
    }
}