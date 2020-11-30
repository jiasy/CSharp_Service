using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;
using Objs;
using Dis;

public class LineRenderMultipleDrawMouse : LineRenderMultiple {
    public void Start () {
        // //激光带闪懂效果
        //preset_Laser_Color_Shan("Particles/Additive");
        //激光不闪
        preset_Laser_Color ("Particles/Additive");
        // //勇度的追踪箭效果
        //preset_Light_YongDu("Sprites/Default");
        //preset_Light_YongDu_Colorful("Sprites/Default");
        // //虚线标示抛物线
        // preset_Dotted_line ("Sprites/Default");
    }

    protected void Update () {
        if (Input.GetMouseButton (0)) { //鼠标移动
            addV2 (Input.mousePosition);
        }
    }
}