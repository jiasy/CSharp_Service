using System;
using System.Collections;
using System.Collections.Generic;
using Objs;
using UnityEngine;

namespace Dis {
    public class ArrowTrajectory : LineRendererSegment, IMovingObj, IUpdateAbleObj ,IDisposable{

        private MoveControlObj _currentMoveControl = null;
        public MoveControlObj currentMoveControl {
            get { return _currentMoveControl; }
            set { _currentMoveControl = value; }
        }

        public ArrowTrajectory () {

        }

        public void initDefaultStyle () {
            initStyle ("Unlit/Texture", Color.white, Color.black, 0.04f, 0.01f); //无光照，更快
            maxPointNum = 10; //线段的个数
        }
        //设置移动控制器
        public void resetMoveControl (MoveControlObj moveControl_) {
            currentMoveControl = moveControl_;
        }
        public void resetAllV2ToMoveControl () {
            int _count = 0;
            while (_count < maxPointNum) { //轨迹的所有点都移动到同一个位置。这样在新位置起始的时候，不会有拖尾连线
                addV2 (_currentMoveControl.pos, _currentMoveControl.posType == MoveControlObj.PosType.UI);
                _count++;
            }
        }
        //通过移动控制器，更新自己的位置
        public void resetByPos () {
            addV2 (_currentMoveControl.pos, _currentMoveControl.posType == MoveControlObj.PosType.UI);
        }
        //更新方法
        public void updateF () {
            if (_currentMoveControl != null) {
                resetByPos ();
            }
        }
        public void Dispose()
        {
            //清理显示对象
            gameObject.transform.parent = null;
            _currentMoveControl.Dispose();
        }
    }
}