using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dis;
using Objs;
using UnityEngine;
using Utils;

namespace App {
    //service manager
    public class AppConstBase : BaseObj {
        public int TIME_NEVER_EXPIRE = -1;
        public int TIME_FIVE_SECONDS = 5;
        public int TIME_TEN_SECONDS = 10;
        public int TIME_HALF_MINUTE = 30;
        public int TIME_ONE_MINUTE = 60;
        public int TIME_TWO_MINUTES = 120;
        public int TIME_TEN_MINUTES = 600;
        public int TIME_ONE_HOUR = 3600;
        public int TIME_ONE_DAY = 86400;
        public int TIME_ONE_WEEK = 604800;
        public int TIME_TWO_WEEKS = 1209600;
        public int TIME_THREE_WEEKS = 1814400;

        public bool isMobile = false;

        public AppConstBase () : base () {
            //当前是不是手机，测试的时候，可以修改这个值
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
                isMobile = true;
            } else {
                isMobile = false;
            }
        }

        public override void Dispose () {
            base.Dispose ();
        }
    }
}