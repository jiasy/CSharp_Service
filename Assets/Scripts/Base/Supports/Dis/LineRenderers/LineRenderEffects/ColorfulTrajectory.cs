using System;
using System.Collections;
using System.Collections.Generic;
using Objs;
using UnityEngine;

namespace Dis {
    public class ColorfulTrajectory : LineRenderMultiple, IMovingObj, IUpdateAbleObj {

        private MoveControlObj _leg = null;
        public MoveControlObj currentMoveControl {
            get { return _leg; }
            set { _leg = value; }
        }

        public ColorfulTrajectory () {
            //usingShaderPath = "Particles/Additive"; //粒子效果的那种
            usingShaderPath = "Particles/Additive"; //粒子效果的那种
        }

        public void Start () {
            // //激光带闪懂效果
            //preset_Laser_Color_Shan("Particles/Additive");
            //激光不闪
            //preset_Laser_Color ("Particles/Additive");
            // //勇度的追踪箭效果
            preset_Light_YongDu("Sprites/Default");
            //preset_Light_YongDu_Colorful("Particles/Additive");
            // //虚线标示抛物线
            //preset_Dotted_line ("Sprites/Default");
        }

        //设置移动控制器
        public void resetMoveControl (MoveControlObj leg_) {
            currentMoveControl = leg_;
        }
        //通过移动控制器，更新自己的位置
        public void resetByPos () {
            addV2 (_leg.pos,false);
        }
        //更新方法
        public void updateF () {
            if (_leg != null) {
                resetByPos ();
            }
        }
    }
}