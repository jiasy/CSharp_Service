using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using App;
using Dis;
using Objs;
using Service;
using UnityEngine;
using Utils;

namespace Sample {
    public class App : AppBase {
        // 在哪个命名空间下就是那个App
        public App () : base (System.Reflection.MethodBase.GetCurrentMethod ().DeclaringType.Namespace) {
            //在切换到测试
            // changeAppState ("First", delegate (string firstStateName_) {
            //     Debug.Log ("First delegate : " + firstStateName_.ToString ());
                changeAppState ("Sample", delegate (string testStateName_) {
                    Debug.Log ("Sample delegate : " + testStateName_.ToString ());
                    changeAppState ("Game", delegate (string sampeStateName_) {
                        Debug.Log ("Game delegate : " + sampeStateName_.ToString ());
                    });
                 });
            // });
        }

        public override void Dispose () {

            //底层消除
            base.Dispose ();
        }

        //监听App的变化事件
        public override void beginStateChange (string current_, string target_) {

            base.beginStateChange (current_, target_);
        }
        public override void endStateChange (string last_, string current_) {

            base.endStateChange (last_, current_);
        }
    }
}