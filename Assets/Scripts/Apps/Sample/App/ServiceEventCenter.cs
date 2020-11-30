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
    public class ServiceEventCenter : EventCenterObj {

        public ServiceEventCenter () : base () {

        }

        public override void Dispose () {
            base.Dispose ();
        }
    }
}