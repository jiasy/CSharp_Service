using System;
using System.Collections;
using System.Collections.Generic;
using Dis;
using Objs;
using Service;
using UnityEngine;
using Utils;
using Game;

namespace Sample {
    public class GameWorld : GameWorldBase {
        public GameWorld(
            ConfigMgrBase configMgr_,GroupMgrBase groupMgr_,CreationMgrBase insMgr_,PoolMgrBase poolMgr_,ProcessMgrBase processMgr_,LoopMgrBase traverseMgr_,UpdateMgrBase updateMgr_
        ) : base (
            configMgr_,groupMgr_,insMgr_,poolMgr_,processMgr_,traverseMgr_,updateMgr_
        ) {
            
        }
        public override void Dispose () {
            base.Dispose ();
        }

        public override void Update (float dt_) {
            
        }
    }
}