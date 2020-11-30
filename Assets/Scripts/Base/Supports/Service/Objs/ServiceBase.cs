using System;
using System.Collections;
using System.Collections.Generic;
using Dis;
using Objs;
using UnityEngine;
using Utils;

namespace Service {
    public class ServiceBase : BaseObj {
        public string serviceName = null;
        private bool _isCreated = false;
        private bool _isDestory = false;
        private List<ServiceSubBase> subServices = null;
        public ServiceBase () : base () {

        }
        public override void Dispose () {
            if (subServices != null) {//同时销毁旗下所有
                for (int _idx = 0; _idx < subServices.Count; _idx++) {
                    ServiceSubBase sub = subServices[_idx];
                    sub.Dispose ();
                }
            }

            base.Dispose ();
        }

        public virtual void create () {
            if (_isDestory) {
                Debug.LogError (this.fullClassName + "is already destoryed ~ !");
            }
            if (_isCreated) {
                Debug.LogError (this.fullClassName + "is already created ~ !");
            }
            _isCreated = true;
        }
        public virtual void destory () {
            if (!_isCreated) {
                //没有被disposed的才需要判断。
                Debug.LogError (this.fullClassName + "is not created ~ !");
            } else {
                _isDestory = true;
                Dispose ();
            }
        }

        public virtual void update (float dt_) {
            //同时更新
            if (subServices != null) {
                for (int _idx = 0; _idx < subServices.Count; _idx++) {
                    subServices[_idx].update (dt_);
                }
            }
        }

        //添加自服务
        public ServiceSubBase addSubService (ServiceSubBase serviceSub_) {
            if (subServices == null) {
                subServices = new List<ServiceSubBase> ();
            }
            if (subServices.Contains (serviceSub_)) {
                Debug.LogError (fullClassName + " 中 addSubService 当前服务已经存在 : " + serviceSub_.fullClassName);
            } else {
                subServices.Add (serviceSub_);
            }
            return serviceSub_;
        }

        //移除子服务
        public ServiceSubBase removeSubService (ServiceSubBase serviceSub_) {
            if (subServices.Contains (serviceSub_)) {
                return serviceSub_;
            } else {
                Debug.LogError (fullClassName + " 中 removeSubService 当前服务并不存在 : " + serviceSub_.fullClassName);
                return null;
            }
        }

        public ServiceSubBase getSubService (string shortName_) {
            return getSubServiceByFullClassName (fullClassName + shortName_);
        }
        //通过类名获取服务，完整类名的后半部分
        public ServiceSubBase getSubServiceByFullClassName (string className_) {
            if (subServices == null) { //还没创建过subServices缓存，肯定就没有。
                return null;
            }
            for (int _idx = 0; _idx < subServices.Count; _idx++) {
                ServiceSubBase _sub = subServices[_idx];
                if (_sub.fullClassName == className_) {
                    return _sub;
                }

            }
            return null;
        }

        // 通过子服务后缀名，拼接出子服务名称
        public ServiceSubBase setSubServiceBySuffixName(string suffixName_) {
            ServiceSubBase _sub = getSubServiceByFullClassName (fullClassName + suffixName_);
            if (_sub != null) {
                Debug.LogError (fullClassName + " setSubServiceBySuffixName : " + suffixName_ + " is already exist~!");
                return _sub;
            }
            //将自己作为构建对象的参数，传递给自服务
            object[] _parameters = new object[1];
            _parameters[0] = this;
            _sub = (ServiceSubBase) TypeUtils.getObjectByClassName (fullClassName + suffixName_, _parameters);
            return addSubService (_sub);
        }
    }
}