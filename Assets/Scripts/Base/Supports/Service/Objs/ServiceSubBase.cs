using System;
using System.Collections;
using System.Collections.Generic;
using Dis;
using Objs;
using UnityEngine;
using Utils;

namespace Service {
    public class ServiceSubBase : BaseObj {
        public ServiceBase belongToService;
        public ServiceSubBase (ServiceBase belongToService_) : base () {
            belongToService = belongToService_;
        }
        public override void Dispose () {
            if (belongToService._disposed) {
                Debug.LogError (fullClassName + " Dispose 调用时间，所在服务应当已经调用了base.Dispose()");
            }
            base.Dispose ();
        }
        public virtual void update (float dt_) {

        }
    }
}