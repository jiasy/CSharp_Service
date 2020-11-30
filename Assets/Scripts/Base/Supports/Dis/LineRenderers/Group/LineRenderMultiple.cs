using System;
using System.Collections.Generic;
using Objs;
using UnityEngine;
using Utils;

namespace Dis {
    //多个 Segment 的链接
    public class LineRenderMultiple : MonoBehaviour {

        private LineRendererSegment[] _lines;

        private Vector3[] paths;

        private IndexLoopObj _idxLoop = new IndexLoopObj ();

        private int perCount;

        private bool isConnect = true; //相邻两个连接上

        public string usingShaderPath = null; //内置的Shader,白色实体


        // public void Start () {
        //     // //激光带闪懂效果
        //     //preset_Laser_Color_Shan("Particles/Additive");
        //     // //激光不闪
        //     //preset_Laser_Color("Particles/Additive");
        //     // //勇度的追踪箭效果
        //     //preset_Light_YongDu("Sprites/Default");
        //     //preset_Light_YongDu_Colorful("Sprites/Default");
        //     // //虚线标示抛物线
        //     // preset_Dotted_line ("Sprites/Default");
        // }

        public void preset_Dotted_line (string shader_) {
            usingShaderPath = shader_;
            float[] _widthList = {
                0.001f,
                0.01f,
                0.02f,
                0.016f,
                0.012f,
                0.008f,
                0.004f,
                0.0001f
            };
            _widthList = NumUtils.multiple (_widthList, 2.0f);
            _widthList = NumUtils.reverse (_widthList);

            Color[] _colorList = {
                Color.white,
                Color.white,
                Color.white,
                Color.white,
                Color.white,
                Color.white,
                Color.white,
                Color.white,
            };

            reset (_widthList, _colorList, 2, false);
        }
        public void preset_Light_YongDu (string shader_) {
            usingShaderPath = shader_;
            float[] _widthList = { 0.01f, 0.02f, 0.05f, 0.032f, 0.024f, 0.02f, 0.016f, 0.014f, 0.012f, 0.008f, 0.006f, 0.004f, 0.002f, 0.0001f };
            _widthList = NumUtils.reverse (_widthList);
            Color[] _colorList = { Color.red, Color.red, Color.red, Color.red, Color.red, Color.red, Color.red, Color.red, Color.red, Color.red, Color.red, Color.red, Color.red, Color.red, Color.red, Color.red };
            //Color[] _colorList = { Color.red, Color.yellow, Color.red, Color.yellow, Color.red, Color.yellow, Color.red, Color.yellow,Color.red, Color.yellow, Color.red, Color.yellow, Color.red, Color.yellow, Color.red, Color.yellow };
            //Color[] _colorList = { Color.red, Color.yellow, Color.green, Color.blue, Color.magenta, Color.grey, Color.cyan, Color.white,Color.red, Color.yellow, Color.green, Color.blue, Color.magenta, Color.grey, Color.cyan, Color.white };
            reset (_widthList, _colorList, 1);
        }
        public void preset_Light_YongDu_Colorful (string shader_) {
            usingShaderPath = shader_;
            float[] _widthList = { 0.01f, 0.02f, 0.05f, 0.032f, 0.024f, 0.02f, 0.016f, 0.014f, 0.012f, 0.008f, 0.006f, 0.004f, 0.002f, 0.0001f };
            _widthList = NumUtils.reverse (_widthList);
            //Color[] _colorList = { Color.red, Color.red, Color.red, Color.red, Color.red, Color.red, Color.red, Color.red, Color.red, Color.red, Color.red, Color.red, Color.red, Color.red, Color.red, Color.red };
            //Color[] _colorList = { Color.red, Color.yellow, Color.red, Color.yellow, Color.red, Color.yellow, Color.red, Color.yellow,Color.red, Color.yellow, Color.red, Color.yellow, Color.red, Color.yellow, Color.red, Color.yellow };
            Color[] _colorList = { Color.red, Color.yellow, Color.green, Color.blue, Color.magenta, Color.grey, Color.cyan, Color.gray, Color.red, Color.yellow, Color.green, Color.blue, Color.magenta, Color.grey, Color.cyan, Color.gray };
            reset (_widthList, _colorList, 1);
        }
        //固定两点
        public void preset_Laser_Color (string shader_) {
            usingShaderPath = shader_;
            float[] _widthList = { 0.001f, 0.01f, 0.02f, 0.016f, 0.012f, 0.008f, 0.004f, 0.0001f };
            _widthList = NumUtils.multiple (_widthList, 10.0f);
            _widthList = NumUtils.reverse (_widthList);
            Color[] _colorList = { Color.red, Color.yellow, Color.green, Color.blue, Color.magenta, Color.grey, Color.cyan, Color.white };
            reset (_widthList, _colorList, 1);
        }

