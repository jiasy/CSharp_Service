using System.Collections.Generic;
using UnityEngine;
using System;
using Utils;
using Objs;
using Dis;

//坐标轴
public class LineRenderTest : MonoBehaviour {
    //绘画黄金分割
    public void Start () {
        // //黄金分割坐标系
        // GoldCutCoordinate _goldCutCoordinate = CreateUtils.getComponentAndAddTo (typeof (GoldCutCoordinate), this) as GoldCutCoordinate;
        // _goldCutCoordinate.gameObject.name = "goldCutCoordinate";

        // //鼠标跟随1
        // LineRendererDrawMouse _followMouse = CreateUtils.getComponentAndAddTo (typeof (LineRendererDrawMouse), this) as LineRendererDrawMouse;
        // _followMouse.gameObject.name = "followMouse";

        // //鼠标跟随2
        // LineTwoSegment _followMouse2 = CreateUtils.getComponentAndAddTo (typeof (LineTwoSegment), this) as LineTwoSegment;
        // _followMouse2.gameObject.name = "followMouse2";

        // //鼠标跟随3
        // LineRenderMultipleDrawMouse _followMouse3 = CreateUtils.getComponentAndAddTo (typeof (LineRenderMultipleDrawMouse), this) as LineRenderMultipleDrawMouse;
        // _followMouse3.gameObject.name = "followMouse3";


        //鼠标跟随4
        LineRendererPrefabDrawMouse _followMouse4 = CreateUtils.getComponentAndAddTo (typeof (LineRendererPrefabDrawMouse), this) as LineRendererPrefabDrawMouse;
        _followMouse4.gameObject.name = "followMouse3";

    }
}