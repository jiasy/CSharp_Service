using System.Collections.Generic;
using UnityEngine;
using System;
using Utils;
using Objs;

namespace Dis {

    public class LineRendererSegment : LineRendererObj {
        private Vector3[] _v3s; //当前的绘画点缓存，长度固定，通过起始索引进行点绘画，添加点只是在做数据的索引偏移
        private IndexLoopObj _idxLoop = new IndexLoopObj ();
        private int _maxPointNum = 0; //点个数最大值

        public int maxPointNum {
            get {
                return _maxPointNum;
            }
            set {
                _v3s = new Vector3[value]; //原有数组销毁
                _idxLoop.reset(value);
                _maxPointNum = value;
            }
        }
        
        // public override void Start () {
        //     base.Start ();
        //     initByShader("Particles/Additive");//内置粒子使用的，叠加效果
        // }

        //重置线段对象
        public void reDraw (Vector2[] v2s_, bool needConvertToWorldPos_ = true) {
            //如果长度不同才需要重置。
            if (_maxPointNum != v2s_.Length) {
                maxPointNum = v2s_.Length;
            }
            for (int _idx = 0; _idx < v2s_.Length; _idx++) {
                addV2 (v2s_[_idx], needConvertToWorldPos_);
            }
        }
        public void resetPos (Vector2 pos_) {
            int _count = 0;
            while (_count < maxPointNum) {
                _count++;
                addV2(pos_);
            }
        }
        //添加一个节点<没有增删，只是通过索引来调整循环的起止位置>
        public void addV2 (Vector2 v2_, bool needConvertToWorldPos_ = true) {
            int _currentFirstIdx = _idxLoop.next ();
            if (needConvertToWorldPos_) {
                _v3s[_currentFirstIdx] = convertToWorldV3 (v2_); //获取世界坐标值
            } else {
                _v3s[_currentFirstIdx] = new Vector3 (v2_.x, v2_.y, _v3s[_currentFirstIdx].z);
            }

            lineRenderer.positionCount = _idxLoop.currentUseCount; //绘画段数
            int[] _currentIdxList = _idxLoop.getCurrentIdxList ();//顺序
            for (int _idx = 0; _idx < _idxLoop.currentUseCount; _idx++) {
                int _realIdx = _currentIdxList[_idx];
                lineRenderer.SetPosition (_idx, _v3s[_realIdx]);
            }
        }
    }
}