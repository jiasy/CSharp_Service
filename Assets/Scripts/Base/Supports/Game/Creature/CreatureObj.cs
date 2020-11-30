using System;
using System.Collections;
using System.Collections.Generic;
using Objs;
using UnityEngine;
using Utils;

namespace Game {
    //游戏中的生物。
    public class CreatureObj : GameObj, IUpdateAbleObj {
        public HeadObj head; //AI
        public BagObj bag; //携带道具，或者被动能力
        public BodyObj body; //Shape大小形状，携带的buff，生命值判断等等。
        public HandObj hand; //武器技能范围，冷却，攻击力信息
        public SkinObj skin; //状态机判断，直接对应显示状态
        public LegObj leg; //位置，各种力叠加对速度的影响，地形对速度的影响等等。

        public MoveControlObj _currentMoveControl; //当前控制位移的移动控制器。位移劫持，可以通过这个控制移动

        public CreatureObj (GameWorldBase gameWorld_) : base (gameWorld_) {
            head = new HeadObj (this);
            bag = new BagObj (this);
            body = new BodyObj (this);
            hand = new HandObj (this);
            skin = new SkinObj (this);
            leg = new LegObj (this);
            _currentMoveControl = leg;
        }

        public override void Dispose () {
            head.Dispose ();
            bag.Dispose ();
            body.Dispose ();
            hand.Dispose ();
            skin.Dispose ();
            leg.Dispose ();
            base.Dispose ();
        }
        
        public void updateF () {
            head.updateF (); //AI决定当前目标，策略。
            bag.updateF (); //背包决定当前附加状态
            body.updateF (); //通过背负的buff，确定状态附加值
            hand.updateF (); //刷新武器属性，技能冷却，以及释放出的可控制技能。
            skin.updateF (); //刷新显示状态，通过state来指定表现状态 
            if (_currentMoveControl == leg) { //移动能力没有被劫持的情况下。
                leg.updateF (); //根据附加值，来影响当前位置速度等 
            }
        }
    }
}