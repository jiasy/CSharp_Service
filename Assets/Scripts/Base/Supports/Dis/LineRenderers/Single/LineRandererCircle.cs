using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Dis {
    public class LineRendererCircle : LineRendererSegment {

        public void Start () {
            initStyle("Unlit/Texture", Color.white, Color.white,0.04f,0.04f);//无光照，更快
            maxPointNum = 50; //线段的个数
        }

        public void drawDefaultCircle () {
            resetCircle (new Vector2 (0, 0), 100);
        }

        public void resetCircle (Vector2 centerPos_, int r_, float percent_ = 1.0f) {
            if (percent_ >= 1.0f) {
                lineRenderer.loop = true; //闭环
            } else {
                lineRenderer.loop = false; //半圆开环
            }
            Vector2[] _v2s = CircleUtils.getCirclePointsVec2 (
                centerPos_, //圆心
                r_, //半径
                0, //分割圆的点个数，0为采用计算的方式得出合适的值
                0, //等距螺旋距离
                percent_//圆周百分比
            );
            reDraw (_v2s); //重新获取圆周点
        }

        public void drawDefaultEquidistantSpirals () {
            resetEquidistantSpirals (new Vector2 (0, 0), 10, 100, 1);
        }

        public void resetEquidistantSpirals (Vector2 centerPos_, int r_, int buffer_, float percent_ = 1.0f) {
            lineRenderer.loop = false; //半圆开环，分割数传递0，按照默认规则分割。
            Vector2[] _v2s = CircleUtils.getCirclePointsVec2 (
                centerPos_, //圆心
                r_, //半径
                0, //分割圆的点个数，0为采用计算的方式得出合适的值
                buffer_, //等距螺旋距离
                percent_ //圆周百分比
                );
            reDraw (_v2s); //重新获取圆周点
        }
    }
}