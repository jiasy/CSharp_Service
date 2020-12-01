using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Objs {
    //有状态的对象，当一个对象有状态。那么，它全程只能持有一个状态机。只不过状态机的状态在变化。
    public class StatefulObj : BaseObj {
        //状态承载体
        public StateObj state = null;
        public StatefulObj (StateObj state_ = null) : base () {
            if (state_ != null) {
                state = state_;
            } else {
                state = new StateObj ();
            }
            //监听状态变化事件
            state.eventDispatcher.AddListener<string, string> (StateObj.BEGIN_STATECHANGE, beginStateChange);
            state.eventDispatcher.AddListener<string, string> (StateObj.END_STATECHANGE, endStateChange);
        }
        public override void Dispose () {
            //移除状态变化事件监听
            state.eventDispatcher.RemoveListener<string, string> (StateObj.BEGIN_STATECHANGE, beginStateChange);
            state.eventDispatcher.RemoveListener<string, string> (StateObj.END_STATECHANGE, endStateChange);
            state.Dispose ();
            state = null;
            base.Dispose ();
        }

        //状态变化中，等待继承者实现
        public virtual void beginStateChange (string current_, string target_) {
            // if (!string.IsNullOrEmpty (current_)) {
            //     Debug.Log ("StateChanging : " + current_.ToString () + " -> " + target_.ToString ());
            // } else {
            //     Debug.Log ("Frist StateChanging : " + target_.ToString ());
            // }
        }
        public virtual void endStateChange (string last_, string current_) {
            // if (!string.IsNullOrEmpty (last_)) {
            //     Debug.Log ("StateChanged : " + last_.ToString () + " -> " + current_.ToString ());
            // } else {
            //     Debug.Log ("Frist StateChanged : " + current_.ToString ());
            // }
        }
    }
}