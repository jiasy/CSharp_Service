using System;
using System.Collections;
using System.Collections.Generic;
using Dis;
using Objs;
using UnityEngine;
using Utils;

namespace Game {
    public class LoopMgrBase : MgrObj {
        //循环生物
        //  循环技能冷却 []
        //  循环buff [计时类型][到时间，就没有]
        //  循环被动 [持有类型][有就是有，除非没有]
        //  循环AI/玩家输入
        //  循环定位

        //循环弹丸
        //  循环特性
        //  循环定位

        //循环碰撞
        //  生物间碰撞
        //    生物 -> 生物
        //    生物 -> 地形
        //  弹丸类碰撞 
        //    弹丸 -> 生物
        //    弹丸 -> 弹丸 [另外的弹丸，可能是技能，持有判断区域，接触之后触发特效，可能是影响位移类型，也可能是吃掉]
        //      护盾类<防御弹丸，并弹射>
        //      漩涡类<影响路径，甚至吸收>
        //      斩击类<反弹弹丸，原路返回，绝地武士>
        //    弹丸 -> 地形 
        //      预算类<预算到达地形的帧数，直线不改变轨迹类，等到时间在算碰撞>
        //      临时类<技能创建的地形>
  
        

        public LoopMgrBase () : base () {
            
        }
        public override void Dispose () {
            base.Dispose ();
        }

    }
}