        //多个固定点，每个点上停留3-4个addV2方法，然后跳转到下一个节点上。
        public void preset_Laser_Color_Shan (string shader_) {
            usingShaderPath = shader_;
            float[] _widthList = { 0.001f, 0.01f, 0.02f, 0.016f, 0.012f, 0.008f, 0.004f, 0.0001f };
            _widthList = NumUtils.multiple (_widthList, 10.0f);
            _widthList = NumUtils.reverse (_widthList);
            Color[] _colorList = { Color.red, Color.yellow, Color.green, Color.blue, Color.magenta, Color.grey, Color.cyan, Color.white };
            reset (_widthList, _colorList, 3, false);
        }

        //宽度列表，每节多少个点
        public void reset (float[] widthList_, Color[] colorList_, int perCount_ = 1, bool isConnect_ = true) {
            if (usingShaderPath == null) {
                Debug.LogError ("ERROR LineRenderMultiple reset 调用是 usingShaderPath 必须已经设置完毕");
            }
            isConnect = isConnect_;
            perCount = perCount_;
            if (isConnect == false && perCount <= 1) {
                Debug.LogError ("ERROR 在 isConnect 为 false 的情况下，perCount 只能大于1，不然看不到东西");
            }
            int _segmentCount = widthList_.Length - 1;
            _lines = new LineRendererSegment[_segmentCount]; //一共多少节 LineRender
            paths = new Vector3[_segmentCount * perCount]; //一共多少个点
            _idxLoop.reset (_segmentCount * perCount); //管理数组大小
            LineRendererSegment _line;
            for (int _idx = 1; _idx < (_segmentCount + 1); _idx++) {
                _line = CreateUtils.getComponentAndAddTo (typeof (LineRendererSegment), this) as LineRendererSegment;
                _line.initStyle(usingShaderPath,colorList_[_idx],colorList_[_idx - 1],widthList_[_idx],widthList_[_idx - 1]);//这里没调用 Start 之前，只能手动初始化一下。
                _line.gameObject.name = "line" + _idx.ToString ();
                _lines[_idx - 1] = _line;
            }
        }

        //添加一个节点<没有增删，只是通过索引来调整循环的起止位置>
        public void addV2 (Vector2 v2_, bool needConvertToWorldPos_ = true) {
            //先记录位置
            int _currentFirstIdx = _idxLoop.next ();
            if (needConvertToWorldPos_) {
                paths[_currentFirstIdx] = Camera.main.ScreenToWorldPoint (new Vector3 (v2_.x, v2_.y, 1)); //获取世界坐标值
            } else {
                paths[_currentFirstIdx] = new Vector3 (v2_.x, v2_.y, paths[_currentFirstIdx].z);
            }
            //当前用了多少个点
            int _useCount = _idxLoop.currentUseCount;
            //使用几个Render来绘画
            int _drawSegmentCount = Mathf.CeilToInt (_useCount / perCount);
            //点的使用顺序获取
            int[] _currentIdxList = _idxLoop.getCurrentIdxList ();
            //当前的Render画到第几个点
            int _loopDrawMax;
            //实际在使用顺序中的序号
            int _realIdx;
            LineRendererSegment _currentLineRender;
            for (int _idx = 0; _idx < _drawSegmentCount; _idx++) { //画几个线段
                _currentLineRender = _lines[_idx];
                if (_idx == _drawSegmentCount - 1) { //当前最后的一个
                    _loopDrawMax = _useCount % perCount; //余几个用几个
                } else { //不是最后一个就是全用
                    _loopDrawMax = perCount;
                }
                if (isConnect) { //两个节点相互连接
                    _currentLineRender.lineRenderer.positionCount = _loopDrawMax + 1; //画几个点
                } else {
                    _currentLineRender.lineRenderer.positionCount = _loopDrawMax; //画几个点
                }

                for (int _j = 0; _j < _loopDrawMax; _j++) { //怎么画
                    _realIdx = _currentIdxList[_idx * perCount + _j]; //当前使用的实际序号
                    _currentLineRender.lineRenderer.SetPosition (_j, paths[_realIdx]); //从位置队列中找出位置，进行设置。
                }

                if (isConnect) { //两个节点相互连接
                    _realIdx = _currentIdxList[_idx * perCount + _loopDrawMax];
                    _currentLineRender.lineRenderer.SetPosition (_loopDrawMax, paths[_realIdx]); //从位置队列中找出位置，进行设置。
                    if (_idx == _drawSegmentCount - 1) {
                        _currentLineRender.lineRenderer.positionCount = _loopDrawMax + 2; //画几个点
                        _currentLineRender.lineRenderer.SetPosition (_loopDrawMax + 1, paths[_currentFirstIdx]); //最后一个Render，的最后一节就是当前鼠标的位置
                    }
                }
            }
        }
    }
}