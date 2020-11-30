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
    //service manager
    public class App : AppBase {

        public App () : base (System.Reflection.MethodBase.GetCurrentMethod ().DeclaringType.Namespace) {
            //在切换到测试
            // changeAppState ("First", delegate (string firstStateName_) {
            //     Debug.Log ("First delegate : " + firstStateName_.ToString ());
            //     changeAppState ("Test", delegate (string testStateName_) {
            //         Debug.Log ("Test delegate : " + testStateName_.ToString ());
                    changeAppState ("Sample", delegate (string sampeStateName_) {
                        Debug.Log ("Sample delegate : " + sampeStateName_.ToString ());
                    });
            //     });
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