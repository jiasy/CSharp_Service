using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Objs;

namespace Game {
    public class MgrObj : BaseObj {
        public GameWorldBase gameWorld;
        public EventObserverObj eventObserver; //事件管理

        //各种管理器
        public ConfigMgrBase configMgr; //配置管理
        public GroupMgrBase groupMgr; //阵营，分组管理
        public CreationMgrBase creationMgr; //实例化管理
        public PoolMgrBase poolMgr; //池管理
        public ProcessMgrBase processMgr; //进程管理
        public LoopMgrBase loopMgr; //遍历管理
        public UpdateMgrBase updateMgr; //更新管理

        public MgrObj () : base () {

        }
        public override void Dispose () {
            //销毁关联引用关系
            gameWorld = null;
            eventObserver = null;
            configMgr = null;
            groupMgr = null;
            creationMgr = null;
            poolMgr = null;
            processMgr = null;
            loopMgr = null;
            updateMgr = null;
            base.Dispose ();
        }

        public void init () {
            //获取各种引用
            gameWorld = GameWorldBase.instance;
            ec = gameWorld.ec;
            configMgr = gameWorld.configMgr;
            groupMgr = gameWorld.groupMgr;
            creationMgr = gameWorld.insMgr;
            poolMgr = gameWorld.poolMgr;
            processMgr = gameWorld.processMgr;
            loopMgr = gameWorld.traverseMgr;
            updateMgr = gameWorld.updateMgr;
        }
    }
}