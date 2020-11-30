using System;
using System.Collections.Generic;

namespace Objs {
    public interface IMovingObj {
        //可以移动的显示对象，或者数据对象，
        MoveControlObj currentMoveControl { get; set; }
        //设置移动控制器
        void resetMoveControl (MoveControlObj moveControl_);
        //都通过MoveAbleObj的pos来重置自己的pos
        void resetByPos();

    }
}