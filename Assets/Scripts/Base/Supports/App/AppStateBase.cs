using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dis;
using Objs;
using Service;
using UnityEngine;
using Utils;

namespace App {
    //service manager
    public class AppStateBase : StateObj {
        public Dictionary<string, List<string>> appStateDict = new Dictionary<string, List<string>> ();
        //需要监听者来监听状态变化
        public AppStateBase (EventCenterObj ec_) : base (ec_) {

        }

        public override void Dispose () {
            appStateDict.Clear();
            base.Dispose ();
        }
    }
}