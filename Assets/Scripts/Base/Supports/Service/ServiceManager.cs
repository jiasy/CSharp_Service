using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using App;
using Dis;
using Objs;
using UnityEngine;
using Utils;

namespace Service {
    public class ServiceManager : BaseObj {
        private int currentCombinServiceTimes = 0;
        private List<ServiceBase> currentRunningServiceList = new List<ServiceBase> ();
        private ValueObj _dataCenter = null;

        //DataCenter 简写 cc.sm.dc
        public ValueObj dc {
            get { return _dataCenter; }
            set {
                if (_dataCenter == null) {
                    _dataCenter = value;
                    cc.dc = _dataCenter;//设置全局可取的变量
                } else {
                    Debug.LogError (fullClassName + " dataCenter 只能创建一次");
                }
            }
        }

        //EventCenter 简写 cc.sm.ec
        public EventDispatcherObj ec;
        public ServiceManager () : base () {
            if (cc.sm != null) {
                Debug.LogError (fullClassName + " 全局已经存在一个 cc.sm 。");
            }
            cc.sm = this;
            ec = new EventDispatcherObj();//事件分发对象
            _dataCenter = new ValueObj ();//依然支持基础的数据变化事件
        }

        public override void Dispose () {
            dc.Dispose ();
            ec.Dispose ();
            _dataCenter = null;
            ec = null;
            base.Dispose ();
        }

        //通过服务名称，添加运行服务
        public ServiceBase addServiceByName (string serviceName_) {
            ServiceBase _s = createServiceByName (serviceName_);
            currentRunningServiceList.Add (_s);
            return _s;
        }

        //通过服务名移除服务
        public ServiceBase removeServiceByName (string serviceName_) {
            ServiceBase _s = getServiceByName (serviceName_);
            if (_s != null) { //找到就移除，并返回
                currentRunningServiceList.Remove (_s); //移除这个服务
                return _s;
            } else { //找不到返回空
                return null;
            }
        }

        //通过服务名称创建服务
        public ServiceBase createServiceByName (string serviceName_) {
            ServiceBase _s = (ServiceBase) TypeUtils.getObjectByClassName (cc.app.appName + "."+serviceName_);
            _s.serviceName = serviceName_;
            return _s;
        }

        //通过名称获取当前运行的服务
        public ServiceBase getServiceByName (string serviceName_) {
            for (int _idx = 0; _idx < currentRunningServiceList.Count; _idx++) {
                ServiceBase _s = currentRunningServiceList[_idx];
                if (_s.serviceName == serviceName_) {
                    return _s;
                }
            }
            return null;
        }

        // 通过当前目标的服务列表，清理掉 目标服务组合中，不存在的服务
        public List<ServiceBase> switchRunningServices (List<string> serviceNameList_) {
            //重新组合的次数加一
            currentCombinServiceTimes = currentCombinServiceTimes + 1;
            //当前正在运行的服务名称集合
            List<string> _runningServiceNames = currentRunningServiceList.Select (_runningService => { return _runningService.serviceName; }).ToList ();
            //当前要切换的服务名称集合
            List<string> _targetServiceNames = serviceNameList_;
            //不在要切换的目标内的当前服务
            List<string> _removeServiceNames = _runningServiceNames.Except (_targetServiceNames).ToList ();
            //获取要移除的服务
            List<ServiceBase> _removeServices = _removeServiceNames.Select (_removeServiceName => { return removeServiceByName (_removeServiceName); }).ToList ();
            //实际执行移除操作
            _removeServices.ForEach (_removeService => _removeService.destory ());
            //在切换目标内但不在正在运行的服务集合
            List<string> _createServiceList = _targetServiceNames.Except (_runningServiceNames).ToList ();
            //创建并添加服务
            List<ServiceBase> _createServices = _createServiceList.Select (_createServiceName => { return addServiceByName (_createServiceName); }).ToList ();
            //实际执行移除操作
            _createServices.ForEach (_createService => _createService.create ());
            //返回创建出来的数组
            return _createServices;
        }
        public void update (float dt_) {
            for (int _idx = 0; _idx < currentRunningServiceList.Count; _idx++) {
                currentRunningServiceList[_idx].update (dt_);
            }
        }
    }
}