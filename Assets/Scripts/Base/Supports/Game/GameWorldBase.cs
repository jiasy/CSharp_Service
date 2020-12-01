using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using LitJson;
using Objs;
using Service;
using UnityEngine;
using Utils;

//世界循环，承载GameObj集合
//携带多个 PoolObj，用来惰性创建Obj

namespace Game {
    public class GameWorldBase : MonoBehaviour {
        public static GameWorldBase instance = null;
        //事件分发对象
        public EventDispatcherObj eventDispatcher;
        //数值变更对象
        public ValueObj valueObject;
        //各种管理器
        public ConfigMgrBase configMgr; //配置管理
        public GroupMgrBase groupMgr; //阵营，分组管理
        public CreationMgrBase insMgr; //实例化管理
        public PoolMgrBase poolMgr; //池管理
        public ProcessMgrBase processMgr; //进程管理
        public LoopMgrBase traverseMgr; //遍历管理
        public UpdateMgrBase updateMgr; //更新管理

        public GameWorldBase (
            ConfigMgrBase configMgr_,
            GroupMgrBase groupMgr_,
            CreationMgrBase insMgr_,
            PoolMgrBase poolMgr_,
            ProcessMgrBase processMgr_,
            LoopMgrBase traverseMgr_,
            UpdateMgrBase updateMgr_
        ) {
            if (GameWorldBase.instance != null) { //全局引用判断
                Debug.LogError ("GameWorld instance is already exist~!");
                return;
            }
            //全局引用
            GameWorldBase.instance = this;
            //世界的数据对象
            valueObject = new ValueObj ();
            //世界的事件分发中心
            eventDispatcher = new EventDispatcherObj();

            //世界的事件监听分发
            configMgr = configMgr_;
            groupMgr = groupMgr_;
            insMgr = insMgr_;
            poolMgr = poolMgr_;
            processMgr = processMgr_;
            traverseMgr = traverseMgr_;
            updateMgr = updateMgr_;

            //初始化
            configMgr.init ();
            groupMgr.init ();
            insMgr.init ();
            poolMgr.init ();
            processMgr.init ();
            traverseMgr.init ();
            updateMgr.init ();
        
        }

        public void Dispose () {
            valueObject.Dispose ();
            eventDispatcher.Dispose ();
            configMgr.Dispose ();
            groupMgr.Dispose ();
            insMgr.Dispose ();
            poolMgr.Dispose ();
            processMgr.Dispose ();
            traverseMgr.Dispose ();
            updateMgr.Dispose ();
            GameWorldBase.instance = null;
        }

        //重新根据给定的数据初始化
        public void initVo (JsonData jsonData_, bool dispatch_) {
            valueObject.setValueToPath ("game", jsonData_, dispatch_); //设置值对象，初始化，设置对象，可以不分发事件给监听。
        }
        public void initVo (object dataObject_, bool dispatch_) {
            valueObject.setValueToPath ("game", dataObject_, dispatch_); //设置值对象，初始化，设置对象，可以不分发事件给监听。
        }
        public void Update () {
            
        }
    }
}