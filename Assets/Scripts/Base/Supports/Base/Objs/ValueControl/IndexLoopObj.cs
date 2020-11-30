using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Objs {
    // 固定长数组，循环使用其中内容。为了重新利用空间，进行可用索引的循环利用
    public class IndexLoopObj : BaseObj {
        public int maxCapacity = 0;//点个数最大值
        public int currentUseCount;//多少个点有用

        private int _startIdx;//逻辑起点序号
        private int _endIdx;//逻辑终点序号
        private int _nextTimeCount = -1;//用来索引端点
        private int _currentFirstIdx = -1;//当前索引偏移的位置
        private int[] _currentIdxList;//顺序如何。

        public static void doSample () {
            //循环利用一个数组
            int _currentFirstIdx;
            //三个长，加四个元素会循环到第一个，next() 每执行一次，结果如下。
            // _posList中当前值       _idxLoop.getCurrentIdxList()    _idxLoop.currentUseCount  _currentFirstIdx
            // [(0,0)]               0                               1                         0
            // [(0,0),(1,1)]         0,1                             2                         1
            // [(0,0),(1,1),(2,2)]   0,1,2                           3                         2
            // [(3,3),(1,1),(2,2)]   1,2,0                           3                         0
            int _length = 3;
            Vector2[] _posList = new Vector2[_length]; // 点数组
            IndexLoopObj _idxLoop = new IndexLoopObj (); // 索引操控
            _idxLoop.reset (_length); // 同步数组长度

            //添加一个以后位置是哪里
            _currentFirstIdx = _idxLoop.next();//获取第一个可用索引
            _posList[_currentFirstIdx].x = 0;
            _posList[_currentFirstIdx].y = 0;

            _currentFirstIdx = _idxLoop.next();
            _posList[_currentFirstIdx].x = 1;
            _posList[_currentFirstIdx].y = 1;

            _currentFirstIdx = _idxLoop.next();
            _posList[_currentFirstIdx].x = 2;
            _posList[_currentFirstIdx].y = 2;

            _currentFirstIdx = _idxLoop.next();
            _posList[_currentFirstIdx].x = 3;
            _posList[_currentFirstIdx].y = 3;

            int[] _currentIdxList = _idxLoop.getCurrentIdxList ();//当前位置下向后列表顺序
            for (int _idx = 0; _idx < _idxLoop.currentUseCount; _idx++) {//循环当前已经使用的个数
                int _realIdx = _currentIdxList[_idx];//数组需要需要重新映射
                Vector2 _v2 = _posList[_realIdx];//获取实际的那个元素
                Debug.Log ("_v2.x : " + _v2.x.ToString ());
                Debug.Log ("_v2.y : " + _v2.y.ToString ());
            }
        }

        public IndexLoopObj () : base () {

        }
        public override void Dispose () {
            base.Dispose ();
        }
        public void reset (int maxPointNum_) {
            _nextTimeCount = -1;//重置当前 next 执行次数
            _currentFirstIdx = -1;
            maxCapacity = maxPointNum_;
            _currentIdxList = new int[maxCapacity];
        }
        //向下进行一次元素添加，返回往哪个序号添加
        public int next () {
            _currentFirstIdx++;
            if (_currentFirstIdx == maxCapacity) {//到头就回到第一个序号
                _currentFirstIdx = 0;
            }
            _nextTimeCount++;//进行 next 的次数。
            if (_nextTimeCount >= maxCapacity) {//进行的次数超过一个循环
                currentUseCount = maxCapacity; //画线节点数等于最大数
                //_startIdx 为当前最后一个，那么当超过一个循环之后， _startIdx 的下一个就是起始位置
                _startIdx = _currentFirstIdx + 1;
                _endIdx = _currentFirstIdx;
                if (_startIdx == maxCapacity) { //如果是最大长，证明，下一个节点的坐标是第一个点
                    _startIdx = 0;
                    _endIdx = maxCapacity - 1;
                }
            } else {
                currentUseCount = _nextTimeCount + 1;
                _startIdx = 0;
                _endIdx = _nextTimeCount;
            }
            return _currentFirstIdx;
        }
        //计算，当前状态下，从第一个到最后一个，使用的是那些序位上的元素
        public int[] getCurrentIdxList () {
            int _realDrawIdx = 0;
            if (_endIdx >= _startIdx) { //从头找第一个坐标到，当前结束位置
                for (int _idx = _startIdx; _idx <= _endIdx; _idx++) {
                    _currentIdxList[_realDrawIdx] = _idx;
                    _realDrawIdx++;
                }
            } else {//向后到头，然后再从起点继续记录到终止位置。
                for (int _idx = _startIdx; _idx < maxCapacity; _idx++) {//当前位置的下一个起始到结束
                    _currentIdxList[_realDrawIdx] = _idx;
                    _realDrawIdx++;
                }
                for (int _idx = 0; _idx <= _endIdx; _idx++) {//从头找第一个坐标到，当前结束位置
                    _currentIdxList[_realDrawIdx] = _idx;
                    _realDrawIdx++;
                }
            }
            return _currentIdxList;
        }
    }
}