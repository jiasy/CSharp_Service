using System;
using System.Collections;
using System.Collections.Generic;
using Dis;
using Objs;
using Service;
using UnityEngine;
using Utils;

namespace Sample {
    public class SampleSub1 : ServiceSubBase {
        public SampleSub1 (ServiceBase belongToService_) : base (belongToService_) {
            Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + " New");
        }

        public override void update (float dt_) {
            base.update (dt_);
        }

        public override void Dispose () {
             Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + " Dispose");
            base.Dispose ();
        }

        public void doSampeSub () {
            Debug.Log (System.Reflection.MethodBase.GetCurrentMethod ().DeclaringType.FullName + " -> doSampeSub");
        }
    }
}