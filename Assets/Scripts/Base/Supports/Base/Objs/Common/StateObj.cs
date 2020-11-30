using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Objs {
    //状态机对象
    public class StateObj : BaseObj {
        //状态变化事件定义
        public static string BEGIN_STATECHANGE = "StateChanging";
        public static string END_STATECHANGE = "StateChanged";
        public string currentState; //当前状态
        public string lastState; //上一个状态
        public string targetState; //目标状态
        public EventCenterObj ec; //事件分发

        public StateObj (EventCenterObj ec_ = null) : base () {
            currentState = null;
            lastState = null;
            targetState = null;
            //可以用给定的，也可以用自己创建的
            if (ec_ == null) {
                ec = new EventCenterObj ();
            } else {
                ec = ec_;
            }
        }
        public override void Dispose () {
            ec.Dispose ();
            base.Dispose ();
        }
        //立刻改变状态，然后，立刻结束。
        public bool stateChangeRightaway (string targetState_) {
            if (stateChangeBegin (targetState_) != null) { //成功开始改变状态
                stateChangeEnd (); //状态改变结束
                return true;
            }
            return false;
        }


        //状态变化中
        public string stateChangeBegin (string stateName_) {
            if (stateName_ == currentState) { //状态没变
                return null;
            }
            if (stateName_ == targetState) {
                return targetState;
            }
            targetState = stateName_;
            if (ec != null) {
                ec.Broadcast (StateObj.BEGIN_STATECHANGE, currentState, targetState);
            }
            return targetState;
        }
        //状态变化结束
        public string stateChangeEnd () {
            if (targetState == null) { //状态变化已经结束
                return null;
            }
            lastState = currentState; //当前状态记录成上一个状态
            currentState = targetState;
            targetState = null;
            if (ec != null) {
                ec.Broadcast (StateObj.END_STATECHANGE, lastState, currentState);
            }
            return currentState;
        }
    }
}