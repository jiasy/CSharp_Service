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
    public class AppBase : StatefulObj {
        public ServiceManager sm = null;//服务控制器
        public AppConstBase appConst = null;//常量引用
        public string appName = null;//app名称
        private float _dt = 0.0f; // 帧时间累计
        private float _frameUpdateDt; //按照每秒帧数，计算每帧时间
        private int _framePerSecond;//每秒帧数
        public int framePerSecond {
            get {
                return _framePerSecond;
            }
            set {//调整帧频时间
                _framePerSecond = value;
                _frameUpdateDt = 1.0f / (float) _framePerSecond;
            }
        } // 帧数

        public AppBase (string appName_) : base (
            (AppStateBase) TypeUtils.getObjectByClassName (
                appName_ + ".AppState",
                new object[1]{
                    new EventCenterObj()
                }
            )
        ) {
            if (cc.app != null) {
                Debug.LogError ("ERROR " + System.Reflection.MethodBase.GetCurrentMethod ().ReflectedType.FullName + " -> " + new System.Diagnostics.StackTrace ().GetFrame (0).GetMethod ().Name + " : " +
                    " 全局已经存在一个 cc.app 。"
                );
            }

            cc.app = this;
            appName = appName_;//获取App名称
            appConst = (AppConstBase) TypeUtils.getObjectByClassName (appName_ + ".AppConst");//app常量

            sm = new ServiceManager ();
            framePerSecond = 20; //逻辑更新帧数
            Application.targetFrameRate = 20; //Unity显示帧数
            //优化性能 工程层面来降低 Edit->Project Settings->Time 帧数
            // 刚体部件，可以启用插值办法来平滑刚体组件的运动
        }

        public override void Dispose () {

            appConst.Dispose ();
            base.Dispose ();
        }

        public void update (float dt_) {
            _dt = _dt + dt_;
            if (_dt > _frameUpdateDt) {
                sm.update (_dt);//与上一帧的时间间隔。
                _dt = -(_dt - _frameUpdateDt);//超过的部分要从下一帧的时间间隔内刨除
            }
        }

        //添加一个代理方法
        /*
            cc.app.changeAppState("state",null);
            cc.app.changeAppState("state",delegate(string currentStateName_){
                //处理
            });
        */
        public void changeAppState (string stateName_, System.Action<string> action) {
            if (state.stateChangeBegin (stateName_) != null) { //状态开始改变
                sm.switchRunningServices ((state as AppStateBase).appStateDict[stateName_]);
                state.stateChangeEnd (); //结束状态变化
                if (action != null) { //有回调，回调参数是 string，之后执行
                    action.Invoke (stateName_);
                }
            }
        }
        public override void beginStateChange (string current_, string target_) {
            base.beginStateChange (current_, target_);
        }
        public override void endStateChange (string last_, string current_) {
            base.endStateChange (last_, current_);
        }
    }
}