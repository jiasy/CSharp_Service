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

namespace App {
    //App的承载组件
    public class AppComponentBase : MonoBehaviour {
        protected void initApp(string namespace_){//通过
            TypeUtils.getObjectByClassName (namespace_+".App");
            DontDestroyOnLoad (gameObject);//切换场景不删除当前APP所挂载的GameObject
        }
        // Update is called once per frame
        void Update () {
//            if (Time.frameCount % 50 == 0) { //
//                System.GC.Collect ();
//            }
            //时间变化传递个更新函数
            //cc.app.update(Time.deltaTime);
        }
    }
}