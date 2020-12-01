using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using Utils;

// 数据对象，内置一个JsonData。可以进行数据的兼并
namespace Objs {
    public class ValueObj : BaseObj {
        public EventDispatcherObj eventDispatcher = null;//数据变化的观察者
        public JsonDataWrapObj jsonDataWrap = null;//数据保存对象
        
        public static void doSample () {
            //给定一个事件中心的方式创建值对象
            EventDispatcherObj _eventDispatcher = new EventDispatcherObj ();
            ValueObj _vo = new ValueObj (_eventDispatcher);
        }

        public ValueObj (EventDispatcherObj eventDispatcher_ = null) : base () {
            //可以用给定的，也可以用自己创建的
            if(eventDispatcher_ ==null){
                eventDispatcher = new EventDispatcherObj();
            }else{
                eventDispatcher = eventDispatcher_;
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
        public void setValueToPath (string dataPath_, JsonData value_, bool dispatchEvent_ = true) {
            jsonDataWrap.setValueToPath(dataPath_,value_);
            if (dispatchEvent_) {
                dispatchDataChanged (jsonDataWrap.justChangeDict);
            }
        }
        //sv object
        public void setValueToPath (string dataPath_, object value_, bool dispatchEvent_ = true) {
            jsonDataWrap.setValueToPath(dataPath_,value_);
            if (dispatchEvent_) {
                dispatchDataChanged (jsonDataWrap.justChangeDict);
            }
        }
        //sv jsonStr
        public void setValueToPath (string dataPath_, string jsonStr_, bool dispatchEvent_ = true) {
            JsonData _jsonRoot = JsonMapper.ToObject (jsonStr_);
            setValueToPath(dataPath_, _jsonRoot, dispatchEvent_);
        }

        //注册监听事件
        public void AddListener<T, X> (string evtName_, CallBack<T, X> callBack_) {
            eventDispatcher.AddListener<T,X>(evtName_, callBack_);
        }

        //移除监听事件
        public void RemoveListener<T, X> (string evtName_, CallBack<T, X> callBack_) {
            eventDispatcher.RemoveListener<T, X>(evtName_, callBack_);
        }

        //分发数据变化事件
        public void dispatchDataChanged (Dictionary<string, JsonData> justChangeDict_) {
            foreach(string _path in justChangeDict_.Keys){
                eventDispatcher.DispatchEvent(
                    _path,//路径 即是 事件名。用事件名 来 索引 事件。
                    //监听方法，需要传递路径和路径上的数据，以便进行逻辑判断操作。
                    _path,/* T */justChangeDict_[_path]/* X */
                );
            }
        }

        public override void Dispose () {
            eventDispatcher.Dispose();
            eventDispatcher = null;
            jsonDataWrap.Dispose();
            jsonDataWrap = null;
            base.Dispose ();
        }
    }
}