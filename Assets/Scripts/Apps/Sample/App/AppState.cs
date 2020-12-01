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
    public class AppState : AppStateBase {

        public AppState (EventObserverObj ec_) : base (ec_) {
            appStateDict.Add ("Sample", new List<string> () { "Sample" });
        }

        public override void Dispose () {
            base.Dispose ();
        }
    }
}