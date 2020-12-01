using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using Utils;

// 数据对象，内置一个JsonData。可以进行数据的兼并
namespace Objs {
    public class ValueObj : BaseObj {
        public EventObserverObj eventObserver = null;//数据变化的观察者
        public JsonDataWrapObj jsonDataWrap = null;//数据保存对象
        public static void doSample () {
            //给定一个事件中心的方式创建值对象
            EventObserverObj _eventObserver = new EventObserverObj ();
            ValueObj _vo = new ValueObj (_eventObserver);
        }

        public ValueObj (EventObserverObj eventObserver_ = null) : base () {
            //可以用给定的，也可以用自己创建的
            if(eventObserver_ ==null){
                eventObserver = new EventObserverObj();
            }else{
                eventObserver = eventObserver_;
            }
            jsonDataWrap = new JsonDataWrapObj();
        }

        // 获取数据 ---------------------------------------------------------------------------------------------------------
        public string gv (string dataPath_) { //获取路径中内容，变成字符串
            return jsonDataWrap.getStringValueOnPath(dataPath_);
        }
        public string getJsonStr(string dataPath_){//路径转换成Json字符串
            JsonData _jsonDict = jsonDataWrap.getJsonDataOnPath(dataPath_);
            return JsonDataWrapObj.toJsonString(_jsonDict);
        }
        public JsonData getJsonData (string dataPath_) { //获取路径中的 JsonData
            return jsonDataWrap.getJsonDataOnPath(dataPath_);
        }
        // 设置数据 ---------------------------------------------------------------------------------------------------------
        //sv JsonData
        public void sv (string dataPath_, JsonData value_, bool dispatchEvent_ = true) {
            jsonDataWrap.setValueToDataPath(dataPath_,value_);
            if (dispatchEvent_) {
                broadcastDataChanged (jsonDataWrap.justChangeDict);
            }
        }
        //sv object
        public void sv (string dataPath_, object value_, bool dispatchEvent_ = true) {
            jsonDataWrap.setValueToDataPath(dataPath_,value_);
            if (dispatchEvent_) {
                broadcastDataChanged (jsonDataWrap.justChangeDict);
            }
        }
        //sv jsonStr
        public void sv (string dataPath_, string jsonStr_, bool dispatchEvent_ = true) {
            JsonData _jsonRoot = JsonMapper.ToObject (jsonStr_);
            sv(dataPath_, _jsonRoot, dispatchEvent_);
        }

        //广播数据变化
        public void broadcastDataChanged (Dictionary<string, JsonData> justChangeDict_) {
            foreach(string _path in justChangeDict_.Keys){
                eventObserver.Broadcast(_path,justChangeDict_[_path]);
            }
        }

        public override void Dispose () {
            eventObserver.Dispose();
            eventObserver = null;
            jsonDataWrap.Dispose();
            jsonDataWrap = null;
            base.Dispose ();
        }
    }
}