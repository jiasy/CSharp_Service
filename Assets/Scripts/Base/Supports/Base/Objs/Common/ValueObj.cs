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
        private List<string> _dataPathListenList = new List<string>();//监听的数据列表

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
        public void setJsonToPath (string dataPath_, string jsonStr_, bool dispatchEvent_ = true) {
            JsonData _jsonRoot = JsonMapper.ToObject (jsonStr_);
            setValueToPath(dataPath_, _jsonRoot, dispatchEvent_);
        }

        //对给定的数据进行监听
        public void addDataPathListen (string dataPath_) {
            if (_dataPathListenList.IndexOf (dataPath_) >= 0) { //监听过了
                return;
            }
            eventDispatcher.AddListener<string, JsonData> (dataPath_, dataChangeHandle);
        }
        public void removeDataPathListen (string dataPath_) {
            eventDispatcher.RemoveListener<string, JsonData> (dataPath_, dataChangeHandle);
        }
        public void addDataPathListenByList (List<string> targetDataPathList_) {
            List<string> _newDataPathListenList = ListUtils.except (targetDataPathList_, _dataPathListenList);//当前对象没有监听过的
            for (int _idx = 0; _idx < _newDataPathListenList.Count; _idx++) {
                addDataPathListen (_newDataPathListenList[_idx]);
            }
            //在当前的数据路径监听队列中加上这个新对象
            _dataPathListenList = ListUtils.union (_dataPathListenList, _newDataPathListenList);
        }
        public virtual void dataChangeHandle (string dataPath_, JsonData value_) {
            Debug.LogError ("ERROR " + System.Reflection.MethodBase.GetCurrentMethod ().ReflectedType.FullName + " -> " + new System.Diagnostics.StackTrace ().GetFrame (0).GetMethod ().Name + " : " +
                " ValueObj 数据变更响应，需要子类实现"
            );
        }
        //广播数据变化
        public void dispatchDataChanged (Dictionary<string, JsonData> justChangeDict_) {
            foreach(string _path in justChangeDict_.Keys){
                eventDispatcher.DispatchEvent(
                    _path,//事件名
                    _path,//路径
                    justChangeDict_[_path]//值
                );
            }
        }

        public override void Dispose () {
            for (int _idx = 0; _idx < _dataPathListenList.Count; _idx++) {
                removeDataPathListen (_dataPathListenList[_idx]);
            }
            eventDispatcher.Dispose();
            eventDispatcher = null;
            jsonDataWrap.Dispose();
            jsonDataWrap = null;
            base.Dispose ();
        }
    }
}