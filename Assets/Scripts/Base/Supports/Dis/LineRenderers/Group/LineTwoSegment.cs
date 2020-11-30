using System;
using System.Collections.Generic;
using Objs;
using UnityEngine;
using Utils;

namespace Dis {
    //坐标轴
    public class LineTwoSegment : MonoBehaviour {

        private LineRendererSegment _line1;
        private LineRendererSegment _line2;

        public string usingShaderPath = "Sprites/Default"; //内置的Shader,白色实体

        private Vector3[] paths;

        private IndexLoopObj _idxLoop = new IndexLoopObj ();

        //两段链接，无法制作虚线
        public int maxPointNum {
            get { return (_line1.maxPointNum + _line2.maxPointNum); }
            set {
                paths = new Vector3[value];
                _idxLoop.reset (value);
                //前后分段要随着节点数调整，保持前面只有一小段，后面有长拖尾。
                int _fen = value / 10 + 1;
                //除不开的，line2 加上
                int _every = Mathf.FloorToInt (value / (float) _fen);
                _line1.maxPointNum = _every * (_fen - 1) + (value % _fen);
                _line2.maxPointNum = _every + 1; //向前多画一截
            }
        }
        //绘画黄金分割
        public void Start () {
            _line1 = CreateUtils.getComponentAndAddTo (typeof (LineRendererSegment), this) as LineRendererSegment;
            _line1.initStyle (usingShaderPath, Color.yellow, Color.green, 0.03f, 0.005f); //用相同的Shader
            _line1.gameObject.name = "line1";

            _line2 = CreateUtils.getComponentAndAddTo (typeof (LineRendererSegment), this) as LineRendererSegment;
            _line2.initStyle (usingShaderPath, Color.yellow, Color.red, 0.03f, 0.005f); //用相同的Shader
            _line2.gameObject.name = "line2";

            //设置长度
            maxPointNum = 30;
        }
        protected void Update () {
            if (Input.GetMouseButton (0)) { //鼠标移动
                addV2 (Input.mousePosition);
            }
        }

        //添加一个节点<没有增删，只是通过索引来调整循环的起止位置>
        public void addV2 (Vector2 v2_, bool needConvertToWorldPos_ = true) {
            int _currentFirstIdx = _idxLoop.next ();
            //先记录位置
            if (needConvertToWorldPos_) {
                paths[_currentFirstIdx] = Camera.main.ScreenToWorldPoint (new Vector3 (v2_.x, v2_.y, 1)); //获取世界坐标值
            } else {
                paths[_currentFirstIdx] = new Vector3 (v2_.x, v2_.y, paths[_currentFirstIdx].z);
            }
            int _line1MaxNum = _line1.maxPointNum;
            int _useCount = _idxLoop.currentUseCount;

            //一个不够画，就启用第二个
            if (_useCount > _line1MaxNum) {
                _line1.lineRenderer.positionCount = _line1MaxNum;
                _line2.lineRenderer.positionCount = (_useCount - _line1MaxNum + 1); //向前多画一截
            } else {
                _line1.lineRenderer.positionCount = _useCount;
                _line2.lineRenderer.positionCount = 0;
            }
            //获取顺序
            int[] _currentIdxList = _idxLoop.getCurrentIdxList ();
            int _realIdx;
            int _lastIdx = 0;
            for (int _idx = 0; _idx < _useCount; _idx++) {
                _realIdx = _currentIdxList[_idx];
                if (_idx < _line1MaxNum) { //一个不够画，再第二个上接着画
                    _line1.lineRenderer.SetPosition (_idx, paths[_realIdx]);
                } else {
                    if (_idx == _line1MaxNum) { //向前多画一截
                        _line2.lineRenderer.SetPosition (_idx - _line1MaxNum, paths[_lastIdx]);
                    }
                    _line2.lineRenderer.SetPosition (_idx - _line1MaxNum + 1, paths[_realIdx]);
                }
                _lastIdx = _realIdx;
            }
        }
    }
}