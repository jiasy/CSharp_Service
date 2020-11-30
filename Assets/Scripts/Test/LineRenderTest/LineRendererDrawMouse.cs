using System;
using System.Collections;
using System.Collections.Generic;
using Dis;
using Objs;
using UnityEngine;

public class LineRendererDrawMouse : LineRendererSegment {

    public LineRendererDrawMouse () {

    }

    public void Start () {
        initStyle ("Unlit/Texture", Color.red, Color.yellow, 0.1f, 0.01f); //无光照，更快
        maxPointNum = 15; //线段的个数
    }

    protected void Update () {
        if (Input.GetMouseButton (0)) { //鼠标移动
            addV2 (Input.mousePosition);
        }
    }
    void OnGUI () {
        GUILayout.Label (" ");
        GUILayout.Label (" ");
        GUILayout.Label (" ");
        GUILayout.Label ("当前鼠标X轴位置：" + Input.mousePosition.x);
        GUILayout.Label ("当前鼠标Y轴位置：" + Input.mousePosition.y);
    }
}