using System;
using System.Collections;
using System.Collections.Generic;
using Dis;
using LitJson;
using UnityEngine;
using Utils;

namespace Objs {
    //显示对象的控制
    public class DisplayControlObj : StatefulObj {
        //state 控制状态，通过事件分发控制显示。
        /*
            当然，状态也分 : 
                子弹状态
                特效状态
                生物状态
                Boss状态
                Hero状态
        */
        // _displayGameObject 实际的显示对象
        /* 
            显示对象要有同步状态的能力
                子弹类，发射，击打墙壁，击打人物，精致浮空落地
                特效类，弹壳，升级 恢复 光效，表情等等
                生物类，生死站行跳倒飞伤控
                Boss状态，生物的基础上，技能，特殊动画
                Hero状态，Boss的基础上，特殊待机，武器更换，升级。
        */
        private GameObject _displayGameObject = null;
        //创建的时候要设置显示对象
        //显示对象都是gameObject
        public DisplayControlObj (GameObject go_, StateObj stateObj_ = null) : base (stateObj_) {
            _displayGameObject = go_;//引用要控制的对象
        }
        public override void Dispose () {
            _displayGameObject = null;
            base.Dispose ();
        }
    }
}