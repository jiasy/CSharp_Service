using Game;
using Service;
using UnityEngine;
using Utils;
using Objs;

namespace Sample {
    public class Sample : ServiceBase {
        public Sample () : base () {

        }

        public override void create () {
            base.create ();
            Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + " create");
            //添加一个子服务
            setSubServiceBySuffixName ("Sub1");
            SampleSub1 _sub1 = (SampleSub1) getSubService ("Sub1");
            _sub1.doSampeSub ();

            //测试 JsonDataUtils
            JsonDataWrapObj.doSample();

            //测试 EventObserverObj
            GameObj.doSample();

            //设置测试对象
            GameObj _obj = new GameObj(null);
            //设置一下hp值，是否触发相应反应。
            _obj.setValueToPath ("hp", -1);
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