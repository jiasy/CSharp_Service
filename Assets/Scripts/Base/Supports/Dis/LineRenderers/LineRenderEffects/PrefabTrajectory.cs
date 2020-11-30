using System;
using System.Collections.Generic;
using Objs;
using UnityEngine;
using Utils;

namespace Dis {

    public class PrefabTrajectory : LineRendererPrefab, IMovingObj, IUpdateAbleObj,IDisposable {

        private MoveControlObj _currentMoveControl = null;
        public MoveControlObj currentMoveControl {
            get { return _currentMoveControl; }
            set { _currentMoveControl = value; }
        }
        public void Start () {
            initByPrefab ("Prefabs/Lines/ColorLineRenderer", 10);
            //lineRenderer.sortingLayerName = "Ground";
        }

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