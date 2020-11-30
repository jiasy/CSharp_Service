using System;
using System.Collections;
using System.Collections.Generic;
using Objs;
using UnityEngine;
using Utils;

namespace Dis {
    public class LineRendererGoldCut : LineRendererSegment {


        public void Start () {
            initStyle("Unlit/Texture",Color.white,Color.white,0.04f,0.04f);//无光照，更快
            maxPointNum = 50; //线段的个数
        }
        public LineRendererGoldCut () {

        }

        //获得黄金分割螺旋直线------------------------------------------------------------------
        public void drawDefaultGoldCutLine () {
            resetGoldCutLine (new Vector2 (0, 0), 10.0f, 3);
        }
        public void resetGoldCutLine (Vector2 centerPos_, float beginR_, int circleCount = 3) {
            //起始10，12/4 = 3，3圈
            Vector2[] _v2s = GoldenCutUtils.getGoldCutSpiralV2s (beginR_, circleCount * 4);
            _v2s = V2Utils.trans (centerPos_, _v2s);
            reDraw (_v2s); //重新获取线段点
        }

        //获得黄金分割方形----------------------------------------------------------------------
        public void drawDefaultGoldCutRect () {
            resetGoldCutSpiralRect (new Vector2 (0, 0), 10.0f, 3);
        }
        public void resetGoldCutSpiralRect (Vector2 centerPos_, float beginR_, int circleCount = 3) {
            //起始10，12/4 = 3，3圈
            Vector2[] _v2s = GoldenCutUtils.getGoldCutRectV2s (beginR_, circleCount * 4);
            _v2s = V2Utils.trans (centerPos_, _v2s);
            reDraw (_v2s); //重新获取线段点
        }

        //获得黄金分割贝塞尔曲线------------------------------------------------------------------
        public void drawDefaultGoldCutBezier () {
            resetGoldCutBezier (new Vector2 (0, 0), 10.0f, 3);
        }
        public void resetGoldCutBezier (Vector2 centerPos_, float beginR_, int circleCount = 3) {
            //起始10，12/4 = 3，3圈
            Vector2[] _v2s = GoldenCutUtils.getGoldCutSpiralBezierV2s (beginR_, circleCount * 4);
            _v2s = V2Utils.trans (centerPos_, _v2s);
            reDraw (_v2s); //重新获取线段点
        }

        //获取螺旋曲线------------------------------------------------------------------
        public void drawDefaultGoldCutCircle () {
            resetGoldCutCircle (new Vector2 (0, 0), 10.0f, 3);
        }
        public void resetGoldCutCircle (Vector2 centerPos_, float beginR_, int circleCount = 3) {
            //起始10，12/4 = 3，3圈
            Vector2[] _v2s = GoldenCutUtils.getGoldCutSpiralCircleV2s (beginR_, circleCount * 4);
            _v2s = V2Utils.flipX (_v2s);
            _v2s = V2Utils.trans (centerPos_, _v2s);
            reDraw (_v2s); //重新获取线段点
        }
    }
}