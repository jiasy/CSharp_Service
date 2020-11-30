using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using Utils;

// 数据对象，内置一个JsonData。可以进行数据的兼并
namespace Objs {
    public class ValueObj : BaseObj {
        public EventCenterObj ec = null;
        //数据保存对象
        public JsonData jsonRoot = null;

        private Dictionary<string, IJsonWrapper>[] _justChangeDict;

        public static void doSample () {
            //给定一个事件中心的方式创建值对象
            EventCenterObj _ec = new EventCenterObj ();
            ValueObj _vo = new ValueObj (_ec);
        }

        public ValueObj (EventCenterObj ec_ = null) : base () {
            //可以用给定的，也可以用自己创建的
            if(ec_ ==null){
                ec = new EventCenterObj();
            }else{
                ec = ec_;
            }
            jsonRoot = new JsonData (); //承载数据的对象
            jsonRoot.SetJsonType (JsonType.Object);
        }

        // 获取数据 ---------------------------------------------------------------------------------------------------------
        public string gv (string dataPath_) { //获取路径中内容，变成字符串
            string _value = JsonDataUtils.getStringFromDataPath (jsonRoot, dataPath_);
            return _value;
        }
        public JsonData go (string dataPath_) { //获取路径中的JsonData用来获取对象或者数组
            JsonData _dataOnPath = JsonDataUtils.getJsonDataFromDataPath (jsonRoot, dataPath_);
            return _dataOnPath;
        }
        // 设置数据 ---------------------------------------------------------------------------------------------------------
        //sv JsonData
        public void sv (string dataPath_, JsonData value_, bool dispatchEvent_ = true) {
             _justChangeDict = JsonDataUtils.setValueToDataPath (jsonRoot, dataPath_, value_, dispatchEvent_);
            if (dispatchEvent_) {
                broadcastDataChanged (_justChangeDict);
                _justChangeDict = null;
            }
        }
        //sv object
        public void sv (string dataPath_, object value_, bool dispatchEvent_ = true) {
            _justChangeDict = JsonDataUtils.setValueToDataPath (jsonRoot, dataPath_, value_, dispatchEvent_);
            if (dispatchEvent_) {
                broadcastDataChanged (_justChangeDict);
                _justChangeDict = null;
            }
        }
        //sv jsonStr
        public void sv (string dataPath_, string json_, bool dispatchEvent_ = true) {
            JsonData _jsonRoot = JsonMapper.ToObject (json_);
            _justChangeDict = JsonDataUtils.setValueToDataPath (jsonRoot, dataPath_, _jsonRoot, dispatchEvent_);
            if (dispatchEvent_) {
                broadcastDataChanged (_justChangeDict);
                _justChangeDict = null;
            }
        }
        public void broadcastDataChanged (Dictionary<string, IJsonWrapper>[] justChangeDict_) {
            Dictionary<string, IJsonWrapper> _typeChangeDict;
            /*
                public enum DataType
                { 
                    Int,
                    Float,
                    String,
                    Bool,
                    Array,
                    Dict
                }
            */
            _typeChangeDict = justChangeDict_[0/*DataType.Int*/];
            if(_typeChangeDict.Count>0){
                foreach(string _key in _typeChangeDict.Keys){
                    ec.Broadcast(_key,_key,_typeChangeDict[_key].GetInt());
                }
            }
            _typeChangeDict = justChangeDict_[1/*DataType.Float*/];
            if(_typeChangeDict.Count>0){
                foreach(string _key in _typeChangeDict.Keys){
                    ec.Broadcast(_key,_key,_typeChangeDict[_key].GetDouble());
                }
            }
            _typeChangeDict = justChangeDict_[2/*DataType.String*/];
            if(_typeChangeDict.Count>0){
                foreach(string _key in _typeChangeDict.Keys){
                    ec.Broadcast(_key,_key,_typeChangeDict[_key].GetString());
                }
            }
            _typeChangeDict = justChangeDict_[3/*DataType.Bool*/];
            if(_typeChangeDict.Count>0){
                foreach(string _key in _typeChangeDict.Keys){
                    ec.Broadcast(_key,_key,_typeChangeDict[_key].GetBoolean());
                }
            }
            _typeChangeDict = justChangeDict_[4/*DataType.Array*/];
            if(_typeChangeDict.Count>0){
                foreach(string _key in _typeChangeDict.Keys){
                    ec.Broadcast(_key,_key,(JsonData)_typeChangeDict[_key]);
                }
            }
            _typeChangeDict = justChangeDict_[5/*DataType.Dict*/];
            if(_typeChangeDict.Count>0){
                foreach(string _key in _typeChangeDict.Keys){
                    ec.Broadcast(_key,_key,(JsonData)_typeChangeDict[_key]);
                }
            }
        }

        public override void Dispose () {
            ec.Dispose ();
            jsonRoot.Clear ();
            base.Dispose ();
        }
    }
}