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
        
        public static GameWorld getGameWorld(){
            return new GameWorld(
                new ConfigMgr(),
                new GroupMgr(),
                new CreationMgr(),
                new PoolMgr(),
                new ProcessMgr(),
                new LoopMgr(),
                new UpdateMgr()
            );
        }
        public GameWorld(
            ConfigMgrBase configMgr_,
            GroupMgrBase groupMgr_,
            CreationMgrBase insMgr_,
            PoolMgrBase poolMgr_,
            ProcessMgrBase processMgr_,
            LoopMgrBase traverseMgr_,
            UpdateMgrBase updateMgr_
        ) : base (
            configMgr_,
            groupMgr_,
            insMgr_,
            poolMgr_,
            processMgr_,
            traverseMgr_,
            updateMgr_
        ) {

        }

        public void update (float dt_) {
            
        }
    }
}