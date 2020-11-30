using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Dis {
    public class LineRendererRect : LineRendererSegment {

        public void Start () {
            initStyle ("Unlit/Texture", Color.white, Color.white, 0.04f, 0.04f); //无光照，更快
            maxPointNum = 50; //线段的个数
        }

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>

        public void drawDefaultRect () {
            resetRect (new Vector2 (0, 0), 100, 200);
        }

        public void resetRect (Vector2 centerPos_, int width_, int height_) {
            lineRenderer.loop = true; //闭环
            Vector2[] _4v2;
            if (width_ > height_) {
                _4v2 = CircleUtils.getV2s (height_, 8);
                _4v2 = V2Utils.interval (2, _4v2, 1);
                _4v2 = V2Utils.scaleX ((float) width_ / (float) height_, _4v2);
            } else {
                _4v2 = CircleUtils.getV2s (width_, 8);
                _4v2 = V2Utils.interval (2, _4v2, 1);
                _4v2 = V2Utils.scaleY ((float) height_ / (float) width_, _4v2);
            }
            reDraw (_4v2); //重新绘画
        }

        //画正方形
        public void resetRectSquare (Vector2 centerPos_, int width_) {
            resetRect (centerPos_, width_, width_);
        }
    }
}