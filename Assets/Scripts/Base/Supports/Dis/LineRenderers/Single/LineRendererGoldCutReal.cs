using System;
using System.Collections;
using System.Collections.Generic;
using Objs;
using UnityEngine;
using Utils;

namespace Dis {
    public class LineRendererGoldCutReal : LineRendererSegment {


        public void Start () {
            initStyle("Unlit/Texture",Color.white,Color.white,0.04f,0.04f);//无光照，更快
            maxPointNum = 50; //线段的个数
        }

        public LineRendererGoldCutReal () {

        }

        //获得黄金分割直线------------------------------------------------------------------
        public void drawDefaultRect () {
            resetRect (new Vector2 (0, 0), 10.0f, 3);
        }
        public void resetRect (Vector2 centerPos_, float beginR_, int circleCount = 3) {
            //起始10，12/4 = 3，3圈
            Vector2[] _v2s = GoldenCutUtils.getRectV2sReal (beginR_, circleCount * 4);
            _v2s = V2Utils.trans (centerPos_, _v2s);
            reDraw (_v2s); //重新获取线段点
        }

        //获得黄金分割螺旋------------------------------------------------------------------
        public void drawDefaultCruve () {
            resetCruve (new Vector2 (0, 0), 10.0f, 3);
        }
        public void resetCruve (Vector2 centerPos_, float beginR_, int circleCount = 3) {
            //起始10，12/4 = 3，3圈
            Vector2[] _v2s = GoldenCutUtils.getCruveV2sReal (beginR_, circleCount * 4);
            _v2s = V2Utils.trans (centerPos_, _v2s);
            reDraw (_v2s); //重新获取线段点
        }
    }
}