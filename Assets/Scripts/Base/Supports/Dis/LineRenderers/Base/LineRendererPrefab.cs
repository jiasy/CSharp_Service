using System;
using System.Collections.Generic;
using Objs;
using UnityEngine;
using Utils;

namespace Dis {
    //坐标轴
    public class LineRendererPrefab : MonoBehaviour {

        public LineRenderer lineRenderer;

        private Vector3[] paths = null;

        private IndexLoopObj _idxLoop = new IndexLoopObj ();

        private int _maxPointNum;

        //两段链接，无法制作虚线
        public int maxPointNum {
            get { return _maxPointNum; }
            set {
                paths = new Vector3[value];
                _idxLoop.reset (value);
                _maxPointNum = value;
            }
        }

        public void initByPrefab (string prefabPath_, int maxPointNum_) {
            GameObject _lineGO = ResUtils.duplicatePrefab (prefabPath_);
            DisplayUtils.addBToA (gameObject, _lineGO);
            lineRenderer = _lineGO.GetComponent<LineRenderer> ();
            maxPointNum = maxPointNum_; //设置长度
        }

        //添加一个节点<没有增删，只是通过索引来调整循环的起止位置>
        public void addV2 (Vector2 v2_, bool needConvertToWorldPos_ = true) {
            if (paths == null) {
                Debug.LogError ("ERROR " + System.Reflection.MethodBase.GetCurrentMethod ().ReflectedType.FullName + " -> " + new System.Diagnostics.StackTrace ().GetFrame (0).GetMethod ().Name + " : " +
                    "paths 为创建 ， 要先设置 maxPointNum"
                );
            }

            int _currentFirstIdx = _idxLoop.next ();
            //先记录位置
            if (needConvertToWorldPos_) {
                paths[_currentFirstIdx] = Camera.main.ScreenToWorldPoint (new Vector3 (v2_.x, v2_.y, 1)); //获取世界坐标值
            } else {
                paths[_currentFirstIdx] = new Vector3 (v2_.x, v2_.y, paths[_currentFirstIdx].z);
            }
            //当前使用点数量
            int _useCount = _idxLoop.currentUseCount;
            //当前使用了多少个点
            lineRenderer.positionCount = _useCount;

            //获取顺序
            int[] _currentIdxList = _idxLoop.getCurrentIdxList ();
            int _realIdx;
            for (int _idx = 0; _idx < _useCount; _idx++) {
                _realIdx = _currentIdxList[_idx];
                lineRenderer.SetPosition (_idx, paths[_realIdx]);
            }
        }
    }
}