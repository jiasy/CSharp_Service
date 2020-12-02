using Game;
using Service;
using UnityEngine;
using Utils;
using Objs;

namespace Sample {
    public class Game : ServiceBase {
        
        public Game () : base () {

        }

        public override void create () {
            base.create ();
            Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + " create");
            new GameWorld(//创建游戏世界
                new ConfigMgr(),new GroupMgr(),new CreationMgr(),new PoolMgr(),new ProcessMgr(),new LoopMgr(),new UpdateMgr()
            );
        }

        public override void destory () {

            Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + " destory");
            base.destory ();
        }

        public override void update (float dt_) {
            base.update (dt_);
        }
    }
